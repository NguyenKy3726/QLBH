using Microsoft.AspNetCore.Http;

namespace QLBH.Services;

public sealed record ImageUploadResult(byte[]? Bytes, string? FileName, string? ContentType, string? Error);

public static class ImageUploadService
{
    private const long MaxBytes = 5 * 1024 * 1024;

    public static async Task<ImageUploadResult> ReadAsync(IFormFile? file)
    {
        if (file is null)
            return new ImageUploadResult(null, null, null, null);

        if (file.Length <= 0 || file.Length > MaxBytes)
            return new ImageUploadResult(null, null, null, "Ảnh phải có dung lượng từ 1 byte đến 5 MB.");

        var fileName = Path.GetFileName(file.FileName.Replace('\\', '/')).Trim();
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Length > 255)
            return new ImageUploadResult(null, null, null, "Tên file ảnh không hợp lệ hoặc dài quá 255 ký tự.");

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => null
        };
        if (contentType is null)
            return new ImageUploadResult(null, null, null, "Chỉ nhận ảnh JPG, PNG hoặc WEBP.");

        await using var input = file.OpenReadStream();
        var header = new byte[12];
        var bytesRead = await input.ReadAsync(header.AsMemory(0, header.Length));
        var validSignature = extension switch
        {
            ".jpg" or ".jpeg" => bytesRead >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            ".png" => bytesRead >= 8 && header.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            ".webp" => bytesRead >= 12 && header.AsSpan(0, 4).SequenceEqual("RIFF"u8) && header.AsSpan(8, 4).SequenceEqual("WEBP"u8),
            _ => false
        };
        if (!validSignature)
            return new ImageUploadResult(null, null, null, "Nội dung file không đúng định dạng ảnh đã chọn.");

        input.Position = 0;
        await using var buffer = new MemoryStream((int)file.Length);
        await input.CopyToAsync(buffer);
        return new ImageUploadResult(buffer.ToArray(), fileName, contentType, null);
    }
}
