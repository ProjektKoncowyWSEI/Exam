using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    public interface IConvertable : IFileInfo
    {
        byte[] GetByteArray(MemoryStream stream);
        byte[] GetByteFromFile(string path);
        MemoryStream GetMemoryStream(byte[] array);
        MemoryStream GetMemoryStreamFromFile(string path);
        string GetStringBase64(byte[] array);
        string GetStringBase64(MemoryStream stream);
        string GetStringBase64(string path);
        void SaveFile(byte[] content, string path);
        void SaveFileBytesAsString(byte[] content, string path);
        void SaveFile(MemoryStream content, string path);
    }
}
