using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Autofac;

using MethodBoundaryAspect.Fody.Attributes;

using Dto;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorServer.Controllers;

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
                   arg => arg != null && typeof(AuthorizedDto<>).IsAssignableFrom(arg.GetType().GetGenericTypeDefinition())
                );

                session = authorizedDto?.GetType().GetProperty(
                    nameof(AuthorizedDto<IdDto>.Session)
                ).GetValue(authorizedDto) as SessionDto;
            }

            HttpResponseMessage response = Task.Run(() => (args.Instance as ControllerBase).Execute(
                model => SessionService.CheckSession(model), session, false
            )).GetAwaiter().GetResult();
            
            if(response != null)
            {
                args.ReturnValue = Task.Run(() => response);
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
