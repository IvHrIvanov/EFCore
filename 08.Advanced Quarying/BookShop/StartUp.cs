using System;
namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContext);
            string command = "teEN";
            string result = GetAuthorNamesEndingIn(dbContext, "dy");
            Console.WriteLine(result);
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var ageRestrictionInfo = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .ToArray()
                .OrderBy(x => x.Title);

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
                .OrderByDescending(x => x.Price);
            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();
            var notRealeasedBooks = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .ToArray()
                .OrderBy(x => x.BookId);
            foreach (var book in notRealeasedBooks)
            {
                sb.AppendLine($"{book.Title}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            if (input == null)
            {
                input = Console.ReadLine();
            }

            var categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToArray();

            var books = context.Books
                .Include(x => x.BookCategories)
                .ThenInclude(x => x.Category)
                .ToArray()
               .Where(b => b.BookCategories
               .Any(c => categories.Contains(c.Category.Name.ToLower())))
               .Select(b => b.Title)
               .OrderBy(x => x);

            string result = string.Join(Environment.NewLine, books);
            return result;
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();
            var dateTimeParse = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .ToArray()
                .Where(x => x.ReleaseDate.Value < dateTimeParse)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    Title = x.Title,
                    EditionType = x.EditionType,
                    Price = x.Price,


                });
               

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            var fullName = context.Authors
                .ToArray()
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x=> new
                {
                    FullName = x.FirstName + " "+ x.LastName
                })
                .OrderBy(x => x.FullName);


            foreach (var author in fullName)
            {
                sb.AppendLine($"{author.FullName}");
            }
            return sb.ToString().TrimEnd();

        }
    }
}
