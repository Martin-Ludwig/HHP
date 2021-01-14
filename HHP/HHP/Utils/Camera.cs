using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HHP.Utils
{
    /// <summary>
    /// The main camer class.
    /// Contains all methods for performing camera/photo functions.
    /// </summary>
    public static class Camera
    {
        /// <summary>
        /// Uses the internal camera to take a photo,
        /// saves it in the internal storage and
        /// returns its path as a string
        /// </summary>
        public static async Task<String> TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
            });

            if (file == null)
                return "";

            return file.AlbumPath;
        }
        /// <summary>
        /// Returns a showable ImageSource for a Xamarin.Forms.Image .
        /// </summary>
        public static ImageSource LoadPhoto(String path)
        {
            return ImageSource.FromFile(path);
        }
        /// <summary>
        /// It shows a photo in the Xamarin.Forms.Image which were
        /// recently taken with the internal camera.
        /// </summary>
        public static async Task<String> TakePhotoAndShow(Image image)
        {

            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
            });

            if (file == null || image == null)
                return "";

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            return file.AlbumPath;
        }
        /// <summary>
        /// Pick a Photo from the internal storage.
        /// Return its path as a string.
        /// </summary>
        /// <returns></returns>
        public static async Task<String> PickPhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return "";

            return file.Path;
        }
        /// <summary>
        /// Pick a photo from the internal storage and 
        /// shows it in the given Xamarin.Forms.Image .
        /// Returns the photo-path as a string.
        /// </summary>
        public static async Task<String> PickPhotoAndShow(Image image)
        {
            // 
            await CrossMedia.Current.Initialize();


            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null || image == null)
                return "";


            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            return file.AlbumPath;
        }
    }

}
