using System;
using System.Collections.Generic;
using System.Linq;

namespace SecondHandMarket.Models;

/// <summary>
/// Represents a registered user in the marketplace.
/// A user can both buy and sell items.
/// </summary>
public class User
{
    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Gets the listings created by the user.
    /// </summary>
    public List<Listing> Listings { get; }

    /// <summary>
    /// Gets the transactions where the user was the buyer.
    /// </summary>
    public List<Transaction> Purchases { get; }

    /// <summary>
    /// Gets the transactions where the user was the seller.
    /// </summary>
    public List<Transaction> Sales { get; }

    /// <summary>
    /// Gets the reviews received by the user as a seller.
    /// </summary>
    public List<Review> ReviewsReceived { get; }

    /// <summary>
    /// Gets the average seller rating for the user.
    /// Returns 0 if the user has no reviews.
    /// </summary>
    public double AverageRating => ReviewsReceived.Any()
        ? ReviewsReceived.Average(review => review.Rating)
        : 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <exception cref="ArgumentException">Thrown when username or password is invalid.</exception>
    public User(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }

        Username = username.Trim();
        Password = password.Trim();
        Listings = new List<Listing>();
        Purchases = new List<Transaction>();
        Sales = new List<Transaction>();
        ReviewsReceived = new List<Review>();
    }
}