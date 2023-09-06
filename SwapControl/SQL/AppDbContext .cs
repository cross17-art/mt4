using Microsoft.EntityFrameworkCore;
using SwapControl.SQL.Entities;
using SwapControl.SQL.Interface;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Configs.Products;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapControl.SQL
{
    public class AppDbContext : DbContext, IPostgreSQL
    {
        private readonly string connectionString;
        public DbSet<Symbol> Symbol { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupSymbol> GroupSymbol { get; set; }

        public AppDbContext(string conStr)
        {
            this.connectionString = conStr;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@$"{connectionString}");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Symbol>().HasIndex(symbol => symbol.SymbolName).IsUnique(true);
           modelBuilder.Entity<Group>().HasIndex(group => group.GroupName).IsUnique(true);

            modelBuilder.Entity<GroupSymbol>()
            .HasKey(gs => new { gs.symbolId, gs.groupId });

            modelBuilder.Entity<GroupSymbol>()
                .HasOne(gs => gs.symbol)
                .WithMany(s => s.groupSymbol)
                .HasForeignKey(gs => gs.symbolId);

            modelBuilder.Entity<GroupSymbol>()
                .HasOne(gs => gs.group)
                .WithMany(g => g.groupSymbol)
                .HasForeignKey(gs => gs.groupId);
        }

        public bool isConnected()
        {
            return this.Database.CanConnect();
        }

        void IPostgreSQL.SaveChanges()
        {
            this.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
           return this.SaveChangesAsync(CancellationToken.None);
        }
    }
}
