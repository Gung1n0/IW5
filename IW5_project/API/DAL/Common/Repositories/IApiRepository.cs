using System;
using System.Collections.Generic;
using IW5_project.API.DAL.Common.Entities.Interfaces;

namespace IW5_project.API.DAL.Common.Repositories
{
    public interface IApiRepository
    {
        public interface IApiRepository<TEntity>
        where TEntity : IEntity
        {
            IList<TEntity> GetAll();
            TEntity? GetById(Guid id);
            Guid Insert(TEntity entity);
            Guid? Update(TEntity entity);
            void Remove(Guid id);
            bool Exists(Guid id);
        }
    }
}
