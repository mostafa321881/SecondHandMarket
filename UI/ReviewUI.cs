using System;
using System.Collections.Generic;
using SecondHandMarket.Models;
using SecondHandMarket.Services;

namespace SecondHandMarket.UI;

/// <summary>
/// Handles console interaction for review features.
/// </summary>
public class ReviewUI
{
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;
    private readonly PurchaseService _purchaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewUI"/> class.
    /// </summary>
    /// <param name="reviewService">The review service.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="purchaseService">The purchase service.</param>
    public ReviewUI(
        ReviewService reviewService,
        UserService userService,
        PurchaseService purchaseService)
    {
        _reviewService = reviewService;
        _userService = userService;
        _purchaseService = purchaseService;
    }

    /// <summary>
    /// Displays the review menu for the current user.
    /// </summary>
    public void ShowReviewMenu()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to manage reviews.");
            return;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Reviews ===");
            Console.WriteLine("1. Leave a review");
            Console.WriteLine("2. View received reviews");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    LeaveReview();
                    break;

                case "2":
                    ShowReceivedReviews();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Lets the current user leave a review for one of their completed purchases.
    /// </summary>
    public void LeaveReview()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to leave a review.");
            return;
        }

        List<Transaction> purchasedTransactions =
            _purchaseService.GetBoughtTransactions(_userService.CurrentUser);

        List<Transaction> reviewableTransactions = purchasedTransactions.FindAll(
            transaction => !transaction.HasReview);

        Console.WriteLine();
        Console.WriteLine("=== Leave Review ===");

        if (reviewableTransactions.Count == 0)
        {
            Console.WriteLine("You have no purchases available for review.");
            return;
        }

        for (int i = 0; i < reviewableTransactions.Count; i++)
        {
            Transaction transaction = reviewableTransactions[i];
            Console.WriteLine(
                $"{i + 1}. {transaction.Listing.Title} | Seller: {transaction.Seller.Username} | Date: {transaction.PurchasedAt:dd.MM.yyyy}");
        }

        Console.Write("Select a transaction to review: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int selectedIndex) ||
            selectedIndex < 1 ||
            selectedIndex > reviewableTransactions.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        Transaction selectedTransaction = reviewableTransactions[selectedIndex - 1];

        Console.Write("Enter rating (1-6): ");
        string? ratingInput = Console.ReadLine();

        if (!int.TryParse(ratingInput, out int rating))
        {
            Console.WriteLine("Invalid rating.");
            return;
        }

        Console.Write("Enter comment (optional): ");
        string comment = Console.ReadLine() ?? string.Empty;

        try
        {
            _reviewService.LeaveReview(
                selectedTransaction,
                _userService.CurrentUser,
                rating,
                comment);

            Console.WriteLine("Review submitted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Displays reviews received by the current user as a seller.
    /// </summary>
    public void ShowReceivedReviews()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to view reviews.");
            return;
        }

        List<Review> reviews = _reviewService.GetReviewsForSeller(_userService.CurrentUser);

        Console.WriteLine();
        Console.WriteLine("=== Received Reviews ===");

        if (reviews.Count == 0)
        {
            Console.WriteLine("You have not received any reviews yet.");
            return;
        }

        Console.WriteLine($"Average Rating: {_userService.CurrentUser.AverageRating:F1}");
        Console.WriteLine();

        foreach (Review review in reviews)
        {
            Console.WriteLine($"Item: {review.Transaction.Listing.Title}");
            Console.WriteLine($"Buyer: {review.Reviewer.Username}");
            Console.WriteLine($"Rating: {review.Rating}/6");
            Console.WriteLine($"Comment: {review.Comment}");
            Console.WriteLine($"Date: {review.CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine(new string('-', 30));
        }
    }
}