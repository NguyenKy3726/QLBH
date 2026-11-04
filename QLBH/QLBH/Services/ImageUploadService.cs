using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace QLBH.Services;

public sealed record ImageUploadResult(string? RelativePath, string? Error);

public static class ImageUploadService
{
    private const long MaxFileSize = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedFolders = new(StringComparer.OrdinalIgnoreCase)
    {
        "products", "employees", "customers"
    };

    public static async Task<string?> ValidateAsync(IFormFile? file, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return null;

        if (file.Length > MaxFileSize)
            return "Ảnh phải có dung lượng tối đa 5 MB.";

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var expectedSignature = extension switch
        {
            ".jpg" or ".jpeg" => new byte[] { 0xFF, 0xD8, 0xFF },
            ".png" => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
            ".webp" => new byte[] { 0x52, 0x49, 0x46, 0x46 },
            _ => null
        };

        if (expectedSignature is null)
            return "Chỉ chấp nhận ảnh JPG, PNG hoặc WEBP.";

        try
        {
            var header = new byte[12];
            await using var input = file.OpenReadStream();
            var bytesRead = 0;
            while (bytesRead < header.Length)
            {
                var count = await input.ReadAsync(header.AsMemory(bytesRead), cancellationToken);
                if (count == 0)
                    break;
                bytesRead += count;
            }

            if (bytesRead < expectedSignature.Length ||
                !header.AsSpan(0, expectedSignature.Length).SequenceEqual(expectedSignature))
                return "Tệp được chọn không phải ảnh hợp lệ.";

            if (extension == ".webp" &&
                (bytesRead < 12 || !header.AsSpan(8, 4).SequenceEqual("WEBP"u8)))
                return "Tệp WEBP không hợp lệ.";
        }
        catch (IOException)
        {
            return "Không thể đọc ảnh đã chọn. Vui lòng chọn lại tệp.";
        }

        return null;
    }

    public static async Task<ImageUploadResult> SaveAsync(
        IFormFile? file,
        IWebHostEnvironment environment,
        string folder,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return new(null, null);

        if (!AllowedFolders.Contains(folder))
            return new(null, "Thư mục lưu ảnh không hợp lệ.");

        var validationError = await ValidateAsync(file, cancellationToken);
        if (validationError is not null)
            return new(null, validationError);

        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(environment.ContentRootPath, "wwwroot");

        try
        {
            var uploadFolder = Path.Combine(webRoot, "uploads", folder.ToLowerInvariant());
            Directory.CreateDirectory(uploadFolder);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(uploadFolder, fileName);
            await using (var output = File.Create(fullPath))
            await using (var input = file.OpenReadStream())
            {
                await input.CopyToAsync(output, cancellationToken);
            }

            return new($"/uploads/{folder.ToLowerInvariant()}/{fileName}", null);
        }
        catch (IOException)
        {
            return new(null, "Không thể lưu ảnh. Hãy thử lại hoặc chọn ảnh khác.");
        }
        catch (UnauthorizedAccessException)
        {
            return new(null, "Ứng dụng không có quyền lưu ảnh vào thư mục tải lên.");
        }
    }
}
