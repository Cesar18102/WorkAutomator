using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using MethodBoundaryAspect.Fody.Attributes;

using Attributes;

using WorkAutomatorServer.Controllers;
using WorkAutomatorServer.Exceptions;
using WorkAutomatorServer.Output;

namespace WorkAutomatorServer.Aspects
{
    public class WireHeadersAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            HttpRequestMessage request = (args.Instance as ControllerBase).Request;

            try
            {
                WireHeaders(request, args.Arguments.First());
                args.FlowBehavior = FlowBehavior.Continue;
            }
            catch (ValidationException ex)
            {
                Response response = new Response()
                {
                    Error = new ErrorPart(
                        ControllerBase.ErrorStatusCodes.Keys.ToList().IndexOf(
                            typeof(ValidationException)
                        ) + 1, ex
                    )
                };

                Task<HttpResponseMessage> responseMessage = Task.Run(() => request.CreateResponse(
                    ControllerBase.ErrorStatusCodes[typeof(ValidationException)], response
                ));

                args.ReturnValue = responseMessage;
                args.FlowBehavior = FlowBehavior.Return;
            }

            base.OnEntry(args);
        }

        private void WireHeaders(HttpRequestMessage request, object obj)
        {
            if (obj == null || obj.GetType().IsPrimitive)
                return;

            PropertyInfo[] properties = obj.GetType().GetProperties()
                .Where(property => property.GetCustomAttribute<HeaderAutoWiredAttribute>() != null)
                .ToArray();

            foreach (PropertyInfo property in properties)
            {
                HeaderAutoWiredAttribute attr = property.GetCustomAttribute<HeaderAutoWiredAttribute>();

                if (property.PropertyType.IsPrimitive || property.PropertyType.Equals(typeof(string)))
                {
                    string header = GetHeaderOrCookie(request, attr.HeaderName, attr.ThrowIfNotPresented);
                    try
                    {
                        object value = Convert.ChangeType(header, property.PropertyType);
                        property.SetValue(obj, value);
                    }
                    catch { throw new ValidationException($"Header {attr.HeaderName} is invalid"); }
                }
                else
                {
                    object value = property.GetValue(obj);

                    if (value == null)
                    {
                        value = property.PropertyType.GetConstructor(new Type[] { })
                            .Invoke(new object[] { });

                        property.SetValue(obj, value);
                    }

                    WireHeaders(request, value);
                }
            }
        }

        private string GetHeaderOrCookie(HttpRequestMessage request, string name, bool throwIfNotPresented)
        {
            string header = null;
            if (!request.Headers.TryGetValues(name, out IEnumerable<string> tokens))
            {
                Collection<CookieHeaderValue> cookies = request.Headers.GetCookies();
                if (cookies != null && cookies.Count > 0 && cookies[0][name] != null)
                    header = cookies[0][name].Value;
            }
            else
                header = tokens.FirstOrDefault();

            if (header == null && throwIfNotPresented)
                throw new ValidationException($"{name} was not presented");

            return header;
        }
    }
}