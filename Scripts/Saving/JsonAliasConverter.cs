using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonAliasConverter<T> : JsonConverter<T> where T : new()
{
    private readonly Dictionary<string, PropertyInfo> _propertyMap;

    public JsonAliasConverter()
    {
        _propertyMap = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

        foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanWrite) continue;

            // Add the actual property name
            _propertyMap[prop.Name] = prop;

            // Add aliases if any
            var aliasAttr = prop.GetCustomAttribute<JsonAliasAttribute>();
            if (aliasAttr != null)
            {
                foreach (var alias in aliasAttr.Aliases)
                {
                    _propertyMap[alias] = prop;
                }
            }
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        var obj = new T();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return obj;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString();

            if (!_propertyMap.TryGetValue(propertyName, out PropertyInfo prop))
            {
                // Skip unknown properties
                reader.Skip();
                continue;
            }

            reader.Read();

            object value = JsonSerializer.Deserialize(ref reader, prop.PropertyType, options);
            prop.SetValue(obj, value);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead) continue;

            var propValue = prop.GetValue(value);

            // Use the actual property name (not alias) for serialization
            writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name);
            JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
        }

        writer.WriteEndObject();
    }
}
