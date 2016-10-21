﻿using Ninject;
using Ninject.Modules;

using ADONET.Homework.Logic.CommandProviders.Contracts;
using ADONET.Homework.Logic.CommandProviders;
using ADONET.Homework.Logic.ConnectionProviders.Contracts;
using ADONET.Homework.Logic.ConnectionProviders;
using ADONET.Homework.Logic.DataHandlers.Contracts;
using ADONET.Homework.Logic.DataHandlers;
using ADONET.Homework.Logic.QueryEngines.Contract;
using ADONET.Homework.Logic.QueryEngines;
using ADONET.Homework.Logic.QueryServices.Contracts;
using ADONET.Homework.Logic.QueryServices;

namespace ADONET.Homework.ConsoleClient.Bindings
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IConnectionProvider>().To<DefaultSqlServerConnectionProvider>();
            this.Bind<ICommandProvider>().To<SqlCommandProvider>();
            this.Bind<IDataHandler>().To<DataHandler>();
            this.Bind<IQueryService>().To<QueryService>();

            this.Bind<IQueryEngine>().To<SimpleQueryEngine>()
                .WithConstructorArgument("connectionProvider", ctx => ctx.Kernel.Get<IConnectionProvider>())
                .WithConstructorArgument("queryService", ctx => ctx.Kernel.Get<IQueryService>())
                .WithConstructorArgument("dataHandler", ctx => ctx.Kernel.Get<IDataHandler>());
        }
    }
}
