using Evo.Scm.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evo.Scm
{
    public class ApiResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                ResultMessage<object> result = new ResultMessage<object>();
                result.Code = ExceptionCodes.正常;
                result.Data = objectResult.Value;
                
                context.Result = new JsonResult(result);
            }

            base.OnResultExecuting(context);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
