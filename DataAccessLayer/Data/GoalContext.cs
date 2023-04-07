﻿using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class GoalContext : DbContext
    {
        public GoalContext(DbContextOptions<GoalContext> options)
                    : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<DataAccessLayer.Models.Goal> GoalList { get; set; } = default!;
        public DbSet<DataAccessLayer.Models.Member> MembersIds { get; set; }
        public DbSet<DataAccessLayer.Models.GoalTask> GoalTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Goal>().HasIndex(u => new { u.Id });
        }
    }
}