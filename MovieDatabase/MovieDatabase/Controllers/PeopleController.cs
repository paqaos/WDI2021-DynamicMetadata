using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto;
using MovieDatabase.Model;
using MovieDatabase.Repositories;
using MovieDatabase.Services;

namespace MovieDatabase.Controllers
{
    [Route("people")]
    public class PeopleController : Controller
    {
        private readonly IReadService<Person> _personReadService;
        public PeopleController(IReadService<Person> personReadService)
        {
            _personReadService = personReadService;
        }

        [HttpGet, Route("")]
        public List<PersonDto> GetAllPeople()
        {
            return _personReadService.GetAllAs<PersonDto>();
        }
    }
}
