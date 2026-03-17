using System;
using SecondHandMarket.Enums;
using SecondHandMarket.Models;

namespace SecondHandMarket.Services;

/// <summary>
/// Handles purchasing logic for marketplace listings.
/// </summary>
public class PurchaseService
{
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

        buyer.Purchases.Add(transaction);
        listing.Seller.Sales.Add(transaction);

        return transaction;
    }
}