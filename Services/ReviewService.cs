using System;
using System.Collections.Generic;
using System.Linq;
using SecondHandMarket.Models;

namespace SecondHandMarket.Services;

/// <summary>
/// Handles review logic for completed transactions.
/// </summary>
public class ReviewService
{
    /// <summary>
    /// Gets all reviews in the system.
    /// </summary>
    public List<Review> Reviews { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewService"/> class.
    /// </summary>
    public ReviewService()
    {
        Reviews = new List<Review>();
    }

    /// <summary>
    /// Leaves a review for a completed transaction.
    /// </summary>
    /// <param name="transaction">The transaction to review.</param>
    /// <param name="buyer">The buyer leaving the review.</param>
    /// <param name="rating">Rating from 1 to 6.</param>
    /// <param name="comment">Optional comment.</param>
    /// <returns>The created review.</returns>
    public Review LeaveReview(
        Transaction transaction,
        User buyer,
        int rating,
        string comment)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (buyer is null)
        {
            throw new ArgumentNullException(nameof(buyer));
        }

        if (transaction.Buyer != buyer)
        {
            throw new InvalidOperationException("Only the buyer can review this transaction.");
        }

        if (transaction.HasReview)
        {
            throw new InvalidOperationException("This transaction has already been reviewed.");
        }

        if (rating < 1 || rating > 6)
        {
            throw new ArgumentException("Rating must be between 1 and 6.");
        }

        Review review = new Review(
            transaction,
            buyer,
            transaction.Seller,
            rating,
            comment);

        Reviews.Add(review);
        transaction.Seller.ReviewsReceived.Add(review);
        buyer.ReviewsWritten.Add(review);
        transaction.HasReview = true;

        return review;
    }

    /// <summary>
    /// Gets all reviews received by a seller.
    /// </summary>
    /// <param name="seller">The seller.</param>
    /// <returns>A list of reviews received by the seller.</returns>
    public List<Review> GetReviewsForSeller(User seller)
    {
        if (seller is null)
        {
            throw new ArgumentNullException(nameof(seller));
        }

        return Reviews
            .Where(r => r.ReviewedUser == seller)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
    }
}