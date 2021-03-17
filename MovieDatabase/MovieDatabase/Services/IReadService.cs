using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Services
{
    public interface IReadService<T> where T : DatabaseItem
    {
        List<T> GetAll();

        T GetById(string id);

        List<TTarget> GetAllAs<TTarget>();

        TTarget GetById <TTarget>(string id);
    }
}
