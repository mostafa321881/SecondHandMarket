using System;

namespace SecondHandMarket.Models;

/// <summary>
/// Represents a completed purchase transaction between a buyer and a seller.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Gets the listing that was purchased.
    /// </summary>
    public Listing Listing { get; }

    /// <summary>
    /// Gets the buyer in the transaction.
    /// </summary>
    public User Buyer { get; }

    /// <summary>
    /// Gets the seller in the transaction.
    /// </summary>
    public User Seller { get; }

    /// <summary>
    /// Gets the final transaction price.
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// Gets the date and time when the transaction happened.
    /// </summary>
    public DateTime PurchasedAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Transaction"/> class.
    /// </summary>
    /// <param name="listing">The purchased listing.</param>
    /// <param name="buyer">The buyer.</param>
    /// <param name="seller">The seller.</param>
    public Transaction(Listing listing, User buyer, User seller)
    {
        Listing = listing ?? throw new ArgumentNullException(nameof(listing));
        Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
        Seller = seller ?? throw new ArgumentNullException(nameof(seller));
        Price = listing.Price;
        PurchasedAt = DateTime.Now;
    }
}