using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Dto.MetadataDefinitions;
using MovieDatabase.Model;
using MovieDatabase.Repositories;
using MovieDatabase.Services;

namespace MovieDatabase.Controllers
{
    [Route("metadata-definitions")]
    public class MetadataDefinitionsController : Controller
    {
        private readonly IReadService<DynamicMetadataDefinition> _dynamicMetedataDefinitionReadService;

        public MetadataDefinitionsController(IReadService<DynamicMetadataDefinition> dynamicMetedataDefinitionReadService)
        {
            _dynamicMetedataDefinitionReadService = dynamicMetedataDefinitionReadService;
        }

        [HttpGet, Route("")]
        public List<MetadataDefinitionDto> GetAllDynamicMetadataDefinitions()
        {
            return _dynamicMetedataDefinitionReadService.GetAllAs<MetadataDefinitionDto>();
        }
    }
}
