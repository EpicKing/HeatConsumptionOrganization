using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization.Models
{
    class DBInitializer
    {
        public static void Initialize(Context dbContext, int numberOfRecords)
        {
            dbContext.Database.EnsureCreated();

            var rng = new Random();

            string[] organizations = { "ООО РИАЛ", "ООО СТК", "ООО СпецТехСити", "ООО Стройтех", "ООО Авангард" };
            string[] typesOfOwnerships = {"частная собственность", "государственная собстенность", "общая собственность"};
            string[] cities = {"Гомель", "Минск", "Брест", "Гродно", "Могилёв", "Витебск"};

            string[] surnames = {"Петров", "Иванов", "Саприко", "Сидоров", "Кулинкин", "Косинов"};
            const string alphabet = "АБВГДЕИКЛМНОПРСТФЮЯ";


            // Feel Organizations table
            for (var i = 0; i < numberOfRecords; i++)
            {
                dbContext.Organizations.Add(new Organization
                {
                    Name = organizations[rng.Next(organizations.Length - 1)],
                    TypeOfOwnership = typesOfOwnerships[rng.Next(typesOfOwnerships.Length - 1)],
                    Address = cities[rng.Next(cities.Length - 1)],

                    DirectorFullName = surnames[rng.Next(surnames.Length - 1)] + " " +
                                       alphabet[rng.Next(alphabet.Length - 1)] + ". " +
                                       alphabet[rng.Next(alphabet.Length - 1)] + ".",
                    DirectorPhoneNumber = "+37544" + rng.Next(1000000, 9999999),

                    ChiefPowerEngineerFullName = surnames[rng.Next(surnames.Length - 1)] + " " +
                                                 alphabet[rng.Next(alphabet.Length - 1)] + ". " +
                                                 alphabet[rng.Next(alphabet.Length - 1)] + ".",
                    ChiefPowerEngineerPhoneNumber = "+37544" + rng.Next(1000000, 9999999)
                });
            }

            dbContext.SaveChanges();


            string[] products = {"микропроцессоры", "банки", "катушки", "листы шифера", "рулоны"};

            // Feel TypeOfProducts table
            for (var i = 0; i < numberOfRecords; i++)
            {
                dbContext.TypeOfProducts.Add(new TypeOfProduct
                {
                    Name = products[rng.Next(products.Length - 1)],
                    Unit = "шт.",
                    OrganizationID = rng.Next(1, numberOfRecords)
                });
            }

            dbContext.SaveChanges();


            // Feel ManufacturedProducts table
            for (var i = 0; i < numberOfRecords; i++)
            {
                dbContext.ManufacturedProducts.Add(new ManufacturedProduct
                {
                    TotalProduced = rng.Next(10000),
                    Year = rng.Next(2005, 2018),
                    Quarter = rng.Next(1, 4),
                    OrganizationID = rng.Next(1, 4)
                });
            }

            dbContext.SaveChanges();


            // Feel Heat Consumption table
            for (var i = 0; i < numberOfRecords; i++)
            {
                dbContext.HeatConsumptions.Add(new HeatConsumption
                {
                    TotalConsumed = rng.Next(100, 1000),
                    Year = rng.Next(2005, 2018),
                    Quarter = rng.Next(1, 4),
                    OrganizationID = rng.Next(1, 4)
                });
            }

            dbContext.SaveChanges();


            // Feel HeatConsumptionRate table
            for (var i = 0; i < numberOfRecords; i++)
            {
                dbContext.HeatConsumptionRates.Add(new HeatConsumptionRate
                {
                    QuarterConsuptionRate = rng.Next(1, 3),
                    Year = rng.Next(2005, 2018),
                    Quarter = rng.Next(1, 4),
                    OrganizationID = rng.Next(1, 4),
                    ResponsibleOfficer = surnames[rng.Next(surnames.Length - 1)] + " " +
                                         alphabet[rng.Next(alphabet.Length - 1)] + ". " +
                                         alphabet[rng.Next(alphabet.Length - 1)] + "."
                });
            }

            dbContext.SaveChanges();

        }
    }
}
