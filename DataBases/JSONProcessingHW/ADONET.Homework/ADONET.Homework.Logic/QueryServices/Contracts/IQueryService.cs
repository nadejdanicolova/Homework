﻿using System.Data;

namespace ADONET.Homework.Logic.QueryServices.Contracts
{
    public interface IQueryService
    {
        IDataReader ExecuteReaderQuery(IDbCommand command);
    }
}
