using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LINE_Webhook.Utilities
{

    public static class JSON {

        public static string JsonSerialize(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public static string JsonSerialize(this object obj, string typeName) {
            return JsonConvert.SerializeObject(new { typeName = obj });
        }

        public static string JsonSerializeIgnoreLoop(this object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                        new JsonSerializerSettings() {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        }

        public static async Task<string> JsonSerializeAsync(this object obj) {
            return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(obj));
        }

        public static object JsonDeserialize(this string jsonStr) {
            return JsonConvert.DeserializeObject(jsonStr);
        }

        public static object JsonDeserialize<T>(this string jsonStr) {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        public static async Task<object> JsonDeserializeAsync(this string jsonStr) {
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(jsonStr));
        }

        //public static async Task<object> JsonDeserializeAsync<T>(this string jsonStr) {
        //    return await JsonConvert.DeserializeObjectAsync<T>(jsonStr);
        //}

    }

    public class JsonData {
        private static JObject dataX;

        private static readonly JsonData instance = new JsonData();

        public static JsonData Instance() {
            return instance;
        }

        public static JsonData Instance(JObject data) {
            dataX = data;
            return instance;
        }

        public JToken Get_JsonObject(string objectName) {
            return dataX.GetValue(objectName);
        }

        public JToken Get_JsonObject(string objectSource, string objectName) {
            return Instance((JObject)objectSource.JsonDeserialize()).Get_JsonObject(objectName);
        }

        public JObject JsonObject {
            get { return dataX; }
        }
    }

    public class JEntity<T> {

        private static readonly JEntity<T> instance = new JEntity<T>();

        public static JEntity<T> Instance() {
            return instance;
        }

        public T Get_Entity(JObject data) {
            var tName = typeof(T).Name;
            var model = data.GetValue(tName);
            return model.ToObject<T>();
        }

        public T Get_Entity(JsonData data) {
            var tName = typeof(T).Name;
            var model = data.JsonObject.GetValue(tName);
            return model.ToObject<T>();
        }

    }

}
