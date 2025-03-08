namespace MyVirtualAcademy.Helper
{
    public static class SessionHelper
    {
        public static int? GetUserId(HttpContext context)
        {
            return context.Session.GetInt32("IdUsuario");
        }

        public static string? GetUserRole(HttpContext context)
        {
            return context.Session.GetString("Rol");
        }

        public static void Logout(HttpContext context)
        {
            context.Session.Clear();
        }
    }
}
