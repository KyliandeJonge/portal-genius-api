﻿using Microsoft.EntityFrameworkCore;
using PortalGenius.Core.Models;

namespace PortalGenius.WPF.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items => Set<Item>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
