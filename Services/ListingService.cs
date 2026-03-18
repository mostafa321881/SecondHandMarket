using System;
using System.Collections.Generic;
using System.Linq;
using SecondHandMarket.Models;
using SecondHandMarket.Enums;

namespace SecondHandMarket.Services;

public class ListingService
{
    public List<Listing> Listings { get; }

    public ListingService()
    {
        Listings = new List<Listing>();
    }

    public Listing CreateListing(
        User seller,
        string title,
        string description,
        Category category,
        Condition condition,
        decimal price)
    {
        if (seller is null)
            throw new ArgumentNullException(nameof(seller));

        Listing listing = new Listing(title, description, category, condition, price, seller);

        Listings.Add(listing);
        seller.Listings.Add(listing);

        return listing;
    }

    public List<Listing> GetAllListings()
    {
        return Listings;
    }

    public List<Listing> SearchListings(
        string? keyword,
        Category? category,
        Condition? condition,
        decimal? minPrice,
        decimal? maxPrice)
    {
        IEnumerable<Listing> query =
            Listings.Where(l => l.Status == ListingStatus.Available);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string term = keyword.Trim().ToLower();
            query = query.Where(l =>
                l.Title.ToLower().Contains(term) ||
                l.Description.ToLower().Contains(term));
        }

        if (category.HasValue)
            query = query.Where(l => l.Category == category.Value);

        if (condition.HasValue)
            query = query.Where(l => l.Condition == condition.Value);

        if (minPrice.HasValue)
            query = query.Where(l => l.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(l => l.Price <= maxPrice.Value);

        return query.ToList();
    }

    // ⭐ NEW FEATURE — EDIT LISTING
    public void UpdateListing(
        Listing listing,
        User seller,
        string title,
        string description,
        Category category,
        Condition condition,
        decimal price)
    {
        if (listing is null)
            throw new ArgumentNullException(nameof(listing));

        if (seller is null)
            throw new ArgumentNullException(nameof(seller));

        if (listing.Seller != seller)
            throw new InvalidOperationException("You can only edit your own listings.");

        if (listing.Status == ListingStatus.Sold)
            throw new InvalidOperationException("Cannot edit a sold listing.");

        listing.UpdateDetails(title, description, category, condition, price);
    }

    // ⭐ NEW FEATURE — REMOVE LISTING
    public void RemoveListing(Listing listing, User seller)
    {
        if (listing is null)
            throw new ArgumentNullException(nameof(listing));

        if (seller is null)
            throw new ArgumentNullException(nameof(seller));

        if (listing.Seller != seller)
            throw new InvalidOperationException("You can only remove your own listings.");

        Listings.Remove(listing);
        seller.Listings.Remove(listing);
    }
}