using System;
using System.Collections;
using System.Collections.Generic;
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
                SelectAllOrganizations(dbContext);
            }

            Console.WriteLine("Hello World!");
        }

       
        static void Print(string sqlText, IEnumerable items)
        {
            // Heading
            var attributes = new string[0];

            // Stores the length of the longest words
            var longestWords = new int[0];

            // Create array for body
            var numberOfQueries = 0;
            var tuples = new string[numberOfQueries][];

            // Flag to fill the heading
            var isFirstTry = true;

            foreach (var item in items)
            {
                var allWordsList = new List<string>();

                numberOfQueries++;
                Array.Resize(ref tuples, numberOfQueries);

                // get Key-Value
                var cleanItem = item.ToString().Remove(0, 1);
                cleanItem = cleanItem.Remove(cleanItem.Length - 1);
                var keyValueStrings = cleanItem.Split(',');

                foreach (var keyValue in keyValueStrings)
                {
                    var keyValueSplit = keyValue.Split("=");
                    allWordsList.AddRange(keyValueSplit);
                }

                if (isFirstTry)
                {
                    var columnList = new List<string>();
                    isFirstTry = false;
                    for (var i = 0; i < allWordsList.ToArray().Length; i += 2)
                    {
                        columnList.Add(allWordsList[i]);
                        attributes = columnList.ToArray();
                        Array.Resize(ref longestWords, attributes.Length);
                        for (var j = 0; j < attributes.Length; j++)
                        {
                            longestWords[j] = attributes[j].Length;
                        }
                    }
                }

                var rowList = new List<string>();

                for (var i = 1; i < allWordsList.ToArray().Length; i += 2)
                {
                    rowList.Add(allWordsList[i]);
                }

                for (var i = 0; i < rowList.ToArray().Length; i++)
                {
                    if (rowList[i].Length > longestWords[i])
                    {
                        longestWords[i] = rowList[i].Length;
                    }
                }

                tuples[numberOfQueries - 1] = new string[rowList.ToArray().Length];

                for (var n = 0; n < rowList.ToArray().Length; n++)
                {
                    tuples[numberOfQueries - 1][n] = rowList[n];
                }
            }

            // Create frame for table
            var frame = "";

            for (var i = 0; i < attributes.Length; i++)
            {
                frame += "+";
                for (var j = 0; j < attributes[i].PadRight(longestWords[i] + 2).Length; j++)
                {
                    frame += "-";
                }
            }

            frame += "+";

            Console.WriteLine(frame);

            // Draw attributes
            for (var h = 0; h < attributes.Length; h++)
            {
                Console.Write("| " + attributes[h].PadRight(longestWords[h]) + " ");
            }
            Console.Write("|");

            Console.WriteLine();
            Console.WriteLine(frame);
            
            // Draw tuples
            for (var k = 0; k < tuples.Length; k++)
            {
                for (var j = 0; j < tuples[k].Length; j++)
                {
                    Console.Write("| " + tuples[k][j].PadRight(longestWords[j]) + " ");
                }
                Console.Write("|");
                Console.WriteLine();
                Console.WriteLine(frame);
            }
        }

        static void SelectAllOrganizations(Context dbContext)
        {
            var queryLinq = from organization in dbContext.Organizations
              //  join typeOfProduct in dbContext.TypeOfProducts 
               // on organization.OrganizationID equals typeOfProduct.OrganizationID
               // where organization.Address == "Гомель"
               // orderby organization.DirectorFullName descending
                select new
                {
                    Id = organization.OrganizationID,
                    Название = organization.Name,
                    Форма_собственности = organization.TypeOfOwnership,
                    Адрес = organization.Address,
                    ФИО_руководителя = organization.DirectorFullName,
                    Телефон_руководителя = organization.DirectorPhoneNumber,
                    ФИО_главного_энергетика = organization.ChiefPowerEngineerFullName,
                    Телефон_главного_энергетика = organization.ChiefPowerEngineerPhoneNumber,
                  //  Вид_продукции = typeOfProduct.Name,
                   // Единица_измерения = typeOfProduct.Unit

                };

            var comment = "Результат выполнения запроса на ";
            Print(comment, queryLinq.ToList());
        }

        static void Second(Context dbContext)
        {
            var queryLinq = from organization in dbContext.Organizations
                where organization.Address == "Гомель"
                select new
                {
                    Id = organization.OrganizationID,
                    Название = organization.Name,
                    Форма_собственности = organization.TypeOfOwnership,
                    Адрес = organization.Address,
                    ФИО_руководителя = organization.DirectorFullName,
                    Телефон_руководителя = organization.DirectorPhoneNumber,
                    ФИО_главного_энергетика = organization.ChiefPowerEngineerFullName,
                    Телефон_главного_энергетика = organization.ChiefPowerEngineerPhoneNumber,
                };

            var comment = "Результат выполнения запроса на ";
            Print(comment, queryLinq.ToList());



            //var queryLinq = from manufacturedProduct in dbContext.ManufacturedProducts
            //    where manufacturedProduct.Year < 2017 && manufacturedProduct.Quarter == 3
            //    orderby manufacturedProduct.TotalProduced descending
            //    select new
            //    {
            //        manufacturedProduct.TotalProduced,
            //        manufacturedProduct.Year,
            //        manufacturedProduct.Quarter,
            //        manufacturedProduct.OrganizationID
            //    };

        }

        static void Third(Context dContext)
        {
            var queryLinq = from heatConsumption in dContext.HeatConsumptions
                group heatConsumption by heatConsumption.TotalConsumed
                into grouping
                select new
                {
                    TotalConsumed = grouping.Key,
                    Average = grouping.Count(),
                    Years = from heatConsumption in grouping select heatConsumption
                };

            foreach (var VARIABLE in queryLinq)
            {
                
            }
        }

        static void Fourth(Context dbContext)
        {
            var queryLinq = from organization in dbContext.Organizations
                join manufacturedProduct in dbContext.ManufacturedProducts 
                    on organization.OrganizationID equals manufacturedProduct.OrganizationID
                select new
                {
                    Id = organization.OrganizationID,
                    Название = organization.Name,
                    Форма_собственности = organization.TypeOfOwnership,
                    Год = manufacturedProduct.Year,
                    Всего_произведено = manufacturedProduct.TotalProduced
                };

            var comment = "Результат выполнения запроса на ";
            Print(comment, queryLinq.ToList());
        }

        static void Fifth(Context dbContext)
        {
            var queryLinq = from organization in dbContext.Organizations
                join manufacturedProduct in dbContext.ManufacturedProducts 
                    on organization.OrganizationID equals manufacturedProduct.OrganizationID
                where organization.TypeOfOwnership == "частная собственность"
                select new
                {
                    Id = organization.OrganizationID,
                    Название = organization.Name,
                    Форма_собственности = organization.TypeOfOwnership,
                    Год = manufacturedProduct.Year,
                    Всего_произведено = manufacturedProduct.TotalProduced
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


            dbContext.Organizations.Add(organization);
            dbContext.SaveChanges();
        }

        static void Seventh(Context dbContext)
        {
            var typeOfProduct = new TypeOfProduct
            {
                Name = "кирпичи",
                Unit = "шт."
            };

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
