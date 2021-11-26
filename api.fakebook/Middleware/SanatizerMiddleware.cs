using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.SecurityTokenService;

namespace api.fakebook.Middleware
{
    public class SanatizerMiddleware
    {

        private readonly RequestDelegate _next;

        public SanatizerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            context.Request.EnableBuffering();

            var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

            var rawBody = await reader.ReadToEndAsync();

            var sanatizer = new HtmlSanitizer();

            var sanatized = sanatizer.Sanitize(rawBody);

            if(rawBody != sanatized) throw new BadRequestException("XSS injection detected from middleware.");



            context.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next.Invoke(context);
        }





    }
}
