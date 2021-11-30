using Microsoft.EntityFrameworkCore;
using System;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext :DbContext
    {

        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
