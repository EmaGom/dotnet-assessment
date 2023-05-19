using TimeChimp.Backend.Assessment.Enums;
using TimeChimp.Backend.Assessment.Repositories;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public interface IDataAccessLayerFactory
    {
        IDataAccessLayer GetInstance(DataAccessLayerEnum type);
    }
}
