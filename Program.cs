using SecondHandMarket.Services;
using SecondHandMarket.UI;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        var userService = new UserService();
        var listingService = new ListingService();

        var menu = new MenuUI(userService, listingService);
        menu.Start();
    }
}