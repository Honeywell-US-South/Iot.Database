using Remote.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Iot.Database.Queries;

public class ExpressionConverter : JsonConverter<LambdaExpression>
{
    public override LambdaExpression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement custom deserialization logic here
        var json = JsonDocument.ParseValue(ref reader);
        var expression = System.Text.Json.JsonSerializer.Deserialize<LambdaExpression>(json.RootElement.GetRawText(), options);
        return expression;
    }

    public override void Write(Utf8JsonWriter writer, LambdaExpression value, JsonSerializerOptions options)
    {
        // Implement custom serialization logic here
        System.Text.Json.JsonSerializer.Serialize(writer, value, options);
    }
}
