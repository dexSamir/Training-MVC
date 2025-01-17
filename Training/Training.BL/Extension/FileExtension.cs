using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BL.Extension;
public static class FileExtension
{
    public static bool IsValidType(this IFormFile file, string type)
        => file.ContentType.StartsWith(type);

    public static bool IsValidSize(this IFormFile file, int mb)
        => file.Length <= mb * 1024 *1024;

    public static async Task<string> UploadAsync(this IFormFile file, params string[] paths)
    {
        string path = Path.Combine(paths); 
        if(System.IO.File.Exists(path)) 
            Directory.CreateDirectory(path);

        string filename = Path.GetRandomFileName() + Path.GetExtension(file.FileName); 
        using(Stream sr = File.Create(Path.Combine(path, filename)))
            await file.CopyToAsync(sr);

        return filename;
    }
}
