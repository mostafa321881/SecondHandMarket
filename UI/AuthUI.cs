using System;
using SecondHandMarket.Services;

namespace SecondHandMarket.UI;

/// <summary>
/// Handles console interaction for user authentication.
/// </summary>
public class AuthUI
{
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthUI"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    public AuthUI(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Prompts the user to register a new account.
    /// </summary>
    public void Register()
    {
        try
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? string.Empty;

            _userService.Register(username, password);

            Console.WriteLine("Registration successful.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Prompts the user to log in.
    /// </summary>
    public void Login()
    {
        try
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? string.Empty;

            var user = _userService.Login(username, password);

            Console.WriteLine($"Welcome back, {user.Username}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    public void Logout()
    {
        try
        {
            _userService.Logout();
            Console.WriteLine("You have been logged out.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}