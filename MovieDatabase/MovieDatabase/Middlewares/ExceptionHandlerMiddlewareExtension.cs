using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieDatabase.Dto;
using MovieDatabase.Exceptions;
using Newtonsoft.Json;
using SimpleInjector;

namespace MovieDatabase.Middlewares
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app, Container container)
        {
            ILogger<ExceptionHandlerMiddleware> logger = container.GetInstance<ILogger<ExceptionHandlerMiddleware>>();
            IMapper mapper = container.GetInstance<IMapper>();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = GetHttpCode(contextFeature.Error);

                        logger.LogError(contextFeature.Error.Message);

                        var error = new ErrorDto
                        {
                            Message = contextFeature.Error.Message,
                            Code = GetErrorCode(contextFeature)
                        };

                        if (contextFeature.Error is AggregatedValidationException aggregatedValidationException)
                        {
                            foreach (var validationError in aggregatedValidationException.ValidationErrors)
                            {
                                var errorDetails = mapper.Map<ErrorDto>(validationError);
                                error.InternalErrors.Add(errorDetails);
                            }
                        }

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                    }
                });
            });

            return app;
        }

        private static int GetErrorCode(IExceptionHandlerFeature contextFeature)
        {
            int code = 0;

            if (contextFeature.Error is IErrorCodeException errorCodeException)
                code = errorCodeException.ErrorCode;
            else if (contextFeature.Error != null && contextFeature.Error.InnerException is IErrorCodeException innerException)
                code = innerException.ErrorCode;

            return code;
        }

        private static int GetHttpCode(Exception exception)
        {
            switch (exception)
            {
                case AggregatedValidationException _:
                    return (int)HttpStatusCode.BadRequest;
                case NotImplementedException _:
                    return (int)HttpStatusCode.NotImplemented;
                case NotFoundException _:
                    return (int)HttpStatusCode.NotFound;
                case BadRequestException _:
                    return (int) HttpStatusCode.BadRequest;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
