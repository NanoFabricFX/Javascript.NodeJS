﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.IO;
using System.Text;

namespace Jering.JavascriptUtils.NodeJS
{
    public class JsonService : IJsonService
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly JsonSerializer _jsonSerializer;

        public JsonService()
        {
            _jsonSerializer = JsonSerializer.Create(_jsonSerializerSettings);
        }

        public void Serialize(JsonWriter jsonWriter, object value)
        {
            jsonWriter.Formatting = _jsonSerializer.Formatting;
            _jsonSerializer.Serialize(jsonWriter, value);
        }

        public string Serialize(object value)
        {
            // TODO can be pooled to reduce allocations
            var stringBuilder = new StringBuilder(256);
            var stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
            var jsonTextWriter = new JsonTextWriter(stringWriter);

            _jsonSerializer.Serialize(jsonTextWriter, value);

            return stringBuilder.ToString();
        }

        public T Deserialize<T>(JsonReader jsonReader)
        {
            return _jsonSerializer.Deserialize<T>(jsonReader);
        }
    }
}