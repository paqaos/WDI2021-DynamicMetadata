using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.Repositories
{
    public class InMemoryRepository<TModel> : IRepository<TModel> where TModel : DatabaseItem, new()
    {
        private List<TModel> _items { get; } = new List<TModel>();

        /// <inheritdoc />
        public IEnumerable<TModel> GetAll()
        {
            return _items.ToList();
        }

        /// <inheritdoc />
        public TModel Add(TModel item)
        {
            if (!_items.Contains(item))
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                }

                _items.Add(item);
            }

            return item;
        }

        /// <inheritdoc />
        public TModel Get(string id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        /// <inheritdoc />
        public void Update(TModel data)
        {
            var previousItem = Get(data.Id);
            _items.Remove(previousItem);

            _items.Add(data);
        }
    }
}
