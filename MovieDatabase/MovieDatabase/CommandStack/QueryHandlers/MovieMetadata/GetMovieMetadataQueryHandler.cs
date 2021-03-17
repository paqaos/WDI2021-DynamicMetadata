using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.Queries.MovieMetadata;
using MovieDatabase.Dto;
using MovieDatabase.Model;
using MovieDatabase.Services;

namespace MovieDatabase.CommandStack.QueryHandlers.MovieMetadata
{
    public class GetMovieMetadataQueryHandler : IQueryHandler<GetMovieMetadataQuery, List<DynamicMetadataDto>>
    {
        private readonly IReadService<Movie> _movieReadService;
        private readonly IMapper _mapper;

        public GetMovieMetadataQueryHandler(IReadService<Movie> movieReadService, IMapper mapper)
        {
            _movieReadService = movieReadService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<DynamicMetadataDto>> Execute(GetMovieMetadataQuery query)
        {
            var movie = _movieReadService.GetById(query.MovieId);

            if (movie == null)
            {
                return new List<DynamicMetadataDto>();
            }

            return _mapper.Map<List<DynamicMetadataDto>>(movie.Metadata);
        }
    }
}
