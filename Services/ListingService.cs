using System;
using System.Collections.Generic;
using System.Linq;
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
        if (seller is null)
        {
            throw new ArgumentNullException(nameof(seller));
        }

        Listing listing = new Listing(title, description, category, condition, price, seller);

        Listings.Add(listing);
        seller.Listings.Add(listing);

        return listing;
    }

    /// <summary>
    /// Gets all listings.
    /// </summary>
    public List<Listing> GetAllListings()
    {
        return Listings;
    }

    /// <summary>
    /// Searches and filters available listings using LINQ.
    /// </summary>
    /// <param name="keyword">The keyword to search in title or description.</param>
    /// <param name="category">The optional category filter.</param>
    /// <param name="condition">The optional condition filter.</param>
    /// <param name="minPrice">The optional minimum price.</param>
    /// <param name="maxPrice">The optional maximum price.</param>
    /// <returns>A list of matching available listings.</returns>
    public List<Listing> SearchListings(
        string? keyword,
        Category? category,
        Condition? condition,
        decimal? minPrice,
        decimal? maxPrice)
    {
        IEnumerable<Listing> query = Listings.Where(listing => listing.Status == ListingStatus.Available);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string term = keyword.Trim().ToLower();

            query = query.Where(listing =>
                listing.Title.ToLower().Contains(term) ||
                listing.Description.ToLower().Contains(term));
        }

        if (category.HasValue)
        {
            query = query.Where(listing => listing.Category == category.Value);
        }

        if (condition.HasValue)
        {
            query = query.Where(listing => listing.Condition == condition.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(listing => listing.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(listing => listing.Price <= maxPrice.Value);
        }

        return query.ToList();
    }
}