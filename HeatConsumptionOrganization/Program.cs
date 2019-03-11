using System;
using System.Collections;
using System.Linq;
using HeatConsumptionOrganization.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace HeatConsumptionOrganization
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new Context())
            {
                var numberOfRecords = Convert.ToInt32(Console.ReadLine());
                DBInitializer.Initialize(dbContext, numberOfRecords);
            }
            Console.WriteLine("Hello World!");
        }

        static void Print(string sqlText, IEnumerable items)
        {
            Console.WriteLine(sqlText);
            Console.WriteLine("Записи: ");
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.ReadKey();
        }
        // Need fixes
        static void Second(Context dbContext)
        {
            var queryLinq = from manufacturedProduct in dbContext.ManufacturedProducts
                where manufacturedProduct.Year < 2017 && manufacturedProduct.Quarter == 3
                orderby manufacturedProduct.TotalProduced descending
                select new
                {
                    manufacturedProduct.TotalProduced,
                    manufacturedProduct.Year,
                    manufacturedProduct.Quarter,
                    manufacturedProduct.OrganizationID
                };

        }

        static void Third(Context dContext)
        {
            var queryLinq = (from heatConsumption in dContext.HeatConsumptions select heatConsumption.TotalConsumed).Average();
        }

        static void SelectAllOrganizations(Context dbContext)
        {
            var queryLinq = from organization in dbContext.Organizations
                join typeOfProduct in dbContext.TypeOfProducts 
                    on organization.OrganizationID equals typeOfProduct.OrganizationID
                where organization.Address == "Гомель"
                orderby organization.DirectorFullName descending
                select new
                {
                    Название = organization.Name,
                    Форма_собственности = organization.TypeOfOwnership,
                    Адрес = organization.Address,
                    ФИО_руководителя = organization.DirectorFullName,
                    Телефон_руководителя = organization.DirectorPhoneNumber,
                    ФИО_главного_энергетика = organization.ChiefPowerEngineerFullName,
                    Телефон_главного_энергетика = organization.ChiefPowerEngineerPhoneNumber
                };

            var comment = "Результат выполнения запроса на ";
            Print(comment, queryLinq.ToList());
        }

        static void Insert(Context dbContext)
        {
            var organization = new Organization
            {   
                Name = "ООО ДУЛЮК И КИРПИЧИ",
                TypeOfOwnership = "частная собственность",
                Address = "Гомель",
                DirectorFullName = "Дулюк С.И.",
                DirectorPhoneNumber = "+375449876347",
                ChiefPowerEngineerFullName = "Маркевич В.В.",
                ChiefPowerEngineerPhoneNumber = "+375445463978"
            };

            var typeOfProduct = new TypeOfProduct
            {
                Name = "кирпичи",
                Unit = "шт."
            };

            dbContext.Organizations.Add(organization);
            dbContext.TypeOfProducts.Add(typeOfProduct);
            dbContext.SaveChanges();
        }

        static void Update(Context dbContext)
        {
            var oldOrganizationName = "ОАО ДУЛЮК И КИРПИЧИ";

            var updateOrganization = dbContext.Organizations.FirstOrDefault(organization => organization.Name == oldOrganizationName);

            if (updateOrganization != null)
            {
                updateOrganization.Name = "ОАО СТЕПАН И КИРПИЧИ";
            }

            dbContext.SaveChanges();
            Console.WriteLine("Обновление данных произведено успешно");

        }

        static void Delete(Context dbContext)
        {
            var organizationToRemove = "ООО Стройтех";
            int? id = null;
            foreach (var dbContextOrganization in dbContext.Organizations)
            {
                if (dbContextOrganization.Name == organizationToRemove)
                {
                    id = dbContextOrganization.OrganizationID;
                }
            }

            if (id != null)
            {
                var type = dbContext.TypeOfProducts.Where(product => product.OrganizationID == id);
                var manufactured = dbContext.ManufacturedProducts.Where(product => product.ManufacturedProductID == id);
                var heatCons = dbContext.HeatConsumptions.Where(consumption => consumption.HeatConsumptionID == id);
                var heatConsRate = dbContext.HeatConsumptionRates.Where(rate => rate.HeatConsumptionRateID == id);

                var organizationTo =
                    dbContext.Organizations.Where(organization => organization.Name == organizationToRemove);

                dbContext.TypeOfProducts.RemoveRange(type);
                dbContext.ManufacturedProducts.RemoveRange(manufactured);
                dbContext.HeatConsumptions.RemoveRange(heatCons);
                dbContext.HeatConsumptionRates.RemoveRange(heatConsRate);
                dbContext.SaveChanges();

                dbContext.Organizations.RemoveRange(organizationTo);
                dbContext.SaveChanges();

                Console.WriteLine("Удаление данных произведено успешно");
            }
            else
            {
                Console.WriteLine("Не удалось удалить данные: Организации с таким именем не существует");
            }


        }


    }
}
