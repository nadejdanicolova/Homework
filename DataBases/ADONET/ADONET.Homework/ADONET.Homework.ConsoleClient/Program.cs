﻿using System.Reflection;

using Ninject;

using ADONET.Homework.Logic.Models;
using ADONET.Homework.Logic.QueryEngines.Contract;
using ADONET.Homework.Logic.CommandProviders.Contracts;

namespace ADONET.Homework.ConsoleClient
{
    public class Program
    {
        public static void Main()
        {
            var ninject = new StandardKernel();
            ninject.Load(Assembly.GetExecutingAssembly());

            var commandProvider = ninject.Get<ICommandProvider>();
            var queryEngine = ninject.Get<IQueryEngine>();

            DisplayAllCategories(commandProvider, queryEngine);
            DisplayEachProductWithCategory(commandProvider, queryEngine);
        }

        private static void DisplayAllCategories(ICommandProvider commandProvider, IQueryEngine queryEngine)
        {
            var sql = "SELECT * FROM Categories";
            var command = commandProvider.CreateCommand(sql);
            var result = queryEngine.ExecuteReaderCommand<Category>(command);
        }

        private static void DisplayEachProductWithCategory(ICommandProvider commandProvider, IQueryEngine queryEngine)
        {
            var sql = "SELECT p.ProductName, c.CategoryName FROM Products p JOIN Categories c ON p.CategoryID = c.CategoryID";
            var command = commandProvider.CreateCommand(sql);
            var result = queryEngine.ExecuteReaderCommand<ProductWIthCategory>(command);
        }
    }
}
