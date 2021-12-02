using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentSystemContext dbContext = new StudentSystemContext();
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Db Is Created");
            dbContext.Database.EnsureDeleted();
        }
    }
}
