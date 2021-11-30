using P03_FootballBetting.Data;
using System;

namespace Student_System
{
   public class StartUp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            FootballBettingContext dbContext = new FootballBettingContext();

            dbContext.Database.EnsureCreated();

            Console.WriteLine("Db created");
            dbContext.Database.EnsureDeleted();
            Console.WriteLine("Deleted");
        }
    }
}
