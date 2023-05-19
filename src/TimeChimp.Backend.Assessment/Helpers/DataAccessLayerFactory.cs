using System.Collections.Generic;
using System;
using TimeChimp.Backend.Assessment.Repositories;
using System.Linq;
using TimeChimp.Backend.Assessment.Enums;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public class DataAccessLayerFactory : IDataAccessLayerFactory
    {
        private readonly IEnumerable<IDataAccessLayer> dataAccessLayer;

        public DataAccessLayerFactory(IEnumerable<IDataAccessLayer> dataAccessLayer)
        {
            this.dataAccessLayer = dataAccessLayer;
        }

        public IDataAccessLayer GetInstance(DataAccessLayerEnum type)
        {
            return type switch
            {
                DataAccessLayerEnum.EntityFramework => this.GetDataAccessLayer(typeof(DataAccessLayerEF)),
                DataAccessLayerEnum.Dapper => this.GetDataAccessLayer(typeof(DataAccessLayerDapper)),
                _ => throw new InvalidOperationException()
            }; ;
        }

        private IDataAccessLayer GetDataAccessLayer(Type type)
        {
            return this.dataAccessLayer.FirstOrDefault(x => x.GetType() == type)!;
        }
    }
}
