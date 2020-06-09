using System;
using System.Diagnostics;
using System.IO;

namespace AccountingSoft.Services.Data
{
    public interface IPdfService
    {
        byte[] Convert(string basePath, string htmlCode);
    }
}
