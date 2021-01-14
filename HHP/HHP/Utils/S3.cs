using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amazon.CognitoIdentity;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using HHP.secrets;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MongoDB.Bson.Serialization.Serializers;

namespace HHP.Utils
{
    /// <summary>
    /// The main AWS S3 Bucket class.
    /// Contains all methods for performing a stable AWS S3 Bucket connection.
    /// </summary>
    public class S3
    {
        private static CognitoAWSCredentials cognitoCredentials;
        private static IAmazonS3 s3Client;

        /// <summary>
        /// Get the CognitoAWSCredentials from another C#-Data
        /// </summary>
        public static CognitoAWSCredentials Credentials
        {
            get
            {
                if (cognitoCredentials == null)
                {
                    cognitoCredentials = new CognitoAWSCredentials(ConstantsS3.COGNITO_POOL_ID, ConstantsS3.REGION);
                }
                return cognitoCredentials;
            }
        }
        public static IAmazonS3 S3Client
        { 
            get
            {
                if (s3Client == null)
                {
                    s3Client = new AmazonS3Client(Credentials, ConstantsS3.REGION);
                }
                return s3Client;
            }
        }
        /// <summary>
        /// Build a new S3 Bucket if it does not exist.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> BucketExist()
        {
            try
            {
                var response = await S3Client.ListObjectsAsync(new ListObjectsRequest()
                {
                    BucketName = ConstantsS3.BUCKET_NAME.ToLowerInvariant(),
                    MaxKeys = 0
                }).ConfigureAwait(false);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                if ((e.StatusCode.Equals(ConstantsS3.BUCKET_REDIRECT_STATUS_CODE)) || e.StatusCode.Equals(ConstantsS3.BUCKET_ACCESS_FORBIDDEN_STATUS_CODE))
                {
                    return true;
                }
                else if (e.StatusCode.Equals(ConstantsS3.NO_SUCH_BUCKET_STATUS_CODE))
                {
                    return false;
                }
                else
                {
                    throw e;
                }
            }
        }
        /// <summary>
        /// Uploads the file to the S3 Bucket.
        /// Returns the http-string of the uploaded photo.
        /// </summary>
        public static async Task<String> Upload(String Path, String PhotoName)
        {
            if(Path == null || PhotoName == null)
            { return "Path and Name has to be set."; }
            
            var s3Client = Utils.S3.S3Client;
            string s3Path = "https://homehp.s3.amazonaws.com/" + PhotoName;

            await s3Client.PutObjectAsync(new PutObjectRequest()
            {
               BucketName = ConstantsS3.BUCKET_NAME.ToLowerInvariant(),
               FilePath = Path,
               Key = PhotoName,                     
               CannedACL = S3CannedACL.PublicRead
               });                                  

            return s3Path;  
        }
        /// <summary>
        /// Uploads the photo-file to the S3 Bucket and shows it at the Xamarin.Forms.Image.
        /// Returns the http-string of the uploaded photo.
        /// </summary>
        public static async Task<String> UploadAndShow(String Path, String PhotoName, Image image, MediaFile file)
        {
            if (Path == null || PhotoName == null || image == null || file == null)
            { return "Path and Name has to be set."; }

            var s3Client = Utils.S3.S3Client;
            string s3Path = "https://homehp.s3.amazonaws.com/" + PhotoName;

            await s3Client.PutObjectAsync(new PutObjectRequest()
            {
                BucketName = ConstantsS3.BUCKET_NAME.ToLowerInvariant(),
                FilePath = Path,
                Key = PhotoName,
                CannedACL = S3CannedACL.PublicRead
            });

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            return s3Path;
        }
        /// <summary>
        /// Shows the photo with the file path at the Xamarin.Forms.Image.
        /// </summary>
        public static async void ShowPhotoFromS3(Image image, String filePath)
        {
            await CrossMedia.Current.Initialize();

            char[] c  = filePath.ToCharArray();

            var file = await CrossMedia.Current.PickPhotoAsync();

            if(c[0] != 'h' || c[1] != 't' || c[2] != 't' || c[3] != 'p' ||
                c[4] != 's' || c[5] != ':' || c[6] != '/' || c[7] != '/')
            {
                return;
            }

            if (file == null || filePath == null)
                return ;

            image.Source = ImageSource.FromUri(new Uri(filePath));
        }
        /// <summary>
        /// Shows the photo (MediaFile) at the Xamarin.Forms.Image
        /// </summary>
        public static async void ShowPhotoFromStorage(Image image, MediaFile file)
        {
            await CrossMedia.Current.Initialize();

            if (file == null || image == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}