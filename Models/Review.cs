using System;

namespace SecondHandMarket.Models;

public class Review
{
    public User Reviewer { get; }
    public User Seller { get; }
    public int Rating { get; }
    public string Comment { get; }

    public Review(User reviewer, User seller, int rating, string? comment)
    {
        if (rating < 1 || rating > 6)
            throw new ArgumentException("Rating must be 1-6");

        Reviewer = reviewer;
        Seller = seller;
        Rating = rating;
        Comment = comment ?? "";
    }
}