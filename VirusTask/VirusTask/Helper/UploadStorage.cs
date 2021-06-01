using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace VirusTask.Helper
{
    public class UploadStorage
    {
        //load gallery for user
        public static async Task<string> UploadGalleryFile(string uid, Stream fileStream, string fileName)
        {
            FirebaseStorage firebaseStorage = new FirebaseStorage(Constants.storageUrl);

            var imageUrl = await firebaseStorage
                .Child("gallery/" + uid)
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        //load edit and registration profile image
        public static async Task<string> UploadProfileFile(Stream fileStream, string fileName)
        {
            FirebaseStorage firebaseStorage = new FirebaseStorage(Constants.storageUrl);

            var imageUrl = await firebaseStorage
                .Child("images/" + fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
    }
}
