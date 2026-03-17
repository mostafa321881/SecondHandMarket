using System;
using System.Collections.Generic;
using SecondHandMarket.Enums;
using SecondHandMarket.Models;
using SecondHandMarket.Services;

namespace SecondHandMarket.UI;

/// <summary>
/// Handles console interaction for listing-related features.
/// </summary>
public class ListingUI
{
    private readonly ListingService _listingService;
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListingUI"/> class.
    /// </summary>
    /// <param name="listingService">The listing service.</param>
    /// <param name="userService">The user service.</param>
    public ListingUI(ListingService listingService, UserService userService)
    {
        _listingService = listingService;
        _userService = userService;
    }

    /// <summary>
    /// Prompts the logged-in user to create a listing.
    /// </summary>
    public void CreateListing()
    {
        try
        {
            if (_userService.CurrentUser is null)
            {
                Console.WriteLine("You must be logged in to create a listing.");
                return;
            }

            Console.Write("Enter title: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter description: ");
            string description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("\nAvailable categories:");
            var categories = Enum.GetValues<Category>();

            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            Console.Write("\nSelect category number: ");
            string categoryInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(categoryInput, out int categoryChoice) ||
                categoryChoice < 1 ||
                categoryChoice > categories.Length)
            {
                Console.WriteLine("Invalid category selection.");
                return;
            }

            Category categoryValue = categories[categoryChoice - 1];

            Console.WriteLine("\nAvailable conditions:");
            var conditions = Enum.GetValues<Condition>();

            for (int i = 0; i < conditions.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {conditions[i]}");
            }

            Console.Write("\nSelect condition number: ");
            string conditionInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(conditionInput, out int conditionChoice) ||
                conditionChoice < 1 ||
                conditionChoice > conditions.Length)
            {
                Console.WriteLine("Invalid condition selection.");
                return;
            }

            Condition conditionValue = conditions[conditionChoice - 1];

            Console.Write("Enter price in NOK: ");
            string priceInput = Console.ReadLine() ?? string.Empty;

            if (!decimal.TryParse(priceInput, out decimal price) || price <= 0)
            {
                Console.WriteLine("Invalid price. Price must be greater than 0.");
                return;
            }

            var listing = _listingService.CreateListing(
                _userService.CurrentUser,
                title,
                description,
                categoryValue,
                conditionValue,
                price);

            Console.WriteLine($"Listing '{listing.Title}' created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Displays all available listings.
    /// </summary>
    public void BrowseListings()
    {
        List<Listing> listings = _listingService.GetAllListings();

        if (listings.Count == 0)
        {
            Console.WriteLine("No listings available.");
            return;
        }

        Console.WriteLine("=== Available Listings ===");

        int number = 1;
        foreach (var listing in listings)
        {
            if (listing.Status == ListingStatus.Available)
            {
                Console.WriteLine(
                    $"{number}. {listing.Title} | {listing.Category} | {listing.Condition} | {listing.Price} NOK");
                number++;
            }
        }

        if (number == 1)
        {
            Console.WriteLine("No available listings found.");
        }
    }

    /// <summary>
    /// Displays the listings created by the currently logged-in user.
    /// </summary>
    public void ShowMyListings()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to view your listings.");
            return;
        }

        List<Listing> myListings = _userService.CurrentUser.Listings;

        if (myListings.Count == 0)
        {
            Console.WriteLine("You have not created any listings yet.");
            return;
        }

        Console.WriteLine("=== My Listings ===");

        int number = 1;
        foreach (var listing in myListings)
        {
            Console.WriteLine(
                $"{number}. {listing.Title} | {listing.Category} | {listing.Condition} | {listing.Price} NOK | {listing.Status}");
            number++;
        }
    }
}