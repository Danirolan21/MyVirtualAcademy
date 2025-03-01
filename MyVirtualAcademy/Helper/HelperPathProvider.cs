namespace MyVirtualAcademy.Helper
{
    public enum Folders { images, users }
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
                Folders.images => "images",
                Folders.users => "images/users",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };

            return Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);
        }

        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = folder switch
            {
                Folders.images => "images",
                Folders.users => "images/users",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };
            var request = this.httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return Path.Combine(baseUrl, carpeta, fileName);
        }
    }
}
