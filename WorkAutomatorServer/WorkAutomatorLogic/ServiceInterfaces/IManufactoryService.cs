using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IManufactoryService
    {
        Task<ManufactoryModel> CreateManufactory(AuthorizedDto<ManufactoryDto> manufactory);
    }
}
