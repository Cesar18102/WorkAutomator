using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Autofac;

using MethodBoundaryAspect.Fody.Attributes;

using WorkAutomatorLogic;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Controllers;
using WorkAutomatorServer.Dto;

namespace WorkAutomatorServer.Aspects
{
    public class AuthorizedAspect : OnMethodBoundaryAspect
    {
        private ISessionService SessionService = LogicDependencyHolder.Dependencies.Resolve<ISessionService>();

        public override void OnEntry(MethodExecutionArgs args)
        {
            SessionDto session = args.Arguments.FirstOrDefault(arg => arg is SessionDto) as SessionDto;

            if (session == null)
            {
                object authorizedDto = args.Arguments.FirstOrDefault(
                   arg => arg != null && typeof(AuthorizedDto<>).IsAssignableFrom(arg.GetType())
                );

                session = authorizedDto?.GetType().GetProperty(
                    nameof(AuthorizedDto<IdDto>.Session)
                ).GetValue(authorizedDto) as SessionDto;
            }

            SessionCredentialsModel sessionModel = session?.ToModel<SessionCredentialsModel>();

            Task<HttpResponseMessage> response = (args.Instance as ControllerBase).Execute(
                (model) => SessionService.CheckSession(model), sessionModel, false
            );
            
            if(response != null)
            {
                args.ReturnValue = response;
                args.FlowBehavior = FlowBehavior.Return;
            }
            else
            {
                args.FlowBehavior = FlowBehavior.Continue;
            }

            base.OnEntry(args);
        }
    }
}
