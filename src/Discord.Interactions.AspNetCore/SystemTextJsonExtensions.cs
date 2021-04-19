using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Discord.Interactions.AspNetCore.External;
using Optional;
using Optional.Unsafe;

namespace Discord.Interactions.AspNetCore
{
    public static class SerializerOptions
    {
        public static readonly JsonSerializerOptions InteractionsSerializer =
            new()
            {
                PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy(),
                Converters = { new SnowflakeConverter(), new OptionConverter() }
            };

        private sealed class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    return name;

                StringBuilder builder = new(name.Length + Math.Max(2, name.Length / 5));
                UnicodeCategory? previousCategory = null;

                for (int currentIndex = 0; currentIndex < name.Length; currentIndex++)
                {
                    char currentChar = name[currentIndex];
                    if (currentChar == '_')
                    {
                        builder.Append('_');
                        previousCategory = null;
                        continue;
                    }

                    UnicodeCategory currentCategory = char.GetUnicodeCategory(currentChar);

                    switch (currentCategory)
                    {
                        case UnicodeCategory.UppercaseLetter:
                        case UnicodeCategory.TitlecaseLetter:
                            if (previousCategory == UnicodeCategory.SpaceSeparator ||
                                previousCategory == UnicodeCategory.LowercaseLetter ||
                                previousCategory != UnicodeCategory.DecimalDigitNumber &&
                                currentIndex > 0 &&
                                currentIndex + 1 < name.Length &&
                                char.IsLower(name[currentIndex + 1]))
                            {
                                builder.Append('_');
                            }

                            currentChar = char.ToLower(currentChar);
                            break;

                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.DecimalDigitNumber:
                            if (previousCategory == UnicodeCategory.SpaceSeparator)
                            {
                                builder.Append('_');
                            }

                            break;

                        case UnicodeCategory.Surrogate:
                            break;

                        default:
                            if (previousCategory != null)
                            {
                                previousCategory = UnicodeCategory.SpaceSeparator;
                            }

                            continue;
                    }

                    builder.Append(currentChar);
                    previousCategory = currentCategory;
                }

                return builder.ToString();
            }
        }


        private class SnowflakeConverter : JsonConverter<Snowflake>
        {
            public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => Snowflake.Parse(reader.GetString()!);

            public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
                => writer.WriteNumberValue(value.RawValue);
        }

        private class OptionConverter : JsonConverterFactory
        {
            public override bool CanConvert(Type typeToConvert)
            {
                if (!typeToConvert.IsGenericType) { return false; }
                if (typeToConvert.GetGenericTypeDefinition() != typeof(Option<>)) { return false; }
                return true;
            }

            public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            {
                Type valueType = typeToConvert.GetGenericArguments()[0];

                return Activator.CreateInstance(
                    type: typeof(OptionalConverterInner<>).MakeGenericType(new Type[] { valueType }),
                    bindingAttr: BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: null,
                    culture: null
                ) as JsonConverter;
            }
        }

        //Note: for non-nullable types, use [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)], can be removed when JsonConverters support write rejection
        private class OptionalConverterInner<T> : JsonConverter<Option<T?>>
        {
            public override Option<T?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var value = JsonSerializer.Deserialize<T>(ref reader, options);
                
                return Option.Some(value);
            }

            public override void Write(Utf8JsonWriter writer, Option<T?> value, JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, value.ValueOrDefault(), options);
        }
    }
}
