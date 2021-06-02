using System.Threading.Tasks;

using Dto;
using Dto.Tasks;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ITaskService
    {
        Task<TaskModel> Create(AuthorizedDto<TaskDto> dto);
        Task<TaskModel> Assign(AuthorizedDto<AssignTaskDto> dto);
        Task<TaskModel> NotifyDone(AuthorizedDto<TaskDto> dto);
        Task<TaskModel> NotifyReviewed(AuthorizedDto<ReviewTaskDto> dto);
    }
}
