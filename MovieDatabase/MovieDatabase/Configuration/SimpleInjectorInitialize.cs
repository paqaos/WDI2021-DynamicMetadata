using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.CommandStack.CommandHandlers;
using MovieDatabase.CommandStack.CommandHandlers.Movies;
using MovieDatabase.CommandStack.EventHandlers;
using MovieDatabase.CommandStack.EventHandlers.MetadataDefinitions;
using MovieDatabase.CommandStack.Queries.MovieMetadata;
using MovieDatabase.CommandStack.QueryHandlers;
using MovieDatabase.CommandStack.QueryHandlers.MovieMetadata;
using MovieDatabase.Constants;
using MovieDatabase.DataBuilder;
using MovieDatabase.Mappers;
using MovieDatabase.Model;
using MovieDatabase.Model.EventSourcing;
using MovieDatabase.Model.PropertyConfiguration;
using MovieDatabase.Operations;
using MovieDatabase.Operations.Description;
using MovieDatabase.Operations.Metadata;
using MovieDatabase.Operations.Movies;
using MovieDatabase.Repositories;
using MovieDatabase.Services;
using MovieDatabase.Services.EventProcessing;
using MovieDatabase.Services.EventSourcing;
using MovieDatabase.Services.MetadataObject;
using MovieDatabase.Services.Validation;
using MovieDatabase.Services.Validation.PropertyValidators.General;
using MovieDatabase.Services.Validation.PropertyValidators.Numeric;
using MovieDatabase.Services.Validation.PropertyValidators.Person;
using SimpleInjector;

namespace MovieDatabase.Configuration
{
    public static class SimpleInjectorInitialize
    {
        public static void AddServices(Container container)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieProfile>();
                cfg.AddProfile<PersonProfile>();
                cfg.AddProfile<MetadataProfile>();
                cfg.AddProfile<ErrorProfile>();
                cfg.AddProfile<MetadataDefinitionProfile>();
                cfg.AddProfile<ValidatorProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();

            var mapper = new Mapper(mapperConfig);
            container.RegisterInstance<IMapper>(mapper);
            container.Register(typeof(ICommandHandler<,>), typeof(CreateMovieCommandHandler).Assembly);
            container.Register(typeof(IQueryHandler<,>), typeof(GetMovieMetadataQueryHandler).Assembly);

            container.Collection.Register(typeof(IEventHandler<>), typeof(PropertyAddedToMetadataDefinitionEventHandler).Assembly);

            container.RegisterSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
            container.Register(typeof(IReadService<>), typeof(ReadService<>));
            container.Register(typeof(IWriteService<>), typeof(WriteService<>));
            container.Register<IInputDataParser, InputDataParser>();

            container.Collection.Register(typeof(IValidator), typeof(NumericRangePropertyValidator).Assembly);

            container.Register<NumericRangePropertyValidator>();
            container.Register<SingleTypeItemValidator>();
            container.Register<NotNullValidator>();
            container.Register<ExistingPersonValidator>();

            container.Register<IValidationFactory, ValidationFactory>();

            container.RegisterSingleton<IEventDispatcher, EventDispatcher>();

            container.Register<IMovieBuilder, MovieBuilder>();
            container.Register<ICommitService, CommitService>();


            container.RegisterInitializer<IRepository<Movie>>(x =>
            {
                x.Add(new Movie
                {
                    Genres = new List<MovieGenre>{ MovieGenre.Comedy },
                    Name = "Anioł w Krakowie",
                    Id = "movie_1",
                    Metadata = new List<DynamicMetadata>
                    {
                        new DynamicMetadata
                        {
                            Id = "movie_1_recording_location_1",
                            DefinitionId = "recording_location",
                            Properties = new List<DynamicMetadataProperty>
                            {
                                new NumericProperty
                                {
                                    Id = "year",
                                    WrappedValue = 2001,
                                    HasDefaultValue =  false,
                                    State = PropertyState.Active
                                },
                                new StringProperty
                                {
                                    Id = "scene name",
                                    WrappedValue = "Kraków",
                                    HasDefaultValue =  false,
                                    State = PropertyState.Active
                                }
                            },
                            IsValid = true
                        },
                        new DynamicMetadata
                        {
                            Id = "movie_1_actor_1",
                            DefinitionId = "actor",
                            Properties = new List<DynamicMetadataProperty>
                            {
                                new PersonProperty
                                {
                                    Id = "person",
                                    WrappedValue = new Person
                                    {
                                        Id = "actor_1",
                                        FirstName = "Krzysztof",
                                        LastName = "Globisz"
                                    },
                                    HasDefaultValue =  false,
                                    State = PropertyState.Active
                                }
                            },
                            IsValid = true
                        },
                        new DynamicMetadata
                        {
                            Id = "movie_1_actor_2",
                            DefinitionId = "actor",
                            Properties = new List<DynamicMetadataProperty>
                            {
                                new PersonProperty
                                {
                                    Id = "person",
                                    WrappedValue = null,
                                    HasDefaultValue =  false,
                                    State = PropertyState.Active
                                }
                            },
                            IsValid = true
                        }
                    },
                    IsValid = true
                });
            });

            container.RegisterInitializer<IRepository<DynamicMetadataDefinition>>(x =>
            {
                x.Add(new DefinitionBuilder("recording_location", "Location of recording")
                      .AddValidator(validatorCfg => validatorCfg
                                                    .UseKey("numeric-range")
                                                    .UseValidatorConfiguration("property-key", "year")
                                                    .UseValidatorConfiguration("max", "2021"))
                      .AddProperty<NumberPropertyBuilder>("year", propertyCfg => propertyCfg.DefaultValue(2020))
                      .AddProperty<StringPropertyBuilder>("scene name",
                          propertyCfg => propertyCfg.DefaultValue("Cracow Old City"))
                      .Build());

                x.Add(new DefinitionBuilder("director", "Director")
                      .AddProperty<PersonPropertyBuilder>("person", propertyCfg => propertyCfg.DefaultValue(null))
                      .AddValidator(validatorCfg => validatorCfg.UseKey("single-type-item"))
                      .AddValidator(validatorCfg => validatorCfg.UseKey("not-null").UseValidatorConfiguration("property-key", "person"))
                      .AddValidator(validatorCfg => validatorCfg.UseKey("existing-person").UseValidatorConfiguration("property-key", "person"))
                      .Build());

                x.Add(new DefinitionBuilder("actor", "actor")
                      .AddProperty<PersonPropertyBuilder>("person", propertyCfg => propertyCfg.DefaultValue(null))
                      .Build());
            });


            //container.RegisterInitializer<IRepository<DynamicMetadataDefinition>>(x =>
            //{
            //    x.Add(new DynamicMetadataDefinition
            //    {
            //        Id = "recording_location",
            //        Name = "Location of recording",
            //        Properties = new List<DynamicMetadataProperty>
            //        {
            //            new NumericProperty
            //            {
            //                Id = "year",
            //                WrappedValue = 2020
            //            },
            //            new StringProperty
            //            {
            //                Id = "scene name",
            //                WrappedValue = "Cracow Old city"
            //            }
            //        },
            //        Validators = new List<ValidatorConfiguration>
            //        {
            //            new ValidatorConfiguration
            //            {
            //                ValidatorId = "validator_1",
            //                ValidatorKey = "numeric-range",
            //                Parameters = new Dictionary<string, string>
            //                {
            //                    { "property-key", "year"},
            //                    { "max", "2021" }
            //                }
            //            }
            //        }
            //    });
            //    x.Add(new DynamicMetadataDefinition
            //    {
            //        Id = "director",
            //        Name = "Director",
            //        Properties = new List<DynamicMetadataProperty>
            //        {
            //            new PersonProperty
            //            {
            //                Id = "person"
            //            }
            //        },
            //        Validators = new List<ValidatorConfiguration>
            //        {
            //            new ValidatorConfiguration
            //            {
            //                ValidatorId = "validator_2",
            //                ValidatorKey = "single-type-item",
            //                Parameters = new Dictionary<string, string>()
            //            },
            //            new ValidatorConfiguration
            //            {
            //                ValidatorId = "validator_3",
            //                ValidatorKey = "not-null",
            //                Parameters = new Dictionary<string, string>
            //                {
            //                    {"property-key", "person" }
            //                }
            //            }, 
            //            new ValidatorConfiguration
            //            {
            //                ValidatorId = "validator_4",
            //                ValidatorKey = "existing-person",
            //                Parameters = new Dictionary<string, string>
            //                {
            //                    {"property-key", "person" }
            //                }
            //            }
            //        }
            //    });
            //    x.Add(new DynamicMetadataDefinition
            //    {
            //        Id = "actor",
            //        Name = "actor",
            //        Properties = new List<DynamicMetadataProperty>
            //        {
            //            new PersonProperty
            //            {
            //                Id = "person",
            //                WrappedValue = default
            //            }
            //        }
            //    });
            //});

            container.RegisterInitializer<IRepository<Person>>(x =>
            {
                x.Add(new Person
                {
                    Id = "director_1",
                    FirstName = "Jan",
                    LastName = "Kowalski"
                });
            });


            container.RegisterInitializer<IRepository<Commit>>(x =>
            {
                x.Add(new Commit
                {
                    MovieId = "movie_1",
                    CommitOrder = 0,
                    Operations = new List<IMovieOperation>
                    {
                        new CreateMovieOperation
                        {
                            OperationOrder = 0,
                            MovieId = "movie_1"
                        }
                    }
                });

                x.Add(new Commit
                {
                    MovieId = "movie_1",
                    CommitOrder = 1,
                    Operations = new List<IMovieOperation>
                    {
                        new UpdateDescriptionAndGenresOperation
                        {
                            MovieName = "C# Dynamic metadata",
                            OperationOrder = 0,
                            Genres = new List<MovieGenre>
                            {
                                MovieGenre.Comedy
                            }
                        },
                    }
                });

                x.Add(new Commit
                {
                    MovieId = "movie_1",
                    CommitOrder = 2,
                    Operations = new List<IMovieOperation>
                    {
                        new AddMetadataOperation
                        {
                            MetadataContent = new DynamicMetadata
                            {
                                DefinitionId = "definition_1",
                                Properties = new List<DynamicMetadataProperty>()
                            },
                            OperationOrder = 0,
                        },
                        new AddMetadataOperation
                        {
                            MetadataContent = new DynamicMetadata
                            {
                                DefinitionId = "definition_2",
                                Properties = new List<DynamicMetadataProperty>()
                            },
                            OperationOrder = 1,
                        },
                    }
                });

                x.Add(new Commit
                {
                    MovieId = "movie_2",
                    CommitOrder = 0,
                    Operations = new List<IMovieOperation>
                    {
                        new CreateMovieOperation
                        {
                            OperationOrder = 0,
                            MovieId = "movie_2"
                        }
                    }
                });
            });
        }
    }
}
