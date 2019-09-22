namespace Carsport.Backend.Migrations
{
    using Common.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Carsport.Backend.Models.LocalDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Carsport.Backend.Models.LocalDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Stations.AddOrUpdate(x => x.Id,
                new Station() { Id = 1, Name = "Aeropuerto de Jerez", CoreId = "42" },
                new Station() { Id = 2, Name = "C�diz", CoreId = "1" },
                new Station() { Id = 3, Name = "El Puerto de Santa Mar�a", CoreId = "8" },
                new Station() { Id = 4, Name = "Jerez", CoreId = "14" },
                new Station() { Id = 5, Name = "Puerto Real", CoreId = "6" },
                new Station() { Id = 6, Name = "San Fernando", CoreId = "2" }
                );

            context.Cities.AddOrUpdate(x => x.CityId,
                new City() { CityId = 1, Name = "C�diz" },
                new City() { CityId = 2, Name = "Jerez" },
                new City() { CityId = 3, Name = "Puerto Real - ESI" },
                new City() { CityId = 4, Name = "Puerto Real - CASEM" },
                new City() { CityId = 5, Name = "Jerez - Campus" },
                new City() { CityId = 6, Name = "C�diz - Enfermeria" },
                new City() { CityId = 5, Name = "San Fernando" },
                new City() { CityId = 5, Name = "Chiclana" },
                new City() { CityId = 5, Name = "C�diz - ADE" },
                new City() { CityId = 5, Name = "C�diz - Filosof�a" },
                new City() { CityId = 5, Name = "Puerto Real" },
                new City() { CityId = 5, Name = "Puerto de Santa Mar�a" }
                );

            context.Bycicles.AddOrUpdate(x => x.BycicleId,
                new Bycicle() {
                    BycicleId = 1,
                    University = "Escuela Superior de ingenier�a",
                    Description = "Zona cubierta y segura para dejar la bicicleta.",
                    Street = "Av. Universidad de C�diz, 10, 11519 Puerto Real, C�diz",
                    Longitude = "-6.2051521",
                    Latitude = "36.537216",
                    IsAvailable = true },
                new Bycicle() {
                    BycicleId = 2,
                    University = "Universidad de C�diz: CASEM",
                    Description = "Zona cubierta y segura para dejar la bicicleta.",
                    Street = "Avenida Rep�blica �rabe Saharawi, s/n, 11510 Puerto Real, C�diz",
                    Longitude = "-6.210662",
                    Latitude = "36.530100",
                    IsAvailable = true },
                new Bycicle() {
                    BycicleId = 3,
                    University = "UCA: Facultad de Filosof�a y Letras",
                    Description = "Zona indor preparada para dejar la bicieta.",
                    Street = "Av. Dr. G�mez Ulla, 1, 11003 C�diz",
                    Longitude = "-6.3017375",
                    Latitude = "36.5335889",
                    IsAvailable = true }
                );
        }
    }
}
