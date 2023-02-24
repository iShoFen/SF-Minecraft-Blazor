using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RestController;

using JsonConverter = JsonConverter;
using JsonSerializer = JsonSerializer;

public readonly struct IngredientElement
{
    public IngredientClass IngredientClass { get; init; }
    public long? Integer { get; init; }

    public static implicit operator IngredientElement(IngredientClass ingredientClass) => new()
        { IngredientClass = ingredientClass };

    public static implicit operator IngredientElement(long integer) => new()
        { Integer = integer };
}

public readonly struct InShape
{
    public IngredientClass IngredientClass { get; init; }
    public long? Integer { get; init; }

    public bool IsNull => IngredientClass == null && Integer == null;

    public static implicit operator InShape(IngredientClass ingredientClass) => new()
        { IngredientClass = ingredientClass };

    public static implicit operator InShape(long integer) => new()
        { Integer = integer };
}

public static class Serialize
{
    public static string ToJson(this Dictionary<string, Recipes[]> self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

public class IngredientClass
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("metadata")]
    public long Metadata { get; set; }
}

public partial class Recipes
{
    [JsonProperty("ingredients", NullValueHandling = NullValueHandling.Ignore)]
    public IngredientElement[] Ingredients { get; set; } = Array.Empty<IngredientElement>();

    [JsonProperty("inShape", NullValueHandling = NullValueHandling.Ignore)]
    public InShape[][] InShape { get; set; } = Array.Empty<InShape[]>();

    [JsonProperty("outShape", NullValueHandling = NullValueHandling.Ignore)]
    public long?[][] OutShape { get; set; } = Array.Empty<long?[]>();

    [JsonProperty("result")]
    public Result Result { get; set; } = new();
}

public partial class Recipes
{
    public static Dictionary<string, Recipes[]> FromJson(string json)
        => JsonConvert.DeserializeObject<Dictionary<string, Recipes[]>>(json, Converter.Settings) ?? new();
}

public class Result
{
    [JsonProperty("count")]
    public long Count { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("metadata")]
    public long Metadata { get; set; }
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            InShapeConverter.Singleton,
            IngredientElementConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

internal class IngredientElementConverter : JsonConverter
{
    private const string IngredientArgumentExceptionMessage = "Cannot unmarshal type IngredientElement";
    
    public static readonly IngredientElementConverter Singleton = new();

    public override bool CanConvert(Type objectType) => objectType == typeof(IngredientElement) || objectType == typeof(IngredientElement?);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Integer:
                var integerValue = serializer.Deserialize<long>(reader);
                return new IngredientElement { Integer = integerValue };

            case JsonToken.StartObject:
                var objectValue = serializer.Deserialize<IngredientClass>(reader) ?? throw new ArgumentException(IngredientArgumentExceptionMessage);
                return new IngredientElement { IngredientClass = objectValue };
            default:
                throw new ArgumentException(IngredientArgumentExceptionMessage);
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not IngredientElement ingredient) throw new ArgumentException("Value cannot be null or not IngredientElement");
        
        if (ingredient.Integer != null)
        {
            serializer.Serialize(writer, ingredient.Integer.Value);
            return;
        }

        if (ingredient.IngredientClass == null) throw new ArgumentException(IngredientArgumentExceptionMessage);
        
        serializer.Serialize(writer, ingredient.IngredientClass);
    }
}

internal class InShapeConverter : JsonConverter
{
    private const string InShapeArgumentExceptionMessage = "Cannot unmarshal type InShape";
    
    public static readonly InShapeConverter Singleton = new();

    public override bool CanConvert(Type objectType) => objectType == typeof(InShape) || objectType == typeof(InShape?);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                return new InShape();

            case JsonToken.Integer:
                var integerValue = serializer.Deserialize<long>(reader);
                return new InShape { Integer = integerValue };

            case JsonToken.StartObject:
                var objectValue = serializer.Deserialize<IngredientClass>(reader) ?? throw new ArgumentException(InShapeArgumentExceptionMessage);
                return new InShape { IngredientClass = objectValue };
            
            default:
                throw new ArgumentException(InShapeArgumentExceptionMessage);
        } 
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not InShape inShape) throw new ArgumentException("Value cannot be null or not InShape");
        
        if (inShape.IsNull)
        {
            serializer.Serialize(writer, null);
            return;
        }
        if (inShape.Integer != null)
        {
            serializer.Serialize(writer, inShape.Integer.Value);
            return;
        }

        if (inShape.IngredientClass == null) throw new ArgumentException(InShapeArgumentExceptionMessage);
        
        serializer.Serialize(writer, inShape.IngredientClass);
    }
}