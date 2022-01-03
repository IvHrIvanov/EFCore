using System;
namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            string command = "teEN";
            string result = GetBooksByPrice(db);
            Console.WriteLine(result);
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var ageRestrictionInfo = context.Books
                .Where(x => x.AgeRestriction==ageRestriction)
                .ToArray()
                .OrderBy(x=>x.Title);

            foreach (var item in ageRestrictionInfo)
            {
                sb.AppendLine(item.Title);
            }
            return sb.ToString().TrimEnd();

        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var copies = context.Books
                .Where(c => c.Copies < 5000 && c.EditionType == EditionType.Gold)
                .ToArray()
                .OrderBy(x => x.BookId);
            foreach (var copi in copies)
            {
                sb.AppendLine(copi.Title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var booksByPrice = context.Books
                .Where(x => x.Price > 40)
                .ToArray()
                .OrderByDescending(x=>x.Price);
            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
