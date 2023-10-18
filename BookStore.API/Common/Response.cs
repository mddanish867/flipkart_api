using System.Text.Json.Serialization;

namespace BookStore.API.Common
{
    public class Response
    {
        public static Stream OutputStream { get; internal set; }
        public string message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? result { get; set; }

        public class ResponseName
        {
            public string message { get; set; } = "message";
            public string result { get; set; } = "result";
        }
    }
}
