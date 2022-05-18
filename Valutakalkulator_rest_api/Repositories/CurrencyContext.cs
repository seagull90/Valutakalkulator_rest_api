using Microsoft.EntityFrameworkCore;
using System;
using Valutakalkulator_rest_api.Models;

namespace Valutakalkulator_rest_api.Repositories
{
    public class CurrencyContext : DbContext
    {
        public DbSet<BaseCurrency> BaseCurrency { get; set; }
        public DbSet<Rate> Rate { get; set; }

        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BaseCurrency>(bc =>
            {
                bc.Property(bc => bc.Date)
                .HasColumnType("date");

                bc.HasMany(bc => bc.Rates)
                .WithOne(r => r.BaseCurrency)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Rate>(r =>
            {
                r.Property(x => x.ExchangeRate)
                .HasPrecision(18, 2);
            });
        }
    }
}
