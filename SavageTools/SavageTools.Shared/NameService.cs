using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

namespace SavageTools.Names
{
    public static class NameService
    {
        readonly static ConcurrentQueue<RandomPerson> s_Users = new ConcurrentQueue<RandomPerson>();
        readonly static HttpClient s_HttpClient = new HttpClient();

        public static async Task<RandomPerson> CreateRandomPersonAsync()
        {
            RandomPerson result = null;

            s_Users.TryDequeue(out result);

            if (result == null)
            {
                try
                {

                    var rawString = await s_HttpClient.GetStringAsync(new Uri("http://api.randomuser.me/?results=100")).ConfigureAwait(false);
                    var collection = JsonConvert.DeserializeObject<UserRoot>(rawString);

                    result = new RandomPerson(collection.results[0]);
                    for (var i = 1; i < collection.results.Length; i++)
                        s_Users.Enqueue(new RandomPerson(collection.results[i]));
                }
                catch
                {
                    result = new RandomPerson("Error", "Error", "?");
                }
            }
            return result;
        }
    }

    public class RandomPerson
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomName"/> class.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        internal RandomPerson(Result user)
        {
            FirstName = user.name.first.Substring(0, 1).ToUpper() + user.name.first.Substring(1);
            LastName = user.name.last.Substring(0, 1).ToUpper() + user.name.last.Substring(1);
            Gender = user.gender == "female" ? "F" : "M";
        }

        internal RandomPerson(string first, string last, string gender)
        {
            FirstName = first;
            LastName = last;
            Gender = gender;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }


    }



    public class UserRoot
    {
        public Result[] results { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public string seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string version { get; set; }
    }

    public class Result
    {
        public string gender { get; set; }
        public Name name { get; set; }
        public Location location { get; set; }
        public string email { get; set; }
        public Login login { get; set; }
        public string dob { get; set; }
        public string registered { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public Id id { get; set; }
        public Picture picture { get; set; }
        public string nat { get; set; }
    }

    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Location
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public object postcode { get; set; }
    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public string sha256 { get; set; }
    }

    public class Id
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }

}



