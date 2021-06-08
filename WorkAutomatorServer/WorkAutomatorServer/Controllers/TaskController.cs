using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using Dto;
using Dto.Tasks;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Aspects;
using System.Web.Http.Cors;

namespace WorkAutomatorServer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class TaskController : ControllerBase
    {
        private static ITaskService TaskService = LogicDependencyHolder.Dependencies.Resolve<ITaskService>();

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.CompanyId")]
        public async Task<HttpResponseMessage> Create([FromBody] AuthorizedDto<TaskDto> dto)
        {
            return await Execute(task => TaskService.Create(task), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> Assign([FromBody] AuthorizedDto<AssignTaskDto> dto)
        {
            return await Execute(task => TaskService.Assign(task), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> NotifyDone([FromBody] AuthorizedDto<TaskDto> dto)
        {
            return await Execute(task => TaskService.NotifyDone(task), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        public async Task<HttpResponseMessage> GetMy([FromBody] AuthorizedDto<IdDto> dto)
        {
            return await Execute(task => TaskService.GetMyTasks(task), dto);
        }

        [HttpPost]
        [WireHeadersAspect]
        [AuthorizedAspect]
        [RequiredMaskAspect("dto.Data.Id")]
        public async Task<HttpResponseMessage> NotifyReviewed([FromBody] AuthorizedDto<ReviewTaskDto> dto)
        {
            return await Execute(task => TaskService.NotifyReviewed(task), dto);
        }
    }
}
