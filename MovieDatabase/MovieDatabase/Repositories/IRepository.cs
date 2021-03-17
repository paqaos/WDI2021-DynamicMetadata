using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Repositories
{
    public interface IRepository<TModel> where TModel : DatabaseItem, new()
    {
        IEnumerable<TModel> GetAll();

        TModel Add(TModel item);
        TModel Get(string id);
        void Update(TModel data);
    }
}
