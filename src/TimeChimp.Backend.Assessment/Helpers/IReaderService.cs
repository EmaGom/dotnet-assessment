using System.ServiceModel.Syndication;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public interface IReaderService
    {
        Category Read(string categoryName);
    }
}
