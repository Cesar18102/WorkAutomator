using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using MethodBoundaryAspect.Fody.Attributes;

using WorkAutomatorServer.Output;
using WorkAutomatorServer.Exceptions;
using WorkAutomatorServer.Controllers;

namespace WorkAutomatorServer.Aspects
{
    public class RequiredMaskAspect : OnMethodBoundaryAspect
    {
        public string[] Requireds { get; set; }

        public RequiredMaskAspect(params string[] requireds)
        {
            Requireds = requireds;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Collection<HttpParameterDescriptor> parameterDescriptors = (args.Instance as ControllerBase).Request.GetActionDescriptor().GetParameters();

            foreach (string required in Requireds)
            {
                int countPresented = 0;
                string[] xoredExpressions = required.Split('^');

                foreach (string expression in xoredExpressions)
                {
                    string[] parameterPath = expression.Split('.');

                    HttpParameterDescriptor parameter = parameterDescriptors.First(param => param.ParameterName == parameterPath[0]);

                    if (parameter == null)
                    {
                        RaiseValidationException(args, "Al least one of xored required fields must be presented");
                        return;
                    }

                    int parameterPosition = parameterDescriptors.IndexOf(parameter);
                    if (IsValuePresented(parameterPath.Skip(1), args.Arguments[parameterPosition]))
                        ++countPresented;

                    if (countPresented > 1)
                    {
                        RaiseValidationException(args, "Only one of xored required fields must be presented");
                        return;
                    }
                }

                if(countPresented == 0)
                {
                    RaiseValidationException(args, "Al least one of xored required fields must be presented");
                    return;
                }
            }
        }

        private void RaiseValidationException(MethodExecutionArgs args, string message)
        {
            Response response = new Response();

            response.Error = new ErrorPart(
                ControllerBase.ErrorStatusCodes.Keys.ToList().IndexOf(typeof(ValidationException)) + 1,
                new ValidationException(message)
            );

            Task<HttpResponseMessage> responseMessage = Task.Run(() => (args.Instance as ControllerBase).Request.CreateResponse(
                ControllerBase.ErrorStatusCodes[typeof(ValidationException)], response
            ));

            args.ReturnValue = responseMessage;
            args.FlowBehavior = FlowBehavior.Return;
        }

        private bool IsValuePresented(IEnumerable<string> requiredPath, object context)
        {
            if (requiredPath.Count() == 0)
                return context != null;
            
            if (context is IEnumerable<object> collection)
            {
                foreach (object item in collection)
                    if (!IsValuePresented(requiredPath, item))
                        return false;

                return true;
            }
            else
            {
                PropertyInfo nextProperty = context.GetType().GetProperties().First(
                    property => property.Name == requiredPath.First()
                );

                object value = nextProperty.GetValue(context);

                if (value == null)
                    return false;

                return IsValuePresented(requiredPath.Skip(1), value);
            }
        }
    }
}
