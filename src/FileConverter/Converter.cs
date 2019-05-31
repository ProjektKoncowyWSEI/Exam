using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    public class Converter : IConvertable, IConvertableAsync
    {
        public Converter() { }
        public Converter(string path)
        {
            Path = path;
        }
        public string FileName
        {
            get
            {
                return getName(Path);
            }
        }
        public string Path { get; set; }
        private string getName(string path)
        {
            var arr = path.Split('/');
            return arr[arr.Length - 1];
        }

        #region To byte[]
        public byte[] GetByteArray(MemoryStream stream)
        {
            return stream.ToArray();
        }
        public byte[] GetByteFromFile(string path)
        {
            return File.ReadAllBytes(path);
        }
        public async Task<byte[]> GetByteArrayAsync(MemoryStream stream)
        {
            return await Task.Run(() => stream.ToArray());
        }
        public async Task<byte[]> GetByteFromFileAsync(string path)
        {
            return await Task.Run(() => File.ReadAllBytes(path));
        }
        #endregion

        #region To MemoryStream
        public MemoryStream GetMemoryStream(byte[] array)
        {
            return new MemoryStream(array);
        }
        public async Task<MemoryStream> GetMemoryStreamAsync(byte[] array)
        {
            return await Task.Run(() => new MemoryStream(array));
        }

        public MemoryStream GetMemoryStreamFromFile(string path)
        {
            return GetMemoryStream(GetByteFromFile(path));
        }
        public async Task<MemoryStream> GetMemoryStreamFileAsync(string path)
        {
            return await GetMemoryStreamAsync(await GetByteFromFileAsync(path));
        }
        #endregion

        #region To Base64String
        public string GetStringBase64(byte[] array)
        {
            return Convert.ToBase64String(array);
        }
        public string GetStringBase64(MemoryStream stream)
        {
            return GetStringBase64(GetByteArray(stream));
        }
        public string GetStringBase64(string path)
        {
            return GetStringBase64(GetByteFromFile(path));
        }
        public async Task<string> GetStringBase64Async(byte[] array)
        {
            return await Task.Run(() => Encoding.UTF8.GetString(array));
        }
        public async Task<string> GetStringBase64Async(MemoryStream stream)
        {
            return await GetStringBase64Async(await GetByteArrayAsync(stream));
        }
        public async Task<string> GetStringBase64Async(string path)
        {
            return await GetStringBase64Async(await GetByteFromFileAsync(path));
        }
        #endregion

        #region SaveFile
        public void SaveFile(byte[] content, string path)
        {
            File.WriteAllBytes(path, content);
        }
        public void SaveFileBytesAsString(byte[] content, string path)
        {
            var stringArray = string.Join(",", content);
            File.WriteAllText(path, stringArray);
        }
        public void SaveFile(MemoryStream content, string path)
        {
            SaveFile(content.ToArray(), path);
        }
        public async Task SaveFileAsync(byte[] content, string path)
        {
            await Task.Run(() => File.WriteAllBytes(path, content));
        }
        public async Task SaveFileBytesAsStringAsync(byte[] content, string path)
        {
            await Task.Run(() => SaveFileBytesAsString(content, path));
        }
        public async Task SaveFileAsync(MemoryStream content, string path)
        {
            await SaveFileAsync(content.ToArray(), path);
        }
        #endregion

    }
}
