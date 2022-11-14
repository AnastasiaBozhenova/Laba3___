using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Lab3_.Middleware
{
    public class InfoMiddleware
    {
        private readonly RequestDelegate _next;

        public InfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path == "/info")
            {
                var str = "<a href=\"/\">back</a>";

                var remoteIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var isHaveUserAgent = httpContext.Request.Headers.TryGetValue("user-agent", out var userAgent);
                str += "</br>user agent: " + 
                    (isHaveUserAgent
                        ? userAgent
                        : "no");

                var isHaveReffer = httpContext.Request.Headers.TryGetValue("reffer", out var reffer);
                str += "</br>reffer: " +
                    (isHaveReffer
                        ? reffer
                        : "no");

                str += "</br>" + "Client ip: " + remoteIpAddress;

                await httpContext.Response.WriteAsync(str);
            }

            await _next(httpContext);
        }
    }
}