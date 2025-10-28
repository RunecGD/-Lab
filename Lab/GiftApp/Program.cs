using Services;

namespace GiftApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var giftService = new GiftService();
            giftService.CreateGift();
        }
    }
}