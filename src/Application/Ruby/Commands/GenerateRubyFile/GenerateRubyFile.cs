using AIExtensionsCenter.Application.Common.Interfaces;

namespace AIExtensionsCenter.Application.Commands
{
    public class GenerateRubyFileCommand : IRequest<byte[]>
    {
        public string ExtensionId { get; init; }
        public string ModuleName { get; init; }

        public GenerateRubyFileCommand(string extensionId, string moduleName)
        {
            ExtensionId = extensionId;
            ModuleName = moduleName;
        }
    }

    public class GenerateRubyFileCommandHandler : IRequestHandler<GenerateRubyFileCommand, byte[]>
    {
        private readonly IRubyFileService _rubyFileService;

        public GenerateRubyFileCommandHandler(IRubyFileService rubyFileService)
        {
            _rubyFileService = rubyFileService;
        }

        public async Task<byte[]> Handle(GenerateRubyFileCommand request, CancellationToken cancellationToken)
        {
            return await _rubyFileService.GenerateRubyFileAsync(request.ExtensionId, request.ModuleName);
        }
    }
}
