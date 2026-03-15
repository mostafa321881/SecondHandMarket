using System;
using System.Collections.Generic;
using SecondHandMarket.Models;
using SecondHandMarket.Enums;

namespace SecondHandMarket.Services;

/// <summary>
/// Provides functionality for creating and managing marketplace listings.
/// </summary>
public class ListingService
{
    /// <summary>
    /// Gets all listings in the marketplace.
    /// </summary>
    public List<Listing> Listings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListingService"/> class.
    /// </summary>
    public ListingService()
    {
        Listings = new List<Listing>();
    }

    /// <summary>
    /// Creates a new listing for a seller.
    /// </summary>
    public Listing CreateListing(
        User seller,
        string title,
        string description,
        Category category,
        Condition condition,
        decimal price)
    {
        if (seller == null)
            throw new ArgumentException("Seller cannot be null.");

        var listing = new Listing(title, description, category, condition, price, seller);

        Listings.Add(listing);
        seller.Listings.Add(listing);

        return listing;
    }
}