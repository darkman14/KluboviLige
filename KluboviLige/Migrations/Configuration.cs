namespace KluboviLige.Migrations
{
    using KluboviLige.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KluboviLige.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KluboviLige.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Leagues.AddOrUpdate(l => l.Id,
                new League() { Id = 1, Name = "Premier League", AbbName="PREM", YearOfEst=1992},
                new League() { Id = 2, Name = "La Liga", AbbName="LALI", YearOfEst= 1904},
                new League() { Id = 3, Name = "Serie A", AbbName="SERA", YearOfEst=1901},
                new League() { Id = 4, Name = "Eredivisie", AbbName="ERED", YearOfEst= 1984 },
                new League() { Id = 5, Name = "Bundesliga", AbbName="BUNL", YearOfEst=1956}
                );

            context.Clubs.AddOrUpdate(c => c.Id,
                new Club() { Id = 1, Name = "Arsenal FC",   Town = "London", YearOfEst = 1886, Price=1250, LeagueId=1 },
                new Club() { Id = 2, Name = "Barcelona CF", Town = "Barcelona", YearOfEst = 1904, Price=993, LeagueId=2 },
                new Club() { Id = 3, Name = "Juventus FC",  Town = "Torino", YearOfEst = 1923, Price=1324, LeagueId=3},
                new Club() { Id = 4, Name = "PSV",   Town = "Eindhoven", YearOfEst = 1953, Price=847, LeagueId=4},
                new Club() { Id = 5, Name = "Bayern Munich",   Town = "Munich", YearOfEst = 1967, Price=1456, LeagueId=5}
                );


        }
    }
}
