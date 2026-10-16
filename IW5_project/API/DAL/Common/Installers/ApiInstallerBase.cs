using IW5_project.API.DAL.Common.Mappers;
using IW5_project.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace IW5_project.API.DAL.Common.Installers
{
    public abstract class ApiDALInstallerBase : IInstaller
    {
        public virtual void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<PlaceMapper>();
            // add other mappers here
        }
    }
}
