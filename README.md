# Authentication System

This authentication system is built using .NET 8 and incorporates roles-permissions functionality to provide secure access control within your application. It follows the principles of Clean Architecture to ensure maintainability and scalability.

## Features

- **User Authentication**: Users can securely sign up, sign in, and sign out of the system.
- **Role-Based Access Control (RBAC)**: Administrators can assign roles to users, granting them specific permissions based on their role.
- **Permissions Management**: Granular control over what actions users can perform within the application based on their assigned roles.
- **Password Encryption**: User passwords are encrypted to ensure security.
- **Logging**: Comprehensive logging of authentication and authorization events for auditing purposes.
- **Token-Based Authentication**: Uses tokens to authenticate and authorize users, enhancing security and scalability.
- **Session Management**: Management of user sessions to track active sessions and handle session expiration.


## Installation

1. Clone the repository:

```bash
git clone https://github.com/ElohimCode/AuthSystem.git
```

2. Navigate to the project directory:

```bash
cd authentication-system
```

3. Install dependencies:

```bash
dotnet restore
```

4. Run the application:

```bash
dotnet run
```

or 
- Load the dependencies directly using visual studio

## Configuration

- **Database Connection**: Configure the database connection string in `appsettings.json` to connect to your database.
- **Roles and Permissions**: Define roles and their corresponding permissions in the application settings or database.
- **Token Configuration**: Adjust token expiration time, secret key, and other settings in `appsettings.json`.

## Usage

1. Register a new user with the desired role.
2. Sign in with the registered user credentials.
3. Access different features and functionalities based on the assigned role and permissions.
4. Administrators can manage roles, permissions, and user accounts as needed.

## Contributors

- [JB](https://github.com/ElohimCode)

## License

This project is licensed under the [MIT License](LICENSE).
