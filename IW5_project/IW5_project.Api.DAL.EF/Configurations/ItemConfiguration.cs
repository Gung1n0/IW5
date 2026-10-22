using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IW5_project.IW5_project.Api.DAL.EF.Configurations
{
    public class ItemConfiguration
    {
        public void Configure(EntityTypeBuilder<ItemEntity> EntityBuilder)
        {
            EntityBuilder.Property(i => i.Name).IsRequired().HasMaxLength(Constants.MAX_NAME_LENGTH);
            EntityBuilder.Property(i => i.Description).IsRequired().HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
            EntityBuilder.Property(i => i.ImageUrl).HasMaxLength(Constants.MAX_IMAGE_URL_LENGTH);
        }
    }
}
