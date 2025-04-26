using System.IO;
using System.IO.Compression;
using System.Text;
using AIExtensionsCenter.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using MimeKit.Encodings;

namespace AIExtensionsCenter.Infrastructure.Services
{
    public class RubyFileService : IRubyFileService
    {
        private readonly IHostEnvironment _env;

        public RubyFileService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> GenerateRubyFileAsync(string extensionId, string moduleName)
        {
            // ConfiAI
            var configAIPath = Path.Combine(_env.ContentRootPath, "Ruby", "ConfigAI_template.rb");
            if (!File.Exists(configAIPath)) throw new FileNotFoundException("ConfigAI_template.rb not found.");
            string configAI = await File.ReadAllTextAsync(configAIPath, Encoding.UTF8);
            configAI = configAI.Replace("{{EXTENSION_ID}}", extensionId)
                               .Replace("{{ModuleName}}", moduleName);

            // AIExtnsionsCenter
            var aiExtensionsCenterPath = Path.Combine(_env.ContentRootPath, "Ruby", "AIExtensionsCenter.rbe");
            if (!File.Exists(aiExtensionsCenterPath)) throw new FileNotFoundException("AIExtensionsCenter.rbe not found.");
            var aiExtensionsCenter = await File.ReadAllBytesAsync(aiExtensionsCenterPath);


            // Zip
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var configAIEntry = archive.CreateEntry("ConfigAI.rb");
                using (var configAIStream = configAIEntry.Open())
                using (var writer = new StreamWriter(configAIStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(configAI);
                }

                var aiExtensionsCenterEntry = archive.CreateEntry("AIExtensionsCenter.rbe");
                using (var aiExtensionsCenterStream = aiExtensionsCenterEntry.Open())
                {
                    await aiExtensionsCenterStream.WriteAsync(aiExtensionsCenter);
                }
            }

            return memoryStream.ToArray();
        }
    }
}
