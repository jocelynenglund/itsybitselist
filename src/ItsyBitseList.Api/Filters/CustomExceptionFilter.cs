using Microsoft.AspNetCore.Mvc.Filters;

namespace ItsyBitseList.Api.Filters
{
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                context.HttpContext.Response.StatusCode  = (int)System.Net.HttpStatusCode.InternalServerError;
                context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(context.Exception.Message);
                context.ExceptionHandled = true;
            }
        }
    }
}
