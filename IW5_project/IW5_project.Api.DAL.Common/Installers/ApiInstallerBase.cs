using IW5_project.Api.DAL.Common.Mappers;
using IW5_project.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IW5_project.Api.DAL.Common.Installers
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
