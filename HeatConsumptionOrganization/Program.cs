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
                Console.WriteLine("Введите количество записей для добавления");
                var numberOfRecords = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(DBInitializer.Initialize(dbContext, numberOfRecords));

                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectAllOrganizations(dbContext);

                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectOrganizationsByAddress(dbContext);

                Console.WriteLine("====== Будет выполнена группировка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                HeatConsumptionGroupBy(dbContext);

                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectOneToMany(dbContext);

                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectOneToManyWithFilter(dbContext);

                Console.WriteLine("====== Будет выполнена вставка данных в \"ОДИН\" (нажмите любую клавишу) ========");
                Console.ReadKey();
                InsertIntoOnes(dbContext);

                Console.WriteLine("====== Будет выполнена вставка данных в \"МНОГО\" (нажмите любую клавишу) ========");
                Console.ReadKey();
                InsertIntoManys(dbContext);

                Console.WriteLine("====== Будет выполнено удаление данных в \"ОДИН\" (нажмите любую клавишу) ========");
                Console.ReadKey();
                DeleteFromOnes(dbContext);

                Console.WriteLine("====== Будет выполнено удаление данных в \"МНОГО\" (нажмите любую клавишу) ========");
                Console.ReadKey();
                DeleteFromManys(dbContext);

                Console.WriteLine("====== Будет выполнено обновление данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                Update(dbContext);

                Console.ReadKey();
            }
        }

       
        static void Print(string sqlText, IEnumerable items)
        {
            Console.WriteLine(sqlText);

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

            Print("1. Список организаций: ", queryLinq.ToList());
        }

        static void SelectOrganizationsByAddress(Context dbContext)
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

            Print("2. Список организаций расположенных в Гомеле: ", queryLinq.ToList());
        }

        static void HeatConsumptionGroupBy(Context dContext)
        {
            Console.WriteLine("3. Группировка данных по адресу организации: \n");

            var groupingByAddress = from organization in dContext.Organizations
                group organization by organization.Address
                into grouping
                select new
                {
                    Address = grouping.Key,
                    Count = grouping.Count(),
                    Organizations = from organization in grouping select organization
                };

            foreach (var @group in groupingByAddress)
            {
                Console.WriteLine("Количество организаций расположенных в \"{0}\": {1}", @group.Address, @group.Count);
                int i = 0;
                foreach (var organization in @group.Organizations)
                {
                    i++;
                    Console.WriteLine(i + ". " + organization.Name);
                }

                Console.WriteLine();
            }
        }

        static void SelectOneToMany(Context dbContext)
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

            Print("4. Количество произведённой продукции организацией: ", queryLinq.ToList());
        }

        static void SelectOneToManyWithFilter(Context dbContext)
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

            Print("5. Организации отфильтрованные по частной собственности: ", queryLinq.ToList());
        }

        static void InsertIntoOnes(Context dbContext)
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

            Console.WriteLine("6. Была произведена вставка данных в таблину на стороне ОДИН\n");
        }

        static void InsertIntoManys(Context dbContext)
        {
            var rng = new Random();

            var idCount = (from organization in dbContext.Organizations select organization.OrganizationID).Last();

            var typeOfProduct = new TypeOfProduct
            {
                Name = "кирпичи",
                Unit = "шт.",
                OrganizationID = rng.Next(1, idCount)
            };

            dbContext.TypeOfProducts.Add(typeOfProduct);
            dbContext.SaveChanges();

            Console.WriteLine("7. Была произведена вставка данных в таблину на стороне МНОГО\n");
        }


        static void DeleteFromOnes(Context dbContext)
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

                Console.WriteLine("8. Удаление данных прошло успешно\n");
            }
            else
            {
                Console.WriteLine("8. Не удалось удалить данные: Организации с таким именем не существует\n");
            }


        }

        static void DeleteFromManys(Context dbContext)
        {
            var yearForManufacturedProduct = 2017;
            var year = dbContext.ManufacturedProducts.Where(manufacturedProduct =>
                manufacturedProduct.Year == yearForManufacturedProduct);

            dbContext.ManufacturedProducts.RemoveRange(year);
            dbContext.SaveChanges();

            Console.WriteLine("9. Удаление данных прошло успешно\n");
        }

        static void Update(Context dbContext)
        {
            var oldOrganizationName = "ООО ДУЛЮК И КИРПИЧИ";

            var updateOrganization = dbContext.Organizations.FirstOrDefault(organization => organization.Name == oldOrganizationName);

            if (updateOrganization != null)
            {
                updateOrganization.Name = "ООО СТЕПАН И КИРПИЧИ";

                Console.WriteLine("10. Обновление данных прошло успешно");
            }

            dbContext.SaveChanges();
        }
    }
}
