using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VirusTask.Helper
{
    public class FirebaseHelper
    {
        //get links to images of user
        public static async Task<List<string>> GetVirusGallery(string uid)
        {
            var databaseRef = new FirebaseClient(Constants.databaseUrl);
            var items = await databaseRef.Child("gallery/" + uid)
                .OnceAsync<object>();
            var images = new List<string>();

            foreach (var item in items)
            {
                images.Add((string)item.Object);
                Console.WriteLine(item.Object);
            }

            return images;
        }

        //save new photo in gallery
        public static async Task SaveVirusPhoto(string uid, string imageUrl)
        {
            var databaseRef = new FirebaseClient(Constants.databaseUrl);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(imageUrl);
            await databaseRef
                .Child("gallery/" + uid + "/" + Guid.NewGuid().ToString())
                .PutAsync(json);
        }

        //get object from firebase
        public static async Task<Virus> GetVirus(string fullName)
        {
            var viruses = await FetchViruses();
            return viruses.FindLast((virus) =>
            {
                return virus.fullName == fullName;
            });
        }


        //get all viruses from firebase
        public static async Task<List<Virus>> FetchViruses()
        {
            var viruses = new List<Virus>();
            try
            {
                var databaseRef = new FirebaseClient(Constants.databaseUrl);
                var items = await databaseRef.Child("Viruses/")
                                .OrderByKey()
                                .OnceAsync<Virus>();
              

                foreach (var item in items)
                {
                    viruses.Add(item.Object);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            


            return viruses;
        }
    }
}
