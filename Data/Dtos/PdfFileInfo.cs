namespace IAutor.Api.Data.Dtos;

public record PdfFileInfo(
    byte[] ByteArray,
    string MimeType,
    string FileName
);
