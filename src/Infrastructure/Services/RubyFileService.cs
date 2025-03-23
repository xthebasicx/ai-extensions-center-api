using System.Text;
using AIExtensionsCenter.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace AIExtensionsCenter.Infrastructure.Services
{
    public class RubyFileService : IRubyFileService
    {
        private readonly IHostEnvironment _env;

        public RubyFileService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> GenerateRubyFileAsync(string extensionId)
        {
            var templatePath = Path.Combine(_env.ContentRootPath, "Ruby", "AiExtensionsCenter_template.rb");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException("Ruby template file not found.");
            }
            string rubyCode = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);

            rubyCode = rubyCode.Replace("{{EXTENSION_ID}}", extensionId);

            return Encoding.UTF8.GetBytes(rubyCode);
        }
    }
}
