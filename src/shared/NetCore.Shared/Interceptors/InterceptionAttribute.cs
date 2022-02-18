using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NetCore.Shared.Interceptors
{
    public class InterceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public InterceptionAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.StackTrace, null);
        }

        public override void OnException(ExceptionContext context)
        {
        }
    }
}
