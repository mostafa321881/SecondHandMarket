using System;
using SecondHandMarket.Enums;

namespace SecondHandMarket.Models;

public class Listing
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public Condition Condition { get; private set; }
    public decimal Price { get; private set; }
    public ListingStatus Status { get; private set; }

    public User Seller { get; }
    public User? Buyer { get; private set; }

    public Listing(string title, string description, Category category,
        Condition condition, decimal price, User seller)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title required");

        if (price <= 0)
            throw new ArgumentException("Price must be positive");

        Title = title;
        Description = description;
        Category = category;
        Condition = condition;
        Price = price;
        Seller = seller;
        Status = ListingStatus.Available;
    }

    public void MarkAsSold(User buyer)
    {
        if (buyer == Seller)
            throw new InvalidOperationException("Cannot buy own listing");

        Status = ListingStatus.Sold;
        Buyer = buyer;
    }
}