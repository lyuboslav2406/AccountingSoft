using System;
using System.Diagnostics;
using System.IO;

namespace AccountingSoft.Services.Data
{
    public class PdfService : IPdfService
    {
        public byte[] Convert(string basePath, string htmlCode)
        {
            var inputFileName = $"input_{Guid.NewGuid()}.html";
            var outputFileName = $"output_{Guid.NewGuid()}.pdf";
            File.WriteAllText($"{basePath}/{inputFileName}", htmlCode);
            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = @"wwwroot/js/phantomjs.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    Arguments = $"rasterize.js \"{inputFileName}\" \"{outputFileName}\" \"А4\" \"Letter\"",
                    WorkingDirectory = basePath,
                },
            };
            try
            {
                cmd.Start();
            }
            catch (Exception e)
            {

                throw;
            }
            cmd.WaitForExit();

            var bytes = File.ReadAllBytes($"{basePath}/{outputFileName}");

            File.Delete($"{basePath}/{inputFileName}");
            File.Delete($"{basePath}/{outputFileName}");

            return bytes;
        }
    }
}
