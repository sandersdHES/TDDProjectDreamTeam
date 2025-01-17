# Unit Test Documentation

Welcome to our **TDD Project** for the University Service Card system!

This project aims to provide a comprehensive user management system with functionalities for role management, user registration, profile management, and user authentication. It is built using .NET and follows Test-Driven Development (TDD) principles.

This will be useful to create a fully functional user service, containing the capabilities of managing the aforementionned tasks.

**Contributors:**  
Dylan Sanderson, Johann Von Rotten, Maxime Bossi, Nadja Lötscher

## Summary

- [Setup instructions](#setup-instructions)
- [Module Overview](#module-overview)
- [Repository and models](#repository-and-models)
- [Role Management](#role-management)
- [User Registration](#user-registration)
- [Profile Management](#profile-management)
- [User Authentication](#user-authentication)
- [User Service](#user-service)

## Setup instructions

### Prerequisites

1. **Visual Studio 2022**: Ensure you have Visual Studio 2022 installed. You can download it from [here](https://visualstudio.microsoft.com/vs/).
2. **.NET SDK**: Make sure you have the .NET SDK installed. You can download it from [here](https://dotnet.microsoft.com/download).

### Cloning the Repository

1. Open Visual Studio 2022.
2. Go to `File` > `Clone Repository`.
3. Enter the repository URL and choose a local path to clone the repository.

### Building the Project

1. Open the solution file (`.sln`) in Visual Studio 2022.
2. Go to `Build` > `Build Solution` or press `Ctrl+Shift+B`.

### Running the Tests

1. Open the Test Explorer in Visual Studio by going to `Test` > `Test Explorer`.
2. Click on `Run All` to execute all the tests.

### Running the Application

1. Set the startup project by right-clicking on the project in the Solution Explorer and selecting `Set as Startup Project`.
2. Press `F5` to run the application.

### Tools and Libraries
- **Testing Framework**: xUnit, Moq
- **Code Coverage**: Fine Code Coverage
- **Version Control**: Git

### Additional remarks

- **NuGet Packages**: Ensure all required NuGet packages are installed. You can restore them by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.
- **Code Analysis**: Install and follow the process for [Fine Code Coverage](https://github.com/FortuneN/FineCodeCoverage).

## Module Overview

The **User Management Module** includes these responsibilities:
1. **User Registration**: Securely register users by validating details.
2. **User Authentication**: Authenticate users with hashed passwords and manage account access.
3. **Profile Management**: Enable users to update their profile information securely.
4. **Role Management**: Assign, update, and verify roles for users, including managing permissions.

The module forms part of the broader **University Service Card system**, which integrates multiple modules like room access, meal payment, and library services.

## Repository and models

To replicate the gathering of data and information in an API architecture, we have implemented models and repositories.

The models will reflect the objects that we will need to obtain and use to manage the users in our application. The repositories on the other hand will act as the database, from which we will query to obtain the required information depending on the service used.

You will now see in details our chosen models and repositories.

### User Model

The `User` model represents a user in the system. It contains the following properties:
- `Id`: A unique identifier for the user.
- `Name`: The name of the user.
- `Email`: The email address of the user.
- `PasswordHash`: The hashed password of the user.
- `Role`: The role assigned to the user.

We have used 2 specific constructors, since we will only need connection informations like email and password during registration or authentication.

```csharp
public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }

    public User(string id, string name, Role role)
    {
        Id = id;
        Name = name;
        Role = role;
    }

    public User(string id, string name, string email, string passwordHash, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
```

### Role Model

The `Role` model represents a role in the system. It contains the following properties:
- `Name`: The name of the role.
- `Permissions`: A list of permissions associated with the role.

It comes with a set of methods to manipulate the permissions. No permission by default are being used in our services.

```csharp
public class Role
{
    public string Name { get; set; }
    public List<string> Permissions { get; set; }

    public Role(string name, List<string> permissions)
    {
        Name = name;
        Permissions = permissions ?? new List<string>();
    }

    public bool HasPermission(string feature)
    {
        return Permissions.Contains(feature, StringComparer.OrdinalIgnoreCase);
    }

    public void AddPermission(string feature)
    {
        Permissions.Add(feature);
    }

    public void RemovePermission(string feature)
    {
        Permissions.Remove(feature);
    }
}
```

### User Repository

The `UserRepository` class is responsible for managing user data. It provides methods to add, remove, update, and retrieve users.

```csharp
public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();

    public User GetUser(string userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId)
               ?? throw new KeyNotFoundException($"User '{userId}' not found.");
    }

    public void AddUser(User user)
    {
        if (_users.Any(u => u.Id == user.Id))
            throw new InvalidOperationException($"User '{user.Id}' already exists.");
        _users.Add(user);
    }

    public void RemoveUser(string userId)
    {
        var user = GetUser(userId);
        _users.Remove(user);
    }
      
    public bool UserExists(string userId)
    {
        return _users.Any(u => u.Id == userId);
    }

    public IEnumerable<User> GetUsers() {
        return _users;
    }

    public User GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email)
               ?? throw new KeyNotFoundException($"User with email '{email}' not found.");
    }

    public void UpdateUser(User updatedUser)
    {
        var existingUser = GetUser(updatedUser.Id);

        if (existingUser == null)
            throw new KeyNotFoundException($"User '{updatedUser.Id}' not found for update.");

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.PasswordHash = updatedUser.PasswordHash;
        existingUser.Role = updatedUser.Role;
    }
}
```

### Role Repository

The `RoleRepository` class is responsible for managing role data. It provides methods to add, remove, and retrieve roles.

```csharp
public class RoleRepository : IRoleRepository
{
    private readonly List<Role> _roles = new List<Role>();

    public Role GetRole(string roleName)
    {
        return _roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Role '{roleName}' not found.");
    }

    public void AddRole(Role role)
    {
        if (_roles.Any(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Role '{role.Name}' already exists.");
        _roles.Add(role);
    }

    public void RemoveRole(string roleName)
    {
        var role = GetRole(roleName);
        _roles.Remove(role);
    }

    public bool RoleExists(string roleName)
    {
        return _roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Role> GetAllRoles()
    {
        return _roles;
    }
}
```

## Role Management

The Role Management Service is responsible for handling all operations related to roles within the system. The goal of this service is to ensure that roles are managed efficiently and securely, providing the necessary permissions to users based on their roles.

The `IRoleManagementService` interface defines the following methods:

| Method Name               | Description                                                                 |
|---------------------------|-----------------------------------------------------------------------------|
| `CreateRole(Role role)`   | Creates a new role in the system.                                            |
| `AssignRole(string userId, Role role)` | Assigns a specified role to a user.                                |
| `IsRoleValid(Role role)`  | Checks if a given role is valid.                                             |
| `HasAccess(string userId, string feature)` | Checks if a user has access to a specific feature based on their role. |
| `UpdateUserRole(string adminId, string userId, Role newRole)` | Updates the role of a user, typically performed by an admin. |

### Test Cases

**Key Tests**:
- Role creation succeeds for valid roles and fails for duplicates.
- Permissions are verified for roles like `Admin` and `Staff`.
- Role updates are restricted for critical roles (e.g., cannot remove the last `Admin`).

> Only the method to check if a role is valid is not directly tested here, since it is a helper method used in other methods of the same service.

1. **Role Creation**:
   - **CreateRole_ValidRole_ShouldSucceed**: Verifies that creating a valid role succeeds.
   - **CreateRole_DuplicateRole_ShouldFail**: Verifies that creating a duplicate role fails.
   - **CreateRole_EmptyName_ShouldThrowException**: Verifies that creating a role with an empty name throws an exception.
   - **CreateRole_NullRole_ShouldThrowException**: Verifies that creating a null role throws an exception.

2. **Role Assignment**:
   - **AssignRole_ValidRole_ShouldSucceed**: Verifies that assigning a valid role to a user succeeds.
   - **AssignRole_InvalidRole_ShouldFail**: Verifies that assigning an invalid role fails.
   - **AssignRole_EmptyUserId_ShouldThrowException**: Verifies that assigning a role with an empty user ID throws an exception.

3. **Access Control**:
   - **HasAccess_AdminRole_ShouldHaveAllPermissions**: Verifies that admins have all permissions.
   - **HasAccess_StaffRole_ShouldHaveLimitedPermissions**: Verifies that staff roles have limited permissions.

4. **Role Update**:
   - **UpdateUserRole_StudentToStaff_ShouldSucceed**: Verifies that an admin can update a user's role.
   - **UpdateUserRole_LastAdmin_ShouldThrowException**: Verifies that updating the last admin throws an exception.
   - **UpdateUserRole_NonAdmin_ShouldThrowException**: Verifies that only admins can update roles.
   - **UpdateUserRole_NonExistentRole_ReturnsFalse**: Verifies that updating to a non-existent role fails.

### Code coverage

![image](https://github.com/user-attachments/assets/88bd803c-a7d1-4e14-a707-184e20a5462f)

### Examples

#### A role must be able to be created if it does not already exist.
- **Test**: `CreateRole_ValidRole_ShouldSucceed`
- **Description**: This test verifies that the `CreateRole` method returns `true` when a valid role is created and that the role repository is updated.

#### A role must not be able to be created if it already exists.
- **Test**: `CreateRole_DuplicateRole_ShouldFail`
- **Description**: This test verifies that the `CreateRole` method returns `false` when a duplicate role is attempted to be created.

#### A user must be able to be assigned a valid role.
- **Test**: `AssignRole_ValidRole_ShouldSucceed`
- **Description**: This test verifies that the `AssignRole` method returns `true` when a valid role is assigned to a user and that the user's role is updated.

#### An admin must be able to update a user's role.
- **Test**: `UpdateUserRole_StudentToStaff_ShouldSucceed`
- **Description**: This test verifies that the `UpdateUserRole` method returns `true` when an admin updates a user's role and that the user's role is updated.

## User Registration

The User Registration Service is responsible for handling all operations related to user registration within the system. The goal of this service is to ensure that users can register efficiently and securely, validating their information and ensuring that all registration requirements are met.

The `IUserRegistrationService` interface defines the following methods:

| Method Name                       | Description                                                                 |
|-----------------------------------|-----------------------------------------------------------------------------|
| `RegisterUser(string name, string email, string password)` | Registers a new user with the specified name, email, and password. |
| `IsValidLength(string name, string email, string password)`       | Validates the lengths of the provided name, email, and password.          |
| `ValidateName(string name)`       | Validates whether the provided name meets the application's rules.          |
| `IsValidEmail(string email)`      | Validates whether the provided email is in a valid format.                  |
| `IsEmailAvailable(string email)`  | Checks if the email is available (not already registered).                  |
| `IsValidPassword(string password)`| Validates whether the provided password meets the application's security requirements. |

### Test Cases

**Key Tests**:
- Registration succeeds with valid inputs.
- SQL injection attempts are blocked.
- Case insensitivity is handled during email validation.
- Password validation enforces security rules (e.g., includes uppercase, lowercase, digits, and special characters).

#### 1. **User Registration**
- **`RegisterUser_Should_Succeed_With_Valid_Inputs`**: Ensures successful registration for valid inputs.
- **`RegisterUser_Should_Fail_With_Empty_Inputs`**: Confirms registration fails for missing name, email, or password.
- **`RegisterUser_Should_Fail_With_Invalid_Email_Format`**: Validates rejection of improperly formatted emails.
- **`RegisterUser_Should_Fail_With_Weak_Password`**: Confirms registration fails for passwords not meeting complexity requirements.
- **`RegisterUser_Should_Prevent_SQL_Injection`**: Ensures SQL injection attempts are blocked during registration.
- **`RegisterUser_Should_Handle_Email_Case_Insensitivity`**: Ensures email checks are case-insensitive.

#### 2. **Input Validation**
- **`ValidateName_Should_Fail_If_Name_Is_Too_Short`**: Confirms name validation fails for names that are too short.
- **`ValidateName_Should_Fail_With_Special_Characters`**: Ensures name validation fails for names with invalid characters.
- **`IsValidEmail_Should_Succeed_With_Valid_Email`**: Validates that correctly formatted emails are accepted.
- **`IsEmailAvailable_Should_Fail_If_Email_Exists`**: Verifies rejection of registration attempts for existing emails.
- **`IsEmailAvailable_Should_Return_True_For_Nonexistent_Email`**: Confirms new emails are accepted for registration.
- **`IsValidPassword_Should_Fail_With_Missing_Character_Types`**: Ensures passwords missing uppercase, lowercase, digits, or special characters are rejected.

### Code coverage

![image](https://github.com/user-attachments/assets/ed2d589d-29a9-40a0-9957-9383795994f7)

### Examples

#### A user must be able to register with valid inputs.
- **Test**: `RegisterUser_Should_Succeed_With_Valid_Inputs`
- **Description**: Verifies the `RegisterUser` method returns `true` for valid inputs and successfully adds the user to the repository.

#### A user must not be able to register with SQL injection attempts.
- **Test**: `RegisterUser_Should_Prevent_SQL_Injection`
- **Description**: Ensures the `RegisterUser` method blocks SQL injection attempts and prevents malicious inputs.

#### A user must not be able to register with invalid or weak inputs.
- **Test**: 
  - `RegisterUser_Should_Fail_With_Invalid_Email_Format`
  - `RegisterUser_Should_Fail_With_Weak_Password`
- **Description**: Verifies that invalid emails and weak passwords are correctly rejected.

## Profile Management

The User Profile Service is responsible for handling all operations related to user profiles within the system. The goal of this service is to ensure that user profiles are managed efficiently and securely.

The `IUserProfileService` interface defines the following methods:

| Method Name                                      | Description                                                                 |
|--------------------------------------------------|-----------------------------------------------------------------------------|
| `GetProfile(string userId)`                      | Retrieves the profile of a user by their ID.                                 |
| `UpdateProfile(string userId, User updatedUser)` | Updates the profile information of a user.                                   |
| `UpdatePassword(string userId, string currentPassword, string newPassword)` | Updates the password of a user.                                              |
| `IsAuthorizedToUpdateProfile(string userId, string profileId)` | Checks if a user is authorized to update a specific profile.                  |

### Test Cases

**Key Tests**:
- Profile updates succeed with valid data and fail for duplicate or invalid emails.
- Simultaneous updates are handled gracefully.
- Password updates succeed with correct credentials and fail for weak new passwords.

1. **Profile Retrieval**:
   - **GetProfile_WithValidUserId_ShouldReturnUserProfile**: Verifies that retrieving a profile with a valid user ID returns the correct user profile.
   - **GetProfile_WithUnauthorizedUserId_ShouldReturnNull**: Verifies that retrieving a profile with an unauthorized user ID returns null.

2. **Profile Update**:
   - **UpdateProfile_WithValidData_ShouldUpdateUser**: Verifies that updating a profile with valid data succeeds.
   - **UpdateProfile_WithInvalidEmail_ShouldReturnFalse**: Verifies that updating a profile with an invalid email fails.
   - **UpdateProfile_WithDuplicateEmail_ShouldReturnFalse**: Verifies that updating a profile with a duplicate email fails.
   - **UpdateProfile_SimultaneousUpdates_ShouldHandleGracefully**: Verifies that simultaneous profile updates are handled gracefully.

3. **Password Update**:
   - **UpdatePassword_WithCorrectCurrentPassword_ShouldUpdatePassword**: Verifies that updating the password with the correct current password succeeds.
   - **UpdatePassword_WithIncorrectCurrentPassword_ShouldReturnFalse**: Verifies that updating the password with an incorrect current password fails.
   - **UpdatePassword_WithWeakNewPassword_ShouldReturnFalse**: Verifies that updating the password with a weak new password fails.

4. **Authorization**:
   - **IsAuthorizedToUpdateProfile_ByAdmin_ShouldReturnTrue**: Verifies that an admin is authorized to update a profile.
   - **IsAuthorizedToUpdateProfile_ByNonAdmin_ShouldReturnFalse**: Verifies that a non-admin is not authorized to update a profile.

### Code coverage

![image](https://github.com/user-attachments/assets/f0f35db6-4134-4598-8d5d-9c458e5f303e)

### Examples

#### A user must be able to retrieve their profile with a valid user ID.
- **Test**: `GetProfile_WithValidUserId_ShouldReturnUserProfile`
- **Description**: This test verifies that the `GetProfile` method returns the correct user profile when a valid user ID is provided.

#### A user must be able to update their profile with valid data.
- **Test**: `UpdateProfile_WithValidData_ShouldUpdateUser`
- **Description**: This test verifies that the `UpdateProfile` method returns `true` and updates the user profile when valid data is provided.

#### A user must not be able to update their password with a weak new password.
- **Test**: `UpdatePassword_WithWeakNewPassword_ShouldReturnFalse`
- **Description**: This test verifies that the `UpdatePassword` method returns `false` when a weak new password is provided.

#### An admin must be authorized to update any user's profile.
- **Test**: `IsAuthorizedToUpdateProfile_ByAdmin_ShouldReturnTrue`
- **Description**: This test verifies that the `IsAuthorizedToUpdateProfile` method returns `true` when an admin attempts to update a profile.

## User Authentication

### The Hashing Service

The Hashing Service is responsible for securely hashing and verifying passwords. This ensures that user passwords are stored securely and can be verified during authentication.

The `IHashingService` interface defines the following methods:

| Method Name                       | Description                                                                 |
|-----------------------------------|-----------------------------------------------------------------------------|
| `HashPassword(string password)`   | Hashes the provided password.                                               |
| `VerifyPassword(string password, string hashedPassword)` | Verifies that the provided password matches the hashed password.             |

### The Authentication Service

The Authentication Service is responsible for handling user authentication, checking if accounts are locked, and resetting passwords. This ensures that users can authenticate securely and manage their account access.

The `IAuthService` interface defines the following methods:

| Method Name                       | Description                                                                 |
|-----------------------------------|-----------------------------------------------------------------------------|
| `AuthenticateAsync(string email, string password)` | Authenticates a user with the provided email and password.                  |
| `IsAccountLockedAsync(string email)` | Checks if the account associated with the provided email is locked.          |
| `ResetPasswordAsync(string email, string newPassword)` | Resets the password for the account associated with the provided email.      |


### Test Cases

**Key Tests**:
- Login succeeds for valid credentials and fails for invalid credentials.
- Password reset links are sent for valid emails.
- Account lockouts prevent access after repeated failed attempts.

1. **Login**:
   - **Login_WithValidCredentials_ShouldSucceed**: Verifies that logging in with valid credentials succeeds.
   - **Login_WithInvalidCredentials_ShouldFail**: Verifies that logging in with invalid credentials fails.
   - **Login_WithEmptyCredentials_ShouldThrowException**: Verifies that logging in with empty credentials throws an exception.

2. **Token Generation**:
   - **GenerateToken_WithValidUser_ShouldReturnToken**: Verifies that generating a token for a valid user returns a token.
   - **GenerateToken_WithInvalidUser_ShouldThrowException**: Verifies that generating a token for an invalid user throws an exception.

3. **Password Reset**:
   - **ResetPassword_WithValidEmail_ShouldSendResetLink**: Verifies that resetting the password with a valid email sends a reset link.
   - **ResetPassword_WithInvalidEmail_ShouldFail**: Verifies that resetting the password with an invalid email fails.
   - **ResetPassword_WithEmptyEmail_ShouldThrowException**: Verifies that resetting the password with an empty email throws an exception.

### Code coverage

![image](https://github.com/user-attachments/assets/b7ea815f-c3ba-40e1-9680-1d93631e1d53)

### Examples

#### A user must be able to log in with valid credentials.
- **Test**: `Login_WithValidCredentials_ShouldSucceed`
- **Description**: This test verifies that the `Login` method returns `true` when a user logs in with valid credentials.

#### A user must not be able to log in with invalid credentials.
- **Test**: `Login_WithInvalidCredentials_ShouldFail`
- **Description**: This test verifies that the `Login` method returns `false` when a user logs in with invalid credentials.

#### A valid user must be able to generate a token.
- **Test**: `GenerateToken_WithValidUser_ShouldReturnToken`
- **Description**: This test verifies that the `GenerateToken` method returns a token when a valid user is provided.

#### A user must not be able to reset their password with an invalid email.
- **Test**: `ResetPassword_WithInvalidEmail_ShouldFail`
- **Description**: This test verifies that the `ResetPassword` method returns `false` when an invalid email is provided.

## User Service

The User Service is responsible for handling user registration, authentication, and retrieval of user information. This ensures that users can register, log in, and access their profile information securely and efficiently.

The `IUserService` interface defines the following methods:

| Method Name                                | Description                                                                 |
|--------------------------------------------|-----------------------------------------------------------------------------|
| `RegisterUser(User user, string password)` | Registers a new user with the specified user details and password.          |
| `AuthenticateUser(string email, string password)` | Authenticates a user with the provided email and password.                  |
| `GetUserById(string userId)`               | Retrieves a user by their ID.                                               |

### Test Cases

**Key Tests**:
- Retrieval succeeds for valid user IDs and emails.
- Updates succeed with valid data and fail for duplicate or invalid emails.
- Deletion succeeds for valid user IDs and fails for nonexistent users.

1. **User Registration**:
   - **RegisterUser_NullUser_ThrowsArgumentNullException**: Verifies that registering a null user throws an `ArgumentNullException`.
   - **RegisterUser_InvalidRegistrationDetails_ThrowsArgumentException**: Verifies that registering a user with invalid details throws an `ArgumentException`.
   - **RegisterUser_ValidUser_ReturnsRegisteredUser**: Verifies that registering a valid user returns the registered user.

2. **User Authentication**:
   - **AuthenticateUser_EmptyCredentials_ThrowsArgumentException**: Verifies that authenticating with empty credentials throws an `ArgumentException`.
   - **AuthenticateUser_InvalidEmailOrPassword_ThrowsUnauthorizedAccessException**: Verifies that authenticating with nonvalid email or password throws an `UnauthorizedAccessException`.
   - **AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser**: Verifies that authenticating with valid credentials returns the authenticated user.

3. **User Retrieval**:
   - **GetUserById_UserFound_ReturnsUser**: Verifies that retrieving a user by ID returns the user if found.
   - **GetUserById_InvalidUserId_ThrowsKeyNotFoundException**: Verifies that retrieving a user by an invalid ID throws a `KeyNotFoundException`.

### Code coverage

![image](https://github.com/user-attachments/assets/25bb371b-137f-44d2-8e9a-7384ca7f0ce4)

### Examples of Test Cases and Mapping to Requirements

#### A user must not be able to register with null details.
- **Test**: `RegisterUser_NullUser_ThrowsArgumentNullException`
- **Description**: This test verifies that the `RegisterUser` method throws an `ArgumentNullException` when a null user is provided.

#### A user must not be able to authenticate with empty credentials.
- **Test**: `AuthenticateUser_EmptyCredentials_ThrowsArgumentException`
- **Description**: This test verifies that the `AuthenticateUser` method throws an `ArgumentException` when empty credentials are provided.

#### A user must be able to authenticate with valid credentials.
- **Test**: `AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser`
- **Description**: This test verifies that the `AuthenticateUser` method returns the authenticated user when valid credentials are provided.

#### A user must be able to retrieve their details by ID if found.
- **Test**: `GetUserById_UserFound_ReturnsUser`
- **Description**: This test verifies that the `GetUserById` method returns the user when a valid user ID is provided.
