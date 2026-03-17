using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly PurchaseService _purchaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListingUI"/> class.
    /// </summary>
    /// <param name="listingService">The listing service.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="purchaseService">The purchase service.</param>
    public ListingUI(
        ListingService listingService,
        UserService userService,
        PurchaseService purchaseService)
    {
        _listingService = listingService;
        _userService = userService;
        _purchaseService = purchaseService;
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
    /// Displays all available listings and allows the user to purchase one.
    /// </summary>
    public void BrowseListings()
    {
        try
        {
            List<Listing> availableListings = _listingService
                .GetAllListings()
                .Where(listing => listing.Status == ListingStatus.Available)
                .ToList();

            if (availableListings.Count == 0)
            {
                Console.WriteLine("No available listings found.");
                return;
            }

            Console.WriteLine("=== Available Listings ===");

            for (int i = 0; i < availableListings.Count; i++)
            {
                var listing = availableListings[i];
                Console.WriteLine(
                    $"{i + 1}. {listing.Title} | Seller: {listing.Seller.Username} | {listing.Category} | {listing.Condition} | {listing.Price} NOK");
            }

            Console.Write("\nEnter listing number to buy (0 to go back): ");
            string input = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            if (choice == 0)
            {
                return;
            }

            if (choice < 1 || choice > availableListings.Count)
            {
                Console.WriteLine("Invalid listing number.");
                return;
            }

            if (_userService.CurrentUser is null)
            {
                Console.WriteLine("You must be logged in to purchase a listing.");
                return;
            }

            Listing selectedListing = availableListings[choice - 1];

            var transaction = _purchaseService.PurchaseListing(
                _userService.CurrentUser,
                selectedListing);

            Console.WriteLine(
                $"Purchase complete! You bought '{transaction.Listing.Title}' from {transaction.Seller.Username}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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