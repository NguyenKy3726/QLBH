using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace QLBH.Services;

public sealed record ProductImageUploadResult(string? RelativePath, string? Error);

public static class ProductImageUploadService
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    public static async Task<ProductImageUploadResult> SaveAsync(
        IFormFile? file,
        IWebHostEnvironment environment,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return new(null, null);

        if (file.Length > MaxFileSize)
            return new(null, "Ảnh phải có dung lượng tối đa 5 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var expectedSignature = extension switch
        {
            ".jpg" or ".jpeg" => new byte[] { 0xFF, 0xD8, 0xFF },
            ".png" => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
            ".webp" => new byte[] { 0x52, 0x49, 0x46, 0x46 },
            _ => null
        };

        if (expectedSignature is null)
            return new(null, "Chỉ chấp nhận ảnh JPG, PNG hoặc WEBP.");

        var header = new byte[12];
        await using (var input = file.OpenReadStream())
        {
            var bytesRead = await input.ReadAsync(header.AsMemory(0, header.Length), cancellationToken);
            if (bytesRead < expectedSignature.Length ||
                !header.AsSpan(0, expectedSignature.Length).SequenceEqual(expectedSignature))
                return new(null, "Tệp được chọn không phải ảnh hợp lệ.");

            if (extension == ".webp" &&
                (bytesRead < 12 || !header.AsSpan(8, 4).SequenceEqual("WEBP"u8)))
                return new(null, "Tệp WEBP không hợp lệ.");
        }

        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(environment.ContentRootPath, "wwwroot");

        var uploadFolder = Path.Combine(webRoot, "uploads", "products");
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadFolder, fileName);
        await using (var output = File.Create(fullPath))
        await using (var input = file.OpenReadStream())
        {
            await input.CopyToAsync(output, cancellationToken);
        }

        return new($"/uploads/products/{fileName}", null);
    }
}
