using Microsoft.Extensions.DependencyInjection;

namespace IW5_project.IW5_project.Common.Installers
{
    public interface IInstaller
    {
        void Install(IServiceCollection serviceCollection);
    }
}