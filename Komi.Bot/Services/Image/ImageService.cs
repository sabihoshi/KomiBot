using System;
using System.Net.Http;
using System.Threading.Tasks;
using ColorThiefDotNet;
using Komi.Bot.Services.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Color = Discord.Color;

namespace Komi.Bot.Services.Image
{
    /// <summary>
    ///     Desribes a service that performs actions related to images.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        ///     Identifies a dominant color from the image at the supplied location.
        /// </summary>
        /// <param name="location">The location of the image.</param>
        /// <returns>
        ///     A <see cref="ValueTask" /> that will complete when the operation completes,
        ///     containing a dominant color in the image.
        /// </returns>
        ValueTask<Color> GetDominantColorAsync(Uri location);

        /// <summary>
        ///     Identifies a dominant color from the provided image.
        /// </summary>
        /// <param name="imageBytes">The bytes that compose the image for which the dominant color is to be retrieved.</param>
        /// <returns>A dominant color in the provided image.</returns>
        Color GetDominantColor(byte[] imageBytes);
    }

    public sealed class ImageService : IImageService
    {
        public ImageService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = memoryCache;
            _colorThief = new ColorThief();
        }

        /// <inheritdoc />
        public async ValueTask<Color> GetDominantColorAsync(Uri location)
        {
            var key = GetKey(location);

            if (!_cache.TryGetValue(key, out Color color))
            {
                var imageBytes = await _httpClientFactory.CreateClient().GetByteArrayAsync(location);
                color = GetDominantColor(imageBytes);

                _cache.Set(key, color, TimeSpan.FromDays(7));
            }

            return color;
        }

        /// <inheritdoc />
        public Color GetDominantColor(byte[] imageBytes)
        {
            var quantizedColor = _colorThief.GetColor(imageBytes.ToBitmap(), 8);

            return (Color)quantizedColor.ToColor();
        }

        private object GetKey(Uri uri) => new { Target = "DominantColor", uri.AbsoluteUri };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ColorThief _colorThief;
    }
}