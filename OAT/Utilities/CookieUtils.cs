namespace OAT.Utilities
{
    public static class CookieUtils
    {
        public static void SetCookie(this HttpContext context, string key, string value)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddDays(1);
            context.Response.Cookies.Append(key, value, cookie);
        }
        public static void SetCookie(this HttpContext context, string key, string value, int days)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddDays(days);
            context.Response.Cookies.Append(key, value, cookie);
        }
        public static string? GetCookie(this HttpContext context, string key) => 
            context.Request.Cookies[key];

        public static void DeleteCookie(this HttpContext context, string key) => 
            context.Response.Cookies.Delete(key);
    }
}
