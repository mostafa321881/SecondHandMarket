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
            Category[] categories = Enum.GetValues<Category>();

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
            Condition[] conditions = Enum.GetValues<Condition>();

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

            Listing listing = _listingService.CreateListing(
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
                Listing listing = availableListings[i];
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

            Transaction transaction = _purchaseService.PurchaseListing(
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
    /// Searches available listings using optional filters.
    /// </summary>
    public void SearchListings()
    {
        try
        {
            Console.WriteLine("=== Search Listings ===");

            Console.Write("Enter keyword (or leave blank): ");
            string? keyword = Console.ReadLine();

            Category? selectedCategory = null;
            Console.Write("Filter by category? (y/n): ");
            string categoryChoice = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

            if (categoryChoice == "y")
            {
                Category[] categories = Enum.GetValues<Category>();

                Console.WriteLine("\nAvailable categories:");
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                Console.Write("Select category number: ");
                string categoryInput = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(categoryInput, out int categoryIndex) &&
                    categoryIndex >= 1 &&
                    categoryIndex <= categories.Length)
                {
                    selectedCategory = categories[categoryIndex - 1];
                }
                else
                {
                    Console.WriteLine("Invalid category selection.");
                    return;
                }
            }

            Condition? selectedCondition = null;
            Console.Write("Filter by condition? (y/n): ");
            string conditionChoice = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

            if (conditionChoice == "y")
            {
                Condition[] conditions = Enum.GetValues<Condition>();

                Console.WriteLine("\nAvailable conditions:");
                for (int i = 0; i < conditions.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {conditions[i]}");
                }

                Console.Write("Select condition number: ");
                string conditionInput = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(conditionInput, out int conditionIndex) &&
                    conditionIndex >= 1 &&
                    conditionIndex <= conditions.Length)
                {
                    selectedCondition = conditions[conditionIndex - 1];
                }
                else
                {
                    Console.WriteLine("Invalid condition selection.");
                    return;
                }
            }

            decimal? minPrice = null;
            Console.Write("Enter minimum price (or leave blank): ");
            string minInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(minInput))
            {
                if (decimal.TryParse(minInput, out decimal minValue) && minValue >= 0)
                {
                    minPrice = minValue;
                }
                else
                {
                    Console.WriteLine("Invalid minimum price.");
                    return;
                }
            }

            decimal? maxPrice = null;
            Console.Write("Enter maximum price (or leave blank): ");
            string maxInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(maxInput))
            {
                if (decimal.TryParse(maxInput, out decimal maxValue) && maxValue >= 0)
                {
                    maxPrice = maxValue;
                }
                else
                {
                    Console.WriteLine("Invalid maximum price.");
                    return;
                }
            }

            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
            {
                Console.WriteLine("Minimum price cannot be greater than maximum price.");
                return;
            }

            List<Listing> results = _listingService.SearchListings(
                keyword,
                selectedCategory,
                selectedCondition,
                minPrice,
                maxPrice);

            Console.WriteLine();
            Console.WriteLine("=== Search Results ===");

            if (results.Count == 0)
            {
                Console.WriteLine("No matching listings found.");
                return;
            }

            for (int i = 0; i < results.Count; i++)
            {
                Listing listing = results[i];
                Console.WriteLine(
                    $"{i + 1}. {listing.Title} | Seller: {listing.Seller.Username} | {listing.Category} | {listing.Condition} | {listing.Price} NOK");
            }
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
        foreach (Listing listing in myListings)
        {
            Console.WriteLine(
                $"{number}. {listing.Title} | {listing.Category} | {listing.Condition} | {listing.Price} NOK | {listing.Status}");
            number++;
        }
    }
    public void ManageMyListings()
{
    if (_userService.CurrentUser is null)
    {
        Console.WriteLine("You must be logged in.");
        return;
    }

    List<Listing> myListings = _userService.CurrentUser.Listings;

    if (myListings.Count == 0)
    {
        Console.WriteLine("You have no listings.");
        return;
    }

    Console.WriteLine("=== Manage My Listings ===");

    for (int i = 0; i < myListings.Count; i++)
    {
        Listing l = myListings[i];
        Console.WriteLine($"{i + 1}. {l.Title} | {l.Price} NOK | {l.Status}");
    }

    Console.Write("Select listing (0 to cancel): ");
    if (!int.TryParse(Console.ReadLine(), out int choice) ||
        choice < 0 || choice > myListings.Count)
    {
        Console.WriteLine("Invalid choice.");
        return;
    }

    if (choice == 0)
        return;

    Listing selected = myListings[choice - 1];

    Console.WriteLine("1. Edit");
    Console.WriteLine("2. Remove");
    Console.Write("Choose option: ");
    string action = Console.ReadLine() ?? "";

    try
    {
        if (action == "1")
        {
            Console.Write("New title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("New description: ");
            string desc = Console.ReadLine() ?? "";

            Console.Write("New price: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            _listingService.UpdateListing(
                selected,
                _userService.CurrentUser,
                title,
                desc,
                selected.Category,
                selected.Condition,
                price);

            Console.WriteLine("Listing updated.");
        }
        else if (action == "2")
        {
            _listingService.RemoveListing(
                selected,
                _userService.CurrentUser);

            Console.WriteLine("Listing removed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
}