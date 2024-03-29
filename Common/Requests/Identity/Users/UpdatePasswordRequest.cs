﻿namespace Common.Requests.Identity.Users
{
    public class UpdatePasswordRequest
    {
        public string Email { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmedNewPassword { get; set; } = null!;
    }
}
