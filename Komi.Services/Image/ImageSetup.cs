using Microsoft.Extensions.DependencyInjection;

namespace Komi.Services.Image
{
    public static class ImageSetup
    {
        public static IServiceCollection AddImages(this IServiceCollection services) =>
            services.AddScoped<IImageService, ImageService>();
    }
}