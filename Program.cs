using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GameStudioApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new GameContext())
            {
                db.Database.EnsureCreated();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== GAME STUDIOS MANAGEMENT ===");
                    Console.WriteLine("1. Show all studios");
                    Console.WriteLine("2. Show all games");
                    Console.WriteLine("3. Add studio");
                    Console.WriteLine("4. Edit studio");
                    Console.WriteLine("5. Delete studio");
                    Console.WriteLine("0. Exit");
                    Console.Write("\nYour choice: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ShowStudios(db); break;
                        case "2": ShowGames(db); break;
                        case "3": AddStudio(db); break;
                        case "4": EditStudio(db); break;
                        case "5": DeleteStudio(db); break;
                        case "0": return;
                    }
                    Console.WriteLine("\nPress any key...");
                    Console.ReadKey();
                }
            }
        }

        static void ShowStudios(GameContext db)
        {
            Console.WriteLine("\n--- Studios ---");
            var studios = db.Studios.Include(s => s.City).ThenInclude(c => c.Country).ToList();
            foreach (var s in studios)
                Console.WriteLine($"[{s.Id}] {s.Name} ({s.City.Name}, {s.City.Country.Name}) - founded {s.FoundationYear}");
        }

        static void ShowGames(GameContext db)
        {
            Console.WriteLine("\n--- Games ---");
            var games = db.Games.Include(g => g.Studio).ToList();
            foreach (var g in games)
                Console.WriteLine($"[{g.Id}] {g.Title} ({g.Genre}, {g.ReleaseYear}) - {g.Price} USD, studio: {g.Studio.Name}");
        }

        static void AddStudio(GameContext db)
        {
            Console.Write("Studio name: ");
            var name = Console.ReadLine()!;

            if (db.Studios.Any(s => s.Name == name))
            {
                Console.WriteLine("A studio with this name already exists!");
                return;
            }

            Console.Write("City id: ");
            var cityId = int.Parse(Console.ReadLine()!);

            if (!db.Cities.Any(c => c.Id == cityId))
            {
                Console.WriteLine("City not found!");
                return;
            }

            Console.Write("Foundation year: ");
            var year = int.Parse(Console.ReadLine()!);

            db.Studios.Add(new Studio { Name = name, CityId = cityId, FoundationYear = year });
            db.SaveChanges();
            Console.WriteLine("Studio added successfully!");
        }

        static void EditStudio(GameContext db)
        {
            ShowStudios(db);
            Console.Write("\nEnter studio id to edit: ");
            var id = int.Parse(Console.ReadLine()!);

            var studio = db.Studios.Find(id);
            if (studio == null) { Console.WriteLine("Studio not found!"); return; }

            Console.Write($"New name (current: {studio.Name}, press Enter to keep): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) studio.Name = newName;

            Console.Write($"New city id (current: {studio.CityId}, press Enter to keep): ");
            var newCityId = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCityId)) studio.CityId = int.Parse(newCityId);

            Console.Write($"New foundation year (current: {studio.FoundationYear}, press Enter to keep): ");
            var newYear = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newYear)) studio.FoundationYear = int.Parse(newYear);

            db.SaveChanges();
            Console.WriteLine("Studio updated!");
        }

        static void DeleteStudio(GameContext db)
        {
            ShowStudios(db);
            Console.Write("\nEnter studio name to delete: ");
            var name = Console.ReadLine()!;

            var studio = db.Studios.FirstOrDefault(s => s.Name == name);
            if (studio == null) { Console.WriteLine("Studio not found!"); return; }

            Console.Write($"Are you sure you want to delete '{studio.Name}'? (y/n): ");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() != "y") { Console.WriteLine("Cancelled."); return; }

            db.Studios.Remove(studio);
            db.SaveChanges();
            Console.WriteLine("Studio deleted!");
        }
    }
}
