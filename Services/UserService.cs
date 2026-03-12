using System;
using System.Collections.Generic;
using System.Linq;
using SecondHandMarket.Models;

namespace SecondHandMarket.Services;

/// <summary>
/// Provides user registration, login, and logout functionality for the marketplace.
/// </summary>
public class UserService
{
    /// <summary>
    /// Gets the list of registered users.
    /// </summary>
    public List<User> Users { get; }

    /// <summary>
    /// Gets the currently logged-in user, or null if no user is logged in.
    /// </summary>
    public User? CurrentUser { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    public UserService()
    {
        Users = new List<User>();
        CurrentUser = null;
    }

    /// <summary>
    /// Registers a new user with a unique username and password.
    /// </summary>
    /// <param name="username">The username to register.</param>
    /// <param name="password">The password to register.</param>
    /// <returns>The created user.</returns>
    /// <exception cref="ArgumentException">Thrown when input is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the username already exists.</exception>
    public User Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }

        string normalizedUsername = username.Trim();

        bool usernameExists = Users.Any(user =>
            user.Username.Equals(normalizedUsername, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new User(normalizedUsername, password.Trim());
        Users.Add(user);

        return user;
    }

    /// <summary>
    /// Logs in a user with matching username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The logged-in user.</returns>
    /// <exception cref="ArgumentException">Thrown when input is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when login fails.</exception>
    public User Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }

        if (CurrentUser is not null)
        {
            throw new InvalidOperationException("A user is already logged in.");
        }

        var user = Users.FirstOrDefault(existingUser =>
            existingUser.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
            existingUser.Password == password.Trim());

        if (user is null)
        {
            throw new InvalidOperationException("Invalid username or password.");
        }

        CurrentUser = user;
        return user;
    }

    /// <summary>
    /// Logs out the currently logged-in user.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no user is logged in.</exception>
    public void Logout()
    {
        if (CurrentUser is null)
        {
            throw new InvalidOperationException("No user is currently logged in.");
        }

        CurrentUser = null;
    }

    /// <summary>
    /// Finds a registered user by username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The matching user, or null if not found.</returns>
    public User? FindByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        return Users.FirstOrDefault(user =>
            user.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}