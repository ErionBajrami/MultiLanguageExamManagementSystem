using System.Globalization;

namespace MultiLanguageExamManagementSystem.Helpers
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Your code here

            // read the accept language header and set the current culture based on it 
            var cultureQuery = context.Request.Headers["Accept-Language"].ToString();

            var culture = !string.IsNullOrWhiteSpace(cultureQuery)
                ? new CultureInfo(cultureQuery)
                : CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }

}
