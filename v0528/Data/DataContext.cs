﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;

namespace v0528.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>().HasKey(k => new { k.SourceUserId, k.LikedUserId });
            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLike>()
                 .HasOne(s => s.LikedUser)
                 .WithMany(l => l.LikedByUsers)
                 .HasForeignKey(s => s.LikedUserId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                 .HasOne(s => s.Recipient)
                 .WithMany(l => l.MessageReceived)
                 .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Message>()
                 .HasOne(s => s.Sender)
                 .WithMany(l => l.MessageSent)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
