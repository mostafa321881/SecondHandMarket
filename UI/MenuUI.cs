using System;
using SecondHandMarket.Services;

namespace SecondHandMarket.UI;

/// <summary>
/// Handles the main menu flow of the application.
/// </summary>
public class MenuUI
{
    private readonly UserService _userService;
    private readonly ListingService _listingService;
    private readonly PurchaseService _purchaseService;
    private readonly AuthUI _authUI;
    private readonly ListingUI _listingUI;
    private readonly TransactionUI _transactionUI;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuUI"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="listingService">The listing service.</param>
    /// <param name="purchaseService">The purchase service.</param>
    public MenuUI(
        UserService userService,
        ListingService listingService,
        PurchaseService purchaseService)
    {
        _userService = userService;
        _listingService = listingService;
        _purchaseService = purchaseService;
        _authUI = new AuthUI(userService);
        _listingUI = new ListingUI(listingService, userService, purchaseService);
        _transactionUI = new TransactionUI(purchaseService, userService);
    }

    /// <summary>
    /// Starts the application menu loop.
    /// </summary>
    public void Start()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();

            if (_userService.CurrentUser is null)
            {
                ShowGuestMenu();
                string input = Console.ReadLine() ?? string.Empty;

                switch (input)
                {
                    case "1":
                        _authUI.Register();
                        Pause();
                        break;

                    case "2":
                        _authUI.Login();
                        Pause();
                        break;

                    case "3":
                        Console.WriteLine("Goodbye!");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Pause();
                        break;
                }
            }
            else
            {
                ShowUserMenu();
                string input = Console.ReadLine() ?? string.Empty;

                switch (input)
                {
                    case "1":
                        _listingUI.CreateListing();
                        Pause();
                        break;

                    case "2":
                        _listingUI.BrowseListings();
                        Pause();
                        break;

                    case "3":
                        Console.WriteLine("Search Listings - coming next.");
                        Pause();
                        break;

                    case "4":
                        _listingUI.ShowMyListings();
                        Pause();
                        break;

                    case "5":
                        _transactionUI.ShowTransactionHistoryMenu();
                        Pause();
                        break;

                    case "6":
                        Console.WriteLine("My Reviews - coming next.");
                        Pause();
                        break;

                    case "7":
                        _authUI.Logout();
                        Pause();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Pause();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Displays the guest menu.
    /// </summary>
    private void ShowGuestMenu()
    {
        Console.WriteLine("=== Second-Hand Market ===");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Exit");
        Console.Write("Select an option: ");
    }

    /// <summary>
    /// Displays the main menu for a logged-in user.
    /// </summary>
    private void ShowUserMenu()
    {
        Console.WriteLine("=== Main Menu ===");
        Console.WriteLine($"Logged in as: {_userService.CurrentUser!.Username}");
        Console.WriteLine("1. Create Listing");
        Console.WriteLine("2. Browse Listings / Buy Listing");
        Console.WriteLine("3. Search Listings");
        Console.WriteLine("4. My Listings");
        Console.WriteLine("5. Transaction History");
        Console.WriteLine("6. My Reviews");
        Console.WriteLine("7. Log Out");
        Console.Write("Select an option: ");
    }

    /// <summary>
    /// Pauses execution until the user presses a key.
    /// </summary>
    private void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}