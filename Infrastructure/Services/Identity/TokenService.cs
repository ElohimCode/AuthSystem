﻿using Application.Contracts.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Application.AppConfigs;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Cryptography;
using Common.Utilities;
using static Common.Utilities.Constants;




namespace Infrastructure.Services.Identity
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppConfiguration _appConfiguration;

        public TokenService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<AppConfiguration> appConfiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appConfiguration = appConfiguration.Value;
        }

        public async Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest request)
        {
            // Validate user
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Credentials.");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("User is not active. Please contact the administrator.");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("Email is not confirmed.");
            }

            // Check for password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Credentials.");
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTimeUtility.AddTime(DateTimeConstants.RefreshTokenExpiry);
            // Update refresh token
            await _userManager.UpdateAsync(user);

            var token = await GenerateJWTAsync(user);

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry
            };
            return await ResponseWrapper<TokenResponse>.SuccessAsync(response);
        }

        public async Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest request)
        {
            if (request is null)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("Invalid client token.");
            }

            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("User not found.");
            }
            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry <= DateTime.Now)
            {
                return await ResponseWrapper<TokenResponse>.FailAsync("Invalid client token.");
            }
            var token = GenerateEncrytedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry
            };
            return await ResponseWrapper<TokenResponse>.SuccessAsync(response);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken 
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }
            return principal;
        }

       
        private async Task<string> GenerateJWTAsync(ApplicationUser user)
        {
            var token = GenerateEncrytedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }
        private string GenerateEncrytedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTimeUtility.AddTime(_appConfiguration.TokenExpiryInMinutes),
                signingCredentials: signingCredentials
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfiguration.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }


        private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var currentRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForCurrentRole = await _roleManager.GetClaimsAsync(currentRole);
                permissionClaims.AddRange(allPermissionsForCurrentRole);
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Name, user.FirstName),
                new (ClaimTypes.Surname, user.LastName),
                new (ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);
            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
