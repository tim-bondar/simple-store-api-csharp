using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SimpleStore.DB;
using SimpleStore.Models;
using SimpleStore.Services;

namespace SimpleStore.Helpers
{
    public static class DatabaseSeeder
    {
        public static void Seed(SimpleStoreContext context)
        {
            var conn = (NpgsqlConnection)context.Database.GetDbConnection();

            // Do not change DB if it has been initiated
            if (IsDBInitiated(conn))
                return;

            // Create table structure
            using (var cmd = new NpgsqlCommand(@"CREATE TABLE IF NOT EXISTS StoreItems(
                        id uuid,
                        title varchar(50) NOT NULL,
                        description varchar(255),
                        isAvailable boolean NOT NULL,
                        price decimal NOT NULL,
                        PRIMARY KEY (id)
               );", conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand(@"CREATE TABLE IF NOT EXISTS Users(
                        id uuid,
                        firstName varchar(128) NOT NULL,
                        lastName varchar(128) NOT NULL,
                        username varchar(128) NOT NULL,
                        isAdmin boolean NOT NULL,
                        passwordHash varchar(512) NOT NULL,
                        PRIMARY KEY (id)
               );", conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Seed data
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Adminson",
                    IsAdmin = true,
                    UserName = "admin",
                    PasswordHash = UserService.GetPasswordHash("admin")
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "User",
                    LastName = "Userson",
                    IsAdmin = false,
                    UserName = "user",
                    PasswordHash = UserService.GetPasswordHash("user")
                }
            };

            var items = new List<StoreItem>();

            for (int i = 0; i < 8; i++)
            {
                items.Add(new StoreItem
                {
                    Id = Guid.NewGuid(),
                    Title = $"Item {i}",
                    Description = $"Test Item {i}",
                    Price = i * 13.5m,
                    IsAvailable = i % 3 != 0
                });
            }

            context.Users.AddRange(users);
            context.StoreItems.AddRange(items);
            context.SaveChanges();
        }

        private static bool IsDBInitiated(DbConnection conn)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM information_schema.tables WHERE table_name = 'storeitems'"))
            {
                if (cmd.Connection == null)
                    cmd.Connection = (NpgsqlConnection)conn;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                lock (cmd)
                {
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            if (rdr != null && rdr.HasRows)
                                return true;
                            return false;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
