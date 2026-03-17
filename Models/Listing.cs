using System;
using SecondHandMarket.Enums;

namespace SecondHandMarket.Models;

/// <summary>
/// Represents an item listing in the marketplace.
/// </summary>
public class Listing
{
    /// <summary>
    /// Gets the title of the listing.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the description of the listing.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the category of the listing.
    /// </summary>
    public Category Category { get; private set; }

    /// <summary>
    /// Gets the condition of the listing.
    /// </summary>
    public Condition Condition { get; private set; }

    /// <summary>
    /// Gets the price of the listing.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the current status of the listing.
    /// </summary>
    public ListingStatus Status { get; private set; }

    /// <summary>
    /// Gets the seller who created the listing.
    /// </summary>
    public User Seller { get; }

    /// <summary>
    /// Gets the buyer of the listing after purchase.
    /// Null while the listing is still available.
    /// </summary>
    public User? Buyer { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Listing"/> class.
    /// </summary>
    /// <param name="title">The title of the listing.</param>
    /// <param name="description">The description of the listing.</param>
    /// <param name="category">The category of the listing.</param>
    /// <param name="condition">The condition of the listing.</param>
    /// <param name="price">The price of the listing.</param>
    /// <param name="seller">The seller who created the listing.</param>
    public Listing(
        string title,
        string description,
        Category category,
        Condition condition,
        decimal price,
        User seller)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be positive.");
        }

        Seller = seller ?? throw new ArgumentNullException(nameof(seller));

        Title = title.Trim();
        Description = description.Trim();
        Category = category;
        Condition = condition;
        Price = price;
        Status = ListingStatus.Available;
        Buyer = null;
    }

    /// <summary>
    /// Marks the listing as sold to the specified buyer.
    /// </summary>
    /// <param name="buyer">The buyer purchasing the listing.</param>
    public void MarkAsSold(User buyer)
    {
        if (buyer is null)
        {
            throw new ArgumentNullException(nameof(buyer));
        }

        if (buyer == Seller)
        {
            throw new InvalidOperationException("Cannot buy own listing.");
        }

        if (Status == ListingStatus.Sold)
        {
            throw new InvalidOperationException("Listing is already sold.");
        }

        Status = ListingStatus.Sold;
        Buyer = buyer;
    }
}