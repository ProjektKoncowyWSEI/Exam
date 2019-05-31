using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    public interface IConvertableAsync : IFileInfo
    {  
        Task<byte[]> GetByteArrayAsync(MemoryStream stream);
        Task<byte[]> GetByteFromFileAsync(string path);
        Task<MemoryStream> GetMemoryStreamAsync(byte[] array);
        Task<MemoryStream> GetMemoryStreamFileAsync(string path);
        Task<string> GetStringBase64Async(byte[] array);
        Task<string> GetStringBase64Async(MemoryStream stream);
        Task<string> GetStringBase64Async(string path);
        Task SaveFileAsync(byte[] content, string path);
        Task SaveFileBytesAsStringAsync(byte[] content, string path);
        Task SaveFileAsync(MemoryStream content, string path);
    }
}
