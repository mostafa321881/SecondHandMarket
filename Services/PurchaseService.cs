using System;
using System.Collections.Generic;
using System.Linq;
using SecondHandMarket.Enums;
using SecondHandMarket.Models;

namespace SecondHandMarket.Services;

/// <summary>
/// Handles purchasing logic for marketplace listings.
/// </summary>
public class PurchaseService
{
    /// <summary>
    /// Gets all completed transactions in the marketplace.
    /// </summary>
    public List<Transaction> Transactions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseService"/> class.
    /// </summary>
    public PurchaseService()
    {
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Purchases an available listing for the given buyer.
    /// </summary>
    /// <param name="buyer">The user buying the listing.</param>
    /// <param name="listing">The listing to purchase.</param>
    /// <returns>The created transaction.</returns>
    public Transaction PurchaseListing(User buyer, Listing listing)
    {
        if (buyer is null)
        {
            throw new ArgumentNullException(nameof(buyer));
        }

        if (listing is null)
        {
            throw new ArgumentNullException(nameof(listing));
        }

        if (listing.Status != ListingStatus.Available)
        {
            throw new InvalidOperationException("This listing is no longer available.");
        }

        if (listing.Seller == buyer)
        {
            throw new InvalidOperationException("You cannot purchase your own listing.");
        }

        listing.MarkAsSold(buyer);

        var transaction = new Transaction(listing, buyer, listing.Seller);

        Transactions.Add(transaction);
        buyer.Purchases.Add(transaction);
        listing.Seller.Sales.Add(transaction);

        return transaction;
    }

    /// <summary>
    /// Gets all transactions where the user is the buyer.
    /// </summary>
    /// <param name="user">The buyer.</param>
    /// <returns>A list of bought transactions.</returns>
    public List<Transaction> GetBoughtTransactions(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return Transactions
            .Where(t => t.Buyer == user)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();
    }

    /// <summary>
    /// Gets all transactions where the user is the seller.
    /// </summary>
    /// <param name="user">The seller.</param>
    /// <returns>A list of sold transactions.</returns>
    public List<Transaction> GetSoldTransactions(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return Transactions
            .Where(t => t.Seller == user)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();
    }
}