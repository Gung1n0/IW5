using Microsoft.Extensions.DependencyInjection;

namespace IW5_project.Common.Models
{
    public interface IInstaller
    {
        void Install(IServiceCollection serviceCollection);
    }
}