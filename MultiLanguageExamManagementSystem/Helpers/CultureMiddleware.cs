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
            var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
            var culture = DeterminePreferredCulture(acceptLanguageHeader);
            CultureInfo.CurrentCulture = culture;
            await _next(context);
        }

        private CultureInfo DeterminePreferredCulture(string acceptLanguageHeader)
        {
            if (!string.IsNullOrEmpty(acceptLanguageHeader))
            {
                var cultures = acceptLanguageHeader.Split(',');
                foreach (var culture in cultures)
                {
                    if (CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.Name == culture))
                    {
                        return new CultureInfo(culture);
                    }
                }
            }
            return new CultureInfo("en-US");
        }
    }

}
