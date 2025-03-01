namespace MyVirtualAcademy.Helper
{
    public static class SessionHelper
    {
        public static int? GetUserId(HttpContext context)
        {
            return context.Session.GetInt32("IdUsuario");
        }

        public static void Logout(HttpContext context)
        {
            context.Session.Clear();
        }
    }
}
