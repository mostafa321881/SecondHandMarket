using SecondHandMarket.Enums;
using SecondHandMarket.Services;
using SecondHandMarket.UI;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        var userService = new UserService();
        var listingService = new ListingService();
        var purchaseService = new PurchaseService();

        SeedTestData(userService, listingService);

        var menu = new MenuUI(userService, listingService, purchaseService);
        menu.Start();
    }

    private static void SeedTestData(UserService userService, ListingService listingService)
    {
        var user1 = userService.Register("mostafa", "1234");
        var user2 = userService.Register("erik", "1234");

        listingService.CreateListing(
            user1,
            "iPhone 13",
            "Very good condition phone with charger",
            Category.Electronics,
            Condition.Good,
            7000);

        listingService.CreateListing(
            user2,
            "Wooden Table",
            "Large dining table for home",
            Category.FurnitureAndHome,
            Condition.Fair,
            1500);

        listingService.CreateListing(
            user1,
            "Running Shoes",
            "Used running shoes, size 42",
            Category.SportsAndOutdoors,
            Condition.LikeNew,
            600);
    }
}