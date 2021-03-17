using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Dto;
using MovieDatabase.Dto.PropertyConfiguration;
using MovieDatabase.Model;
using MovieDatabase.Model.PropertyConfiguration;
using MovieDatabase.Repositories;

namespace MovieDatabase.Converters
{
    public class PropertyConverter
        : IValueResolver<DynamicMetadataProperty, DynamicPropertyDto, object>
    {
        /// <inheritdoc />
        public object Resolve(DynamicMetadataProperty source, DynamicPropertyDto destination, object destMember,
                              ResolutionContext context)
        {
            switch (source.Type)
            {
                case "numeric":
                {
                    var sourceItem = (int) source.Value;
                    destination.Value = sourceItem;

                    return destination.Value;
                }

                case "text":
                {
                    var sourceItem = (string)source.Value;
                    destination.Value = sourceItem;

                    return destination.Value;
                }

                case "person":
                {
                    var sourceProperty = (Person) source.Value;
                    PersonPropertyDto sourceItem = null;
                    if (sourceProperty != null)
                    {
                        sourceItem = new PersonPropertyDto
                        {
                            FirstName = sourceProperty.FirstName,
                            LastName = sourceProperty.LastName,
                            Id = sourceProperty.Id
                        };
                    }
                   
                    destination.Value = sourceItem;

                    return destination.Value;
                    }
            }

            return null;
        }
    }
}
