using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.UserRegistration
{
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Registers a new user with the specified name, email, and password.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        bool RegisterUser(string name, string email, string password);

        /// <summary>
        /// Validates whether the provided name meets the application's rules.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <returns>True if the name is valid; otherwise, false.</returns>
        bool ValidateName(string name);

        /// <summary>
        /// Validates whether the provided email is in a valid format.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>True if the email is valid; otherwise, false.</returns>
        bool IsValidEmail(string email);

        /// <summary>
        /// Checks if the email is available (not already registered).
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns>True if the email is available; otherwise, false.</returns>
        bool IsEmailAvailable(string email);

        /// <summary>
        /// Validates whether the provided password meets the application's security requirements.
        /// </summary>
        /// <param name="password">The user's password.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        bool IsValidPassword(string password);
    }

}
