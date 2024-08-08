namespace hotel_back.Helpers.Extensions;

public static class FileExtension
{
    public static bool CheckFileType(this IFormFile file, string fileType)
        => file.ContentType.Contains(fileType);
}
