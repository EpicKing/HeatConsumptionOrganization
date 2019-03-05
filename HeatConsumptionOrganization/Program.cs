using System;
using System.Collections;
using System.Linq;
using HeatConsumptionOrganization.Models;
using Remotion.Linq.Clauses;

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

        static void Update(Context dbContext)
        {

        }

        static void Delete(Context dbContext)
        {

        }


    }
}
