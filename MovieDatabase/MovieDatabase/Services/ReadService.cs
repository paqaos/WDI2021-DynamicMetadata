using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Model;
using MovieDatabase.Repositories;

namespace MovieDatabase.Services
{
    public class ReadService<T> : IReadService<T> where T :  DatabaseItem, new()
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;

        public ReadService(IMapper mapper, IRepository<T> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public List<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public List<TTarget> GetAllAs<TTarget>()
        {
            return _mapper.Map<List<TTarget>>(GetAll());
        }

        public T GetById(string id)
        {
            return _repository.Get(id);
        }

        public TTarget GetById<TTarget>(string id)
        {
            return _mapper.Map<TTarget>(GetById(id));
        }
    }
}
