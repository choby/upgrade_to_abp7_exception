using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetCasbin;
using Volo.Abp.Users;
using System.Net;
using System.Linq;
using System.Linq.Expressions;
using System;
using Evo.Scm.Permissions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Evo.Scm.Authorization
{
    public class CasbinAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CasbinAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            (string sub, string dom, string obj, string act) = this.GetRequestDefinition(context);
            //检查请求路径+方式是否在权限项中定义，如果没有定义则放行
            if (!string.IsNullOrEmpty(obj) && await this.PathIsDefinedInPermissions(context, $"/{obj}", act))
            {
                var e = context.RequestServices.GetRequiredService<Enforcer>();
                if (!e.Enforce(sub, dom, obj, act))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    //context.Response.ContentType = "context/textplain;utf-8";
                    await context.Response.WriteAsync("未授权");
                    return;
                }
            }
            await _next(context);
        }

        /// <summary>
        /// 获取casbin request_definition
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private (string sub, string dom, string obj, string act) GetRequestDefinition(HttpContext context)
        {
            var currentUser = context.RequestServices.GetRequiredService<ICurrentUser>();
            var sub = currentUser.Roles.Contains("admin") ? "admin" : currentUser.Id?.ToString();
            var dom = currentUser.TenantId?.ToString();
            
            var endpoint = context.GetEndpoint() as RouteEndpoint;
            var obj = endpoint?.RoutePattern.RawText;

            var act = context.Request.Method;
            return (sub, dom, obj, act);
        }

        /// <summary>
        /// 检查请求路径+方式是否在权限项中定义
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        private async Task<bool> PathIsDefinedInPermissions(HttpContext context, string obj, string act)
        {
            var repoPermission = context.RequestServices.GetRequiredService<INoTrackingRepository<Permission, Guid>>();
            var isdefined = (await repoPermission.GetListAsync(x => x.Path == obj && x.Method == act && x.IsEnabled == true)).Any();
            return isdefined;
        }
    }
}
