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
            string result = GetBooksByAgeRestriction(db,command);
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
    }
}
