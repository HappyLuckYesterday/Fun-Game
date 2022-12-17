using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rhisis.Protocol.Messages;

public class CoreMessageJsonConverter : JsonConverter<CoreMessage>
{
    private const string TypePropertyName = "type";
    private const string ValuePropertyName = "value";

    private static readonly Dictionary<string, Type> _cachedMessageTypes = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(CoreMessage).IsAssignableFrom(typeToConvert);
    }

    public override CoreMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        if (!reader.Read()
                || reader.TokenType != JsonTokenType.PropertyName
                || reader.GetString() != TypePropertyName)
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        CoreMessage message;
        string messageTypeName = reader.GetString();
        Type messageType = GetType(messageTypeName);

        if (!reader.Read()
                || reader.TokenType != JsonTokenType.PropertyName
                || reader.GetString() != ValuePropertyName)
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string value = reader.GetString();

        message = (CoreMessage)JsonSerializer.Deserialize(value, messageType);

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException();
        }

        message.Type = messageType;

        return message;
    }

    public override void Write(Utf8JsonWriter writer, CoreMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(TypePropertyName, value.GetType().FullName);
        writer.WriteString(ValuePropertyName, JsonSerializer.Serialize((object)value));
        writer.WriteEndObject();
    }

    private static Type GetType(string typeName)
    {
        if (_cachedMessageTypes.TryGetValue(typeName, out Type type))
        {
            return type;
        }

        Type foundType = Type.GetType(typeName);

        if (foundType is null)
        {
            throw new InvalidOperationException($"Failed to find type '{typeName}'.");
        }

        if (!_cachedMessageTypes.TryAdd(typeName, foundType))
        {
            return _cachedMessageTypes.GetValueOrDefault(typeName);
        }

        return foundType;
    }
}
