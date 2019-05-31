using System;
using System.Collections.Generic;
using System.Text;

namespace FileConverter
{
    public interface IFileInfo
    {
        string FileName { get; }        
        string Path { get; set; }
    }
}
