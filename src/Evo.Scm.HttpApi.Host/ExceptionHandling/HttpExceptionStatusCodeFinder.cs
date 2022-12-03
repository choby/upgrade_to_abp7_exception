using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace Evo.Scm.ExceptionHandling;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IHttpExceptionStatusCodeFinder))]
public class HttpExceptionStatusCodeFinder : IHttpExceptionStatusCodeFinder, ITransientDependency
{
    protected AbpExceptionHttpStatusCodeOptions Options { get; }

    public HttpExceptionStatusCodeFinder(
        IOptions<AbpExceptionHttpStatusCodeOptions> options)
    {
        Options = options.Value;
    }

    public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
            exceptionWithHttpStatusCode.HttpStatusCode > 0)
        {
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
        }

        if (exception is IHasErrorCode exceptionWithErrorCode &&
            !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
        {
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
            {
                return status;
            }
        }

        if (exception is AbpAuthorizationException)
        {
            return httpContext.User.Identity.IsAuthenticated
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }

        //TODO: Handle SecurityException..?

        if (exception is AbpValidationException)
        {
            return HttpStatusCode.OK;
        }
        //实体未找到这个异常比较特殊，abp做了单独处理，在ExceptionToErrorInfoConverter中也做了一层转换
        if (exception is EntityNotFoundException)
        {
            return HttpStatusCode.OK;
        }

        if (exception is AbpDbConcurrencyException)
        {
            return HttpStatusCode.Conflict;
        }

        if (exception is NotImplementedException)
        {
            return HttpStatusCode.NotImplemented;
        }
        //普通的业务异常（非代码崩掉了），由开发人员在处理业务的时候丢出来， 表示业务受限无法继续，这种异常统一当作服务器正常响应
        if (exception is IBusinessException)
        {
            return HttpStatusCode.OK;
        }
        //这个异常才表示代码崩了， 服务器挂了etc.
        return HttpStatusCode.InternalServerError;
    }
}
