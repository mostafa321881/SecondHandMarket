using System;

namespace SecondHandMarket.Models;

/// <summary>
/// Represents a review left by a buyer for a completed transaction.
/// </summary>
public class Review
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Review"/> class.
    /// </summary>
    /// <param name="transaction">The related transaction.</param>
    /// <param name="reviewer">The buyer leaving the review.</param>
    /// <param name="reviewedUser">The seller being reviewed.</param>
    /// <param name="rating">The rating from 1 to 6.</param>
    /// <param name="comment">The optional review comment.</param>
    public Review(
        Transaction transaction,
        User reviewer,
        User reviewedUser,
        int rating,
        string comment)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (reviewer is null)
        {
            throw new ArgumentNullException(nameof(reviewer));
        }

        if (reviewedUser is null)
        {
            throw new ArgumentNullException(nameof(reviewedUser));
        }

        if (rating < 1 || rating > 6)
        {
            throw new ArgumentException("Rating must be between 1 and 6.");
        }

        Transaction = transaction;
        Reviewer = reviewer;
        ReviewedUser = reviewedUser;
        Rating = rating;
        Comment = comment ?? string.Empty;
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Gets the transaction connected to the review.
    /// </summary>
    public Transaction Transaction { get; }

    /// <summary>
    /// Gets the buyer who wrote the review.
    /// </summary>
    public User Reviewer { get; }

    /// <summary>
    /// Gets the seller being reviewed.
    /// </summary>
    public User ReviewedUser { get; }

    /// <summary>
    /// Gets the numeric rating from 1 to 6.
    /// </summary>
    public int Rating { get; }

    /// <summary>
    /// Gets the review comment.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Gets the date and time when the review was created.
    /// </summary>
    public DateTime CreatedAt { get; }
}