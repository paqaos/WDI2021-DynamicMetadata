using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.Queries;

namespace MovieDatabase.CommandStack.QueryHandlers
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Execute(TQuery query);
    }
}
