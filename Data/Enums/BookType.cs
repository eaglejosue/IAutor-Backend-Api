namespace IAutor.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookType
{
    Size210X297,
    Size148X210
}
