using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Drawing;
using Writers.Models;
using System.IO;

namespace Writers.DAL
{
    public class WritersInitializer : DropCreateDatabaseIfModelChanges<WritersContext>
    {
        protected override void Seed(WritersContext context)
        {
            var persons = new List<Person>()
            {
                new Person {FullName = "William Shakespeare", BirthDate = DateTime.Parse("1564-04-23"),
                    BirthPlace = "Stratford-upon-Avon, England", DieDate = DateTime.Parse("1616-04-23"), DiePlace = "Stratford-upon-Avon, England", Occupation = "Playwright, poet, actor",
                    Biography = "Bio here", Century = "16th-17th", Country = "England", Genres = new List<Genre>() },
                new Person {FullName = "Fyodor Dostoevsky", BirthDate = DateTime.Parse("1821-11-01"),
                    BirthPlace = "Moscow, Russian Empire", DieDate = DateTime.Parse("1881-02-09"), DiePlace = "Saint Petersburg, Russian Empire", Education = "Military Engineering Technical University, St. Petersburg",
                    Occupation = "Novelist, interpreter, philosopher, military engineer", Biography = "Bio here", Century = "19th", Country = "Russian Empire", Genres = new List<Genre>() },
                new Person {FullName = "Charles Dickens", BirthDate = DateTime.Parse("1812-02-07"),
                    BirthPlace = "Landport, England", DieDate = DateTime.Parse("1870-06-09"), DiePlace = "Higham, England",
                    Occupation = "Novelist", Biography = "Bio here", Century = "19th", Country = "England", Genres = new List<Genre>() },
                new Person {FullName = "Anton Chekhov", BirthDate = DateTime.Parse("1860-01-29"),
                    BirthPlace = "Taganrog, Russian Empire", DieDate = DateTime.Parse("1904-07-15"), DiePlace = "Badenweiler, German Empire", Education = "First Moscow State Medical University, Moscow",
                    Occupation = "Novelist, playwright, physician", Biography = "Bio here", Century = "19th-20th", Country = "Russian Empire", Genres = new List<Genre>()},
                new Person {FullName = "Gustave Flaubert", BirthDate = DateTime.Parse("1821-12-12"),
                    BirthPlace = "Rouen, France", DieDate = DateTime.Parse("1880-05-08"), DiePlace = "Rouen, France",
                    Occupation = "Novelist", Biography = "Bio here", Century = "19th", Country = "France", Genres = new List<Genre>()},
                new Person {FullName = "Victor Hugo", BirthDate = DateTime.Parse("1802-02-26"),
                    BirthPlace = "Besancon, Doubs, France", DieDate = DateTime.Parse("1885-05-22"), DiePlace = "Paris, France", Education = "Lycee Louis-le-Grand, Paris",
                    Occupation = "Novelist, poet, journalist, drawer", Biography = "Bio here", Century = "19th", Country = "France", Genres = new List<Genre>()},
                new Person {FullName = "Rudyard Kipling", BirthDate = DateTime.Parse("1865-12-30"),
                    BirthPlace = "Bombay, British India", DieDate = DateTime.Parse("1936-01-18"), DiePlace = "London, England",
                    Occupation = "Novelist, poet, journalist", Biography = "Bio here", Century = "19th-20th", Country = "England", Genres = new List<Genre>()},
                new Person {FullName = "Erich Maria Remarque", BirthDate = DateTime.Parse("1898-06-22"),
                    BirthPlace = "Osnabrück, German Empire", DieDate = DateTime.Parse("1970-09-25"), DiePlace = "Locarno, Swiss Confederation",
                    Occupation = "Novelist", Biography = "Bio here", Century = "19th-20th", Country = "Germany", Genres = new List<Genre>()},
                new Person {FullName = "Richard Aldington", BirthDate = DateTime.Parse("1892-07-08"),
                    BirthPlace = "Portsmouth, England", DieDate = DateTime.Parse("1962-07-27"), DiePlace = "Sury-en-Vaux, France",
                    Occupation = "Novelist, poet", Biography = "Bio here", Century = "19th-20th", Country = "England", Genres = new List<Genre>()},
                new Person {FullName = "Jane Austen", BirthDate = DateTime.Parse("1775-12-16"),
                    BirthPlace = "Steventon, England", DieDate = DateTime.Parse("1817-07-18"), DiePlace = "Winchester, England",
                    Occupation = "Novelist", Biography = "Bio here", Century = "18th-19th", Country = "England", Genres = new List<Genre>()}
            };

            persons.ForEach(p => context.Persons.Add(p));
            context.SaveChanges();

            //var biograpies = new List<Biography>()
            //{
            //    new Biography {PersonFullName = "William Shakespeare", EarlyLife = new List<string>() { "Childhood" } }
            //};

            var images = new List<Images>()
            {
                new Images {ImagePath = "/Content/Images/Shakespeare.jpg"},
                new Images {ImagePath = "/Content/Images/Dostoevsky.jpg"},
                new Images {ImagePath = "/Content/Images/Dickens.jpg"},
                new Images {ImagePath = "/Content/Images/Chekhov.jpg"},
                new Images {ImagePath = "/Content/Images/Flaubert.jpg"},
                new Images {ImagePath = "/Content/Images/Hugo.jpg"},
                new Images {ImagePath = "/Content/Images/Kipling.jpg"},
                new Images {ImagePath = "/Content/Images/Remarque.jpeg"},
                new Images {ImagePath = "/Content/Images/Aldington.jpg"},
                new Images {ImagePath = "/Content/Images/Austen.jpg"}
            };

            images.ForEach(p => context.Images.Add(p));
            context.SaveChanges();

            var works = new List<Work>()
            {
                new Work {AuthorFullName = "William Shakespeare", Title = "Macbeth", ReleaseYear = 1623, ReleasePlace = "London"},
                new Work {AuthorFullName = "Fyodor Dostoevsky", Title = "Crime and Punishment", ReleaseYear = 1886, ReleasePlace = "Moscow"},
                new Work {AuthorFullName = "Charles Dickens", Title = "Oliver Twist", ReleaseYear = 1839, ReleasePlace = "London"},
                new Work {AuthorFullName = "Anton Chekhov", Title = "The Cherry Orchard", ReleaseYear = 1904, ReleasePlace = "Saint Petersburg"},
                new Work {AuthorFullName = "Gustave Flaubert", Title = "Madame Bovary", ReleaseYear = 1856, ReleasePlace = "Paris"},
                new Work {AuthorFullName = "Victor Hugo", Title = "The Toilers of the Sea", ReleaseYear = 1866, ReleasePlace = "Paris"},
                new Work {AuthorFullName = "Rudyard Kipling", Title = "Just So Stories", ReleaseYear = 1902, ReleasePlace = "London"},
                new Work {AuthorFullName = "Erich Maria Remarque", Title = "All Quiet on the Western Front", ReleaseYear = 1929, ReleasePlace = "Berlin"},
                new Work {AuthorFullName = "Richard Aldington", Title = "Death of a Hero", ReleaseYear = 1929, ReleasePlace = "London"},
                new Work {AuthorFullName = "Jane Austen", Title = "Mansfield Park", ReleaseYear = 1814, ReleasePlace = "London"}
            };

            works.ForEach(w => context.Works.Add(w));
            context.SaveChanges();

            var genres = new List<Genre>()
            {
                new Genre {Title = "Poem"},
                new Genre {Title = "Novel"},
                new Genre {Title = "Drama"},
                new Genre {Title = "Short story"},
                new Genre {Title = "Novella"}
            };

            genres.ForEach(g => context.Genres.Add(g));
            context.SaveChanges();

            AddOrUpdateGenre(context, "Fyodor Dostoevsky", "Novel");
            AddOrUpdateGenre(context, "Fyodor Dostoevsky", "Novella");
        }

        public void AddOrUpdateGenre(WritersContext context, string personFullName, string genreTitle)
        {
            var person = context.Persons.SingleOrDefault(p => p.FullName == personFullName);
            var genre = person.Genres.SingleOrDefault(g => g.Title == genreTitle);
            if (genre == null)
                person.Genres.Add(context.Genres.SingleOrDefault(g => g.Title == genreTitle));
        }
    }
}