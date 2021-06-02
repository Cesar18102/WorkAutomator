using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IAccountService
    {
        Task<WorkerModel> Get(AuthorizedDto<AccountDto> dto);
        Task<AccountModel> UpdateProfile(AuthorizedDto<AccountDto> dto);
        Task<WorkerModel> AddBoss(AuthorizedDto<SetRemoveBossDto> dto);
        Task<WorkerModel> RemoveBoss(AuthorizedDto<SetRemoveBossDto> dto);
    }
}
