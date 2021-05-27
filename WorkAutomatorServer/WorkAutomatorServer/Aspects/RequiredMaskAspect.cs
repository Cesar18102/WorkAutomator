using System;
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
                string[] parameterPath = required.Split('.');

                HttpParameterDescriptor parameter = parameterDescriptors.First(param => param.ParameterName == parameterPath[0]);

                if (parameter == null)
                {
                    RaiseValidationException(args);
                    return;
                }

                int parameterPosition = parameterDescriptors.IndexOf(parameter);

                CheckValuePresented(parameterPath.Skip(1), args.Arguments[parameterPosition], args);
            }
        }

        private void RaiseValidationException(MethodExecutionArgs args)
        {
            Response response = new Response()
            {
                Error = new ErrorPart(
                    ControllerBase.ErrorStatusCodes.Keys.ToList().IndexOf(typeof(ValidationException)) + 1, 
                    new ValidationException("All IdDto inheritors must specify id field in indentified-market requests")
                )
            };

            Task<HttpResponseMessage> responseMessage = Task.Run(() => (args.Instance as ControllerBase).Request.CreateResponse(
                ControllerBase.ErrorStatusCodes[typeof(ValidationException)], response
            ));

            args.ReturnValue = responseMessage;
            args.FlowBehavior = FlowBehavior.Return;
        }

        private void CheckValuePresented(IEnumerable<string> requiredPath, object context, MethodExecutionArgs args)
        {
            if (requiredPath.Count() == 0)
            {
                if (context == null)
                    RaiseValidationException(args);

                return;
            }

            Type type = context.GetType();

            if (context is IEnumerable<object> collection)
            {
                foreach (object item in collection)
                    CheckValuePresented(requiredPath, item, args);
            }
            else
            {
                PropertyInfo nextProperty = type.GetProperties().First(
                    property => property.Name == requiredPath.First()
                );

                object value = nextProperty.GetValue(context);

                if (value == null)
                {
                    RaiseValidationException(args);
                    return;
                }

                CheckValuePresented(requiredPath.Skip(1), value, args);
            }
        }
    }
}
