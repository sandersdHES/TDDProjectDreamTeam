## Role Management - Unit Test Documentation

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

## User Registration - Unit Test Documentation

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

## Profile Management - Unit Test Documentation

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

## User Authentication - Unit Test Documentation

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

## User Service - Unit Test Documentation

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
