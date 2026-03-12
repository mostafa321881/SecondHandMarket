using System;

namespace SecondHandMarket.Models;

public class Transaction
{
    public Listing Listing { get; }
    public User Buyer { get; }
    public User Seller { get; }
    public decimal Price { get; }
    public DateTime Date { get; }

    public Review? Review { get; private set; }

    public Transaction(Listing listing, User buyer, User seller)
    {
        Listing = listing;
        Buyer = buyer;
        Seller = seller;
        Price = listing.Price;
        Date = DateTime.Now;
    }

    public void AddReview(Review review)
    {
        if (Review != null)
            throw new InvalidOperationException("Review already exists");

        Review = review;
    }
}