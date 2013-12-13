﻿using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.ServiceModelMappers
{
    public interface IServiceModelMapper<in TEntity, in TModel> where TEntity : Entity
                                                             where TModel : ServiceModel, new()
    {
        void Map(TEntity entity, TModel model);
    }
}