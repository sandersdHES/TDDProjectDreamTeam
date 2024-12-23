# Unit Test Documentation

Welcome to our TDD project. This project will focus on tests related to the module User Management. This module is split into 4 main tasks:

1. Role Management
2. User Registration
3. Profile Management
4. User Authentication

This will be useful to create a fully functionnal user service, containing the capabilities of managing the afro-mentionned tasks.

## Summary

- [Setup instructions](#setup-instructions)
- [Repository and models](#repository-and-models)
- [Role Management](#role-management)
  - [Rationale of Test Cases and Coverage](#a-rationale-of-test-cases-and-coverage-1)
  - [Examples of Test Cases and Mapping to Requirements](#b-examples-of-test-cases-and-mapping-to-requirements-1)
- [User Registration](#user-registration)
  - [Rationale of Test Cases and Coverage](#a-rationale-of-test-cases-and-coverage-2)
  - [Examples of Test Cases and Mapping to Requirements](#b-examples-of-test-cases-and-mapping-to-requirements-2)
- [Profile Management](#profile-management)
  - [Rationale of Test Cases and Coverage](#a-rationale-of-test-cases-and-coverage-3)
  - [Examples of Test Cases and Mapping to Requirements](#b-examples-of-test-cases-and-mapping-to-requirements-3)
- [User Authentication](#user-authentication)
  - [Rationale of Test Cases and Coverage](#a-rationale-of-test-cases-and-coverage-4)
  - [Examples of Test Cases and Mapping to Requirements](#b-examples-of-test-cases-and-mapping-to-requirements-4)
- [User Service](#user-service)
  - [Rationale of Test Cases and Coverage](#a-rationale-of-test-cases-and-coverage-5)
  - [Examples of Test Cases and Mapping to Requirements](#b-examples-of-test-cases-and-mapping-to-requirements-5)

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

### Additional Tools

- **NuGet Packages**: Ensure all required NuGet packages are installed. You can restore them by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.
- **Code Analysis**: Install and follow the process for [Fine Code Coverage](https://github.com/FortuneN/FineCodeCoverage).

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

### a. Rationale of Test Cases and Coverage

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

### b. Examples of Test Cases and Mapping to Requirements

#### Requirement: A role must be able to be created if it does not already exist.
- **Test**: `CreateRole_ValidRole_ShouldSucceed`
- **Description**: This test verifies that the `CreateRole` method returns `true` when a valid role is created and that the role repository is updated.

#### Requirement: A role must not be able to be created if it already exists.
- **Test**: `CreateRole_DuplicateRole_ShouldFail`
- **Description**: This test verifies that the `CreateRole` method returns `false` when a duplicate role is attempted to be created.

#### Requirement: A user must be able to be assigned a valid role.
- **Test**: `AssignRole_ValidRole_ShouldSucceed`
- **Description**: This test verifies that the `AssignRole` method returns `true` when a valid role is assigned to a user and that the user's role is updated.

#### Requirement: An admin must be able to update a user's role.
- **Test**: `UpdateUserRole_StudentToStaff_ShouldSucceed`
- **Description**: This test verifies that the `UpdateUserRole` method returns `true` when an admin updates a user's role and that the user's role is updated.

## User Registration

### a. Rationale of Test Cases and Coverage

1. **User Registration**:
   - **RegisterUser_Should_Succeed_With_Valid_Inputs**: Verifies that registering a user with valid inputs succeeds.
   - **RegisterUser_Should_Fail_With_Empty_Inputs**: Verifies that registering a user with empty inputs fails.
   - **RegisterUser_Should_Fail_With_Invalid_Email_Format**: Verifies that registering a user with an invalid email format fails.
   - **RegisterUser_Should_Fail_With_Weak_Password**: Verifies that registering a user with a weak password fails.
   - **RegisterUser_Should_Prevent_SQL_Injection**: Verifies that registering a user with SQL injection attempts fails.
   - **RegisterUser_Should_Handle_Email_Case_Insensitivity**: Verifies that email case insensitivity is handled correctly during registration.

2. **Name Validation**:
   - **ValidateName_Should_Fail_If_Name_Is_Too_Short**: Verifies that name validation fails if the name is too short.
   - **ValidateName_Should_Succeed_With_Valid_Name**: Verifies that name validation succeeds with a valid name.

3. **Email Validation**:
   - **IsValidEmail_Should_Succeed_With_Valid_Email**: Verifies that email validation succeeds with a valid email.
   - **IsEmailAvailable_Should_Fail_If_Email_Exists**: Verifies that email availability check fails if the email already exists.

4. **Password Validation**:
   - **IsValidPassword_Should_Succeed_With_Strong_Password**: Verifies that password validation succeeds with a strong password.

### b. Examples of Test Cases and Mapping to Requirements

#### Requirement: A user must be able to register with valid inputs.
- **Test**: `RegisterUser_Should_Succeed_With_Valid_Inputs`
- **Description**: This test verifies that the `RegisterUser` method returns `true` when a user is registered with valid inputs and that the user repository is updated.

#### Requirement: A user must not be able to register with empty inputs.
- **Test**: `RegisterUser_Should_Fail_With_Empty_Inputs`
- **Description**: This test verifies that the `RegisterUser` method returns `false` when a user is registered with empty inputs.

#### Requirement: A user must not be able to register with an invalid email format.
- **Test**: `RegisterUser_Should_Fail_With_Invalid_Email_Format`
- **Description**: This test verifies that the `RegisterUser` method returns `false` when a user is registered with an invalid email format.

#### Requirement: A user must not be able to register with a weak password.
- **Test**: `RegisterUser_Should_Fail_With_Weak_Password`
- **Description**: This test verifies that the `RegisterUser` method returns `false` when a user is registered with a weak password.

#### Requirement: A user must not be able to register with SQL injection attempts.
- **Test**: `RegisterUser_Should_Prevent_SQL_Injection`
- **Description**: This test verifies that the `RegisterUser` method returns `false` when a user is registered with SQL injection attempts.

#### Requirement: Email case insensitivity must be handled correctly during registration.
- **Test**: `RegisterUser_Should_Handle_Email_Case_Insensitivity`
- **Description**: This test verifies that the `RegisterUser` method handles email case insensitivity correctly during registration.

## Profile Management

### a. Rationale of Test Cases and Coverage

The unit tests for profile management are designed to verify the critical functionalities of the profile management service. Here are the main categories of tests and their rationale:

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

### b. Examples of Test Cases and Mapping to Requirements

#### Requirement: A user must be able to retrieve their profile with a valid user ID.
- **Test**: `GetProfile_WithValidUserId_ShouldReturnUserProfile`
- **Description**: This test verifies that the `GetProfile` method returns the correct user profile when a valid user ID is provided.

#### Requirement: A user must not be able to retrieve a profile with an unauthorized user ID.
- **Test**: `GetProfile_WithUnauthorizedUserId_ShouldReturnNull`
- **Description**: This test verifies that the `GetProfile` method returns null when an unauthorized user ID is provided.

#### Requirement: A user must be able to update their profile with valid data.
- **Test**: `UpdateProfile_WithValidData_ShouldUpdateUser`
- **Description**: This test verifies that the `UpdateProfile` method returns `true` and updates the user profile when valid data is provided.

#### Requirement: A user must not be able to update their profile with an invalid email.
- **Test**: `UpdateProfile_WithInvalidEmail_ShouldReturnFalse`
- **Description**: This test verifies that the `UpdateProfile` method returns `false` when an invalid email is provided.

#### Requirement: A user must not be able to update their profile with a duplicate email.
- **Test**: `UpdateProfile_WithDuplicateEmail_ShouldReturnFalse`
- **Description**: This test verifies that the `UpdateProfile` method returns `false` when a duplicate email is provided.

#### Requirement: A user must be able to update their password with the correct current password.
- **Test**: `UpdatePassword_WithCorrectCurrentPassword_ShouldUpdatePassword`
- **Description**: This test verifies that the `UpdatePassword` method returns `true` and updates the password when the correct current password is provided.

#### Requirement: A user must not be able to update their password with an incorrect current password.
- **Test**: `UpdatePassword_WithIncorrectCurrentPassword_ShouldReturnFalse`
- **Description**: This test verifies that the `UpdatePassword` method returns `false` when an incorrect current password is provided.

#### Requirement: A user must not be able to update their password with a weak new password.
- **Test**: `UpdatePassword_WithWeakNewPassword_ShouldReturnFalse`
- **Description**: This test verifies that the `UpdatePassword` method returns `false` when a weak new password is provided.

#### Requirement: An admin must be authorized to update any user's profile.
- **Test**: `IsAuthorizedToUpdateProfile_ByAdmin_ShouldReturnTrue`
- **Description**: This test verifies that the `IsAuthorizedToUpdateProfile` method returns `true` when an admin attempts to update a profile.

#### Requirement: A non-admin must not be authorized to update another user's profile.
- **Test**: `IsAuthorizedToUpdateProfile_ByNonAdmin_ShouldReturnFalse`
- **Description**: This test verifies that the `IsAuthorizedToUpdateProfile` method returns `false` when a non-admin attempts to update another user's profile.

## User Authentication

### a. Rationale of Test Cases and Coverage

The unit tests for user authentication are designed to verify the critical functionalities of the user authentication service. Here are the main categories of tests and their rationale:

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

### b. Examples of Test Cases and Mapping to Requirements

#### Requirement: A user must be able to log in with valid credentials.
- **Test**: `Login_WithValidCredentials_ShouldSucceed`
- **Description**: This test verifies that the `Login` method returns `true` when a user logs in with valid credentials.

#### Requirement: A user must not be able to log in with invalid credentials.
- **Test**: `Login_WithInvalidCredentials_ShouldFail`
- **Description**: This test verifies that the `Login` method returns `false` when a user logs in with invalid credentials.

#### Requirement: A user must not be able to log in with empty credentials.
- **Test**: `Login_WithEmptyCredentials_ShouldThrowException`
- **Description**: This test verifies that the `Login` method throws an exception when a user logs in with empty credentials.

#### Requirement: A valid user must be able to generate a token.
- **Test**: `GenerateToken_WithValidUser_ShouldReturnToken`
- **Description**: This test verifies that the `GenerateToken` method returns a token when a valid user is provided.

#### Requirement: An invalid user must not be able to generate a token.
- **Test**: `GenerateToken_WithInvalidUser_ShouldThrowException`
- **Description**: This test verifies that the `GenerateToken` method throws an exception when an invalid user is provided.

#### Requirement: A user must be able to reset their password with a valid email.
- **Test**: `ResetPassword_WithValidEmail_ShouldSendResetLink`
- **Description**: This test verifies that the `ResetPassword` method sends a reset link when a valid email is provided.

#### Requirement: A user must not be able to reset their password with an invalid email.
- **Test**: `ResetPassword_WithInvalidEmail_ShouldFail`
- **Description**: This test verifies that the `ResetPassword` method returns `false` when an invalid email is provided.

#### Requirement: A user must not be able to reset their password with an empty email.
- **Test**: `ResetPassword_WithEmptyEmail_ShouldThrowException`
- **Description**: This test verifies that the `ResetPassword` method throws an exception when an empty email is provided.

## User Service

### a. Rationale of Test Cases and Coverage

The unit tests for user service are designed to verify the critical functionalities of the user service. Here are the main categories of tests and their rationale:

1. **User Registration**:
   - **RegisterUser_NullUser_ThrowsArgumentNullException**: Verifies that registering a null user throws an `ArgumentNullException`.
   - **RegisterUser_InvalidRegistrationDetails_ThrowsArgumentException**: Verifies that registering a user with invalid details throws an `ArgumentException`.
   - **RegisterUser_ValidUser_ReturnsRegisteredUser**: Verifies that registering a valid user returns the registered user.

2. **User Authentication**:
   - **AuthenticateUser_EmptyCredentials_ThrowsArgumentException**: Verifies that authenticating with empty credentials throws an `ArgumentException`.
   - **AuthenticateUser_InvalidEmailOrPassword_ThrowsUnauthorizedAccessException**: Verifies that authenticating with invalid email or password throws an `UnauthorizedAccessException`.
   - **AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser**: Verifies that authenticating with valid credentials returns the authenticated user.

3. **User Retrieval**:
   - **GetUserById_UserFound_ReturnsUser**: Verifies that retrieving a user by ID returns the user if found.
   - **GetUserById_InvalidUserId_ThrowsKeyNotFoundException**: Verifies that retrieving a user by an invalid ID throws a `KeyNotFoundException`.

### b. Examples of Test Cases and Mapping to Requirements

#### Requirement: A user must not be able to register with null details.
- **Test**: `RegisterUser_NullUser_ThrowsArgumentNullException`
- **Description**: This test verifies that the `RegisterUser` method throws an `ArgumentNullException` when a null user is provided.

#### Requirement: A user must not be able to register with invalid details.
- **Test**: `RegisterUser_InvalidRegistrationDetails_ThrowsArgumentException`
- **Description**: This test verifies that the `RegisterUser` method throws an `ArgumentException` when invalid registration details are provided.

#### Requirement: A user must be able to register with valid details.
- **Test**: `RegisterUser_ValidUser_ReturnsRegisteredUser`
- **Description**: This test verifies that the `RegisterUser` method returns the registered user when valid details are provided.

#### Requirement: A user must not be able to authenticate with empty credentials.
- **Test**: `AuthenticateUser_EmptyCredentials_ThrowsArgumentException`
- **Description**: This test verifies that the `AuthenticateUser` method throws an `ArgumentException` when empty credentials are provided.

#### Requirement: A user must not be able to authenticate with invalid email or password.
- **Test**: `AuthenticateUser_InvalidEmailOrPassword_ThrowsUnauthorizedAccessException`
- **Description**: This test verifies that the `AuthenticateUser` method throws an `UnauthorizedAccessException` when invalid email or password is provided.

#### Requirement: A user must be able to authenticate with valid credentials.
- **Test**: `AuthenticateUser_ValidCredentials_ReturnsAuthenticatedUser`
- **Description**: This test verifies that the `AuthenticateUser` method returns the authenticated user when valid credentials are provided.

#### Requirement: A user must be able to retrieve their details by ID if found.
- **Test**: `GetUserById_UserFound_ReturnsUser`
- **Description**: This test verifies that the `GetUserById` method returns the user when a valid user ID is provided.

#### Requirement: A user must not be able to retrieve details with an invalid ID.
- **Test**: `GetUserById_InvalidUserId_ThrowsKeyNotFoundException`
- **Description**: This test verifies that the `GetUserById` method throws a `KeyNotFoundException` when an invalid user ID is provided.
