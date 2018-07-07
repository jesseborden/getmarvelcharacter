using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace GetMarvelCharacter.MarvelAPI
{
    class GetMarvelCharacter
    {
        private const string publickey = "your public key";
        private const string privatekey = "your private key";

        public async static Task<RootObject> GetCharacter(int CharacterID)
        {
            try
            {
                Random random = new Random();
                int offset = random.Next(0, 1400);
                string requesturl = "https://gateway.marvel.com:443/v1/public/characters?offset=" + offset + CreateHash();
                System.Diagnostics.Debug.WriteLine(requesturl);
                var http = new HttpClient();
                var response = await http.GetAsync(requesturl);
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                if (response.IsSuccessStatusCode == true)
                {
                    var data = (RootObject)serializer.ReadObject(ms);
                    return data;
                }
                else
                {
                    return null;
                }
               
            }
            catch (SerializationException)
            {
                throw new SerializationException("You can't authenticate to the API");
            }
        }

        private static string CreateHash()
        {
            var timestamp = DateTime.Now.Ticks.ToString();
            var toBeHashed = timestamp + privatekey + publickey;
            var hashedmessage = ComputeMD5(toBeHashed);
            var urlsuffix = "&ts=" + timestamp + "&apikey=" + publickey + "&hash=" + hashedmessage;
            return urlsuffix;
        }
        private static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;

        }
    }
    [DataContract]
    public class Thumbnail
    {
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string extension { get; set; }
    }
    [DataContract]
    public class Comics
    {
        [DataMember]
        public int available { get; set; }
        [DataMember]
        public string collectionURI { get; set; }
        [DataMember]
        public List<object> items { get; set; }
        [DataMember]
        public int returned { get; set; }
    }
    [DataContract]
    public class Series
    {
        [DataMember]
        public int available { get; set; }
        [DataMember]
        public string collectionURI { get; set; }
        [DataMember]
        public List<object> items { get; set; }
        [DataMember]
        public int returned { get; set; }
    }
    [DataContract]
    public class Stories
    {
        [DataMember]
        public int available { get; set; }
        [DataMember]
        public string collectionURI { get; set; }
        [DataMember]
        public List<object> items { get; set; }
        [DataMember]
        public int returned { get; set; }
    }
    [DataContract]
    public class Events
    {
        [DataMember]
        public int available { get; set; }
        [DataMember]
        public string collectionURI { get; set; }
        [DataMember]
        public List<object> items { get; set; }
        [DataMember]
        public int returned { get; set; }
    }
    [DataContract]
    public class Url
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string url { get; set; }
    }
    [DataContract]
    public class Result
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        //  [DataMember]
        //  public DateTime modified { get; set; }
        [DataMember]
        public Thumbnail thumbnail { get; set; }
        [DataMember]
        public string resourceURI { get; set; }
        [DataMember]
        public Comics comics { get; set; }
        [DataMember]
        public Series series { get; set; }
        [DataMember]
        public Stories stories { get; set; }
        [DataMember]
        public Events events { get; set; }
        [DataMember]
        public List<Url> urls { get; set; }
    }
    [DataContract]
    public class Data
    {
        [DataMember]
        public int offset { get; set; }
        [DataMember]
        public int limit { get; set; }
        [DataMember]
        public int total { get; set; }
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public List<Result> results { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string copyright { get; set; }
        [DataMember]
        public string attributionText { get; set; }
        [DataMember]
        public string attributionHTML { get; set; }
        [DataMember]
        public string etag { get; set; }
        [DataMember]
        public Data data { get; set; }
    }
}
    