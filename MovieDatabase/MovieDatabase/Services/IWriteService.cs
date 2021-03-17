using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services
{
    public interface IWriteService<T>  where T : DatabaseItem
    {
        T CreateItem(T data, CancellationToken ct);
        void UpdateItem(T data, CancellationToken ct);
    }
}
