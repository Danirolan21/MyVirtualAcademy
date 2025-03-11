namespace MyVirtualAcademy.Helper
{
    public enum Folders { images, users, courses }
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment
            , IHttpContextAccessor httpContextAccessor)
        {
            this.hostEnvironment = hostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = folder switch
            {
                Folders.images => "assets/images",
                Folders.users => "assets/images/users",
                Folders.courses => "assets/images/courses",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };

            string fullPath = Path.Combine(this.hostEnvironment.WebRootPath, carpeta);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return Path.Combine(fullPath, fileName);
        }

        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = folder switch
            {
                Folders.images => "assets/images",
                Folders.users => "assets/images/users",
                Folders.courses => "assets/images/courses",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };
            var request = this.httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return Path.Combine(baseUrl, carpeta, fileName);
        }
    }
}
