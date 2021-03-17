using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.Model;
using MovieDatabase.Repositories;

namespace MovieDatabase.Services
{
    public class WriteService<T> : IWriteService<T> where T : DatabaseItem, new()
    {
        private readonly IRepository<T> _repository;

        public WriteService(IRepository<T> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public T CreateItem(T data, CancellationToken ct)
        {
            var item = _repository.Add(data);

            return item;
        }

        /// <inheritdoc />
        public void UpdateItem(T data, CancellationToken ct)
        {
            _repository.Update(data);
        }
    }
}
