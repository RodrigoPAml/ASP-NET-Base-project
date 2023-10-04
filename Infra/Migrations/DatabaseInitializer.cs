using Domain.Exceptions;
using Domain.Models.Entities;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infra.Migrations
{
    /// <summary>
    /// Database initializer with sql application
    /// </summary>
    public static class DatabaseInitializer
    {
        private static bool IsInitialized = false;

        public static void Init(DatabaseRegisterContext db)
        {
            if (IsInitialized)
                return;

            try
            {
                var builder = new DbConnectionStringBuilder();
                builder.ConnectionString = db.Database.GetConnectionString();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Context Initializing: Host: '{builder["Host"]}', Database: '{builder["DataBase"]}'");

                ApplyMigrations(db);

                Console.ForegroundColor = ConsoleColor.White;

                IsInitialized = true;
            }
            catch (Exception e)
            {
                throw new InternalException(e.Message + e.InnerException?.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal static void ApplyMigrations(DatabaseRegisterContext db)
        {
            var database = db.Database;

            database.BeginTransaction();
            database.ExecuteSqlRaw("create schema if not exists public;\r\n\r\ncreate table if not exists public.migration (\r\n\tid bigint primary key generated always as identity,\r\n\tname text not null\r\n);\r\n");
            database.CommitTransaction();

            var sqls = Directory.GetFiles(Directory.GetCurrentDirectory() + "/migrations", "*.sql", SearchOption.AllDirectories)
                .ToList();

            sqls = sqls.OrderBy(x => x).ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Updating migrations");

            foreach (var sql in sqls)
            {
                var filename = Path.GetFileName(sql);
                var content = File.ReadAllText(sql);

                if (!db.Migrations.Any(x => x.Name == filename))
                {

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    database.BeginTransaction();

                    Console.WriteLine($"Applying sql {filename}");

                    database.ExecuteSqlRaw(content);

                    db.Migrations.Add(new Migration()
                    {
                        Name = filename
                    });

                    db.SaveChanges();
                    database.CommitTransaction();
                }
            }

            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Initialization done");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
