using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VioletPixiv
{
    public static class GetImages
    {

        #region Load images functions. Tanks to https://github.com/CryShana/CryPixiv

        public static async Task<BitmapImage> GetImage(string url)
        {
            var image = new BitmapImage();
            var buffer = DownloadImage(url);

            using (var stream = new MemoryStream(await buffer))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }

        #pragma warning disable 1998
        private static async Task<byte[]> DownloadImage(string url)
        {
            
            var buffer = new byte[0];
            using (var client = new WebClient())
            {
                client.Headers.Add("Referer", "http://api.pixiv.net/");
                client.Headers.Add("User-Agent", "PixivIOSApp/5.8.0");
                client.UseDefaultCredentials = true;
                buffer = client.DownloadData(url);
            }
            return buffer;
        }
        #pragma warning restore 1998

        #endregion
    }

}
