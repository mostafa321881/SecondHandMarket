using System;
using System.Collections.Generic;
using SecondHandMarket.Models;
using SecondHandMarket.Services;

namespace SecondHandMarket.UI;

/// <summary>
/// Handles console interaction for transaction history features.
/// </summary>
public class TransactionUI
{
    private readonly PurchaseService _purchaseService;
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionUI"/> class.
    /// </summary>
    /// <param name="purchaseService">The purchase service.</param>
    /// <param name="userService">The user service.</param>
    public TransactionUI(PurchaseService purchaseService, UserService userService)
    {
        _purchaseService = purchaseService;
        _userService = userService;
    }

    /// <summary>
    /// Displays the transaction history menu.
    /// </summary>
    public void ShowTransactionHistoryMenu()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to view transaction history.");
            return;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Transaction History ===");
            Console.WriteLine("1. View bought items");
            Console.WriteLine("2. View sold items");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowBoughtTransactions();
                    break;
                case "2":
                    ShowSoldTransactions();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Displays transactions where the current user is the buyer.
    /// </summary>
    public void ShowBoughtTransactions()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to view bought items.");
            return;
        }

        List<Transaction> transactions =
            _purchaseService.GetBoughtTransactions(_userService.CurrentUser);

        Console.WriteLine();
        Console.WriteLine("=== Bought Items ===");

        if (transactions.Count == 0)
        {
            Console.WriteLine("You have not bought any items yet.");
            return;
        }

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine($"Item: {transaction.Listing.Title}");
            Console.WriteLine($"Price: {transaction.Price} NOK");
            Console.WriteLine($"Date: {transaction.PurchasedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Seller: {transaction.Seller.Username}");
            Console.WriteLine(new string('-', 30));
        }
    }

    /// <summary>
    /// Displays transactions where the current user is the seller.
    /// </summary>
    public void ShowSoldTransactions()
    {
        if (_userService.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to view sold items.");
            return;
        }

        List<Transaction> transactions =
            _purchaseService.GetSoldTransactions(_userService.CurrentUser);

        Console.WriteLine();
        Console.WriteLine("=== Sold Items ===");

        if (transactions.Count == 0)
        {
            Console.WriteLine("You have not sold any items yet.");
            return;
        }

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine($"Item: {transaction.Listing.Title}");
            Console.WriteLine($"Price: {transaction.Price} NOK");
            Console.WriteLine($"Date: {transaction.PurchasedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Buyer: {transaction.Buyer.Username}");
            Console.WriteLine(new string('-', 30));
        }
    }
}