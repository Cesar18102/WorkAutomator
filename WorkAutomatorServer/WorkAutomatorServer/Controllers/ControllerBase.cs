using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Net;
using System.Net.Http;

using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

using Newtonsoft.Json.Linq;

using Attributes;
using Dto;

using WorkAutomatorLogic.Exceptions;

using WorkAutomatorServer.Output;
using WorkAutomatorServer.Exceptions;

namespace WorkAutomatorServer.Controllers
{
    public abstract class ControllerBase : ApiController
    {
        internal static Dictionary<Type, HttpStatusCode> ErrorStatusCodes = new Dictionary<Type, HttpStatusCode>()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },

            { typeof(NotFoundException), HttpStatusCode.NotFound },

            { typeof(LoginDuplicationException), HttpStatusCode.Conflict },
            { typeof(InvalidKeyException), HttpStatusCode.Unauthorized },
            { typeof(PostValidationException), HttpStatusCode.BadRequest },
            { typeof(WrongEncryptionException), HttpStatusCode.BadRequest },
            { typeof(InvalidPasswordException), HttpStatusCode.BadRequest },

            { typeof(WrongPasswordException), HttpStatusCode.Unauthorized },
            { typeof(WrongSessionTokenException), HttpStatusCode.Unauthorized },
            { typeof(SessionExpiredException), HttpStatusCode.Unauthorized },

            { typeof(NotPermittedException), HttpStatusCode.Forbidden },
            { typeof(AlreadyHiredException), HttpStatusCode.Conflict },
            { typeof(NotHiredException), HttpStatusCode.NotFound },


            //{ typeof(NotFoundException), HttpStatusCode.NotFound },
            //{ typeof(NotAppropriateRoleException), HttpStatusCode.Forbidden },
            //{ typeof(MembershipDuplicationException), HttpStatusCode.Conflict },
            //{ typeof(MembershipNotFoundException), HttpStatusCode.NotFound },
            //{ typeof(AlreadyInsideException), HttpStatusCode.Conflict },
            //{ typeof(NotInsideException), HttpStatusCode.NotFound },
            //{ typeof(PointNotFoundException), HttpStatusCode.NotFound },
            //{ typeof(ForbiddenActionException), HttpStatusCode.Forbidden },
        };

        public async Task<HttpResponseMessage> Execute<Tin>(Action<Tin> executor, Tin parameter, bool createResponseOnSuccess = true)
        {
            Func<object> func = new Func<object>(() =>
            {
                executor(parameter);
                return null;
            });

            Func<Task<object>> task = new Func<Task<object>>(
                () => Task.Run(func)
            );

            return await ProtectedExecute(task, parameter, true, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute<Tin>(Func<Tin, Task> executor, Tin parameter, bool createResponseOnSuccess = true)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor(parameter);
                    return null;
                }
            );

            return await ProtectedExecute(task, parameter, true, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Func<Tout> executor, bool createResponseOnSuccess = true)
        {
            return await ProtectedExecute(() => Task.Run(executor), null, false, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute(Func<Task> executor, bool createResponseOnSuccess = true)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor();
                    return null;
                }
            );

            return await ProtectedExecute(task, null, false, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Func<Task<Tout>> executor, bool createResponseOnSuccess = true)
        {
            return await ProtectedExecute(executor, null, false, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute<Tin, Tout>(Func<Tin, Tout> executor, Tin parameter, bool createResponseOnSuccess = true)
        {
            Func<Task<Tout>> task = new Func<Task<Tout>>(
                () => Task.Run(
                    () => executor(parameter)
                )
            );

            return await ProtectedExecute(task, parameter, true, createResponseOnSuccess);
        }

        public async Task<HttpResponseMessage> Execute<Tin, Tout>(Func<Tin, Task<Tout>> executor, Tin parameter, bool createResponseOnSuccess = true)
        {
            return await ProtectedExecute(() => executor(parameter), parameter, true, createResponseOnSuccess);
        }

        protected virtual void ValidateModel(object parameter, bool mustHaveParameter = true)
        {
            if (mustHaveParameter && parameter == null)
                throw new ValidationException("no data passed");

            ICollection<string> errors = new List<string>();
            foreach (KeyValuePair<string, ModelState> fieldState in ModelState)
                foreach (ModelError error in fieldState.Value.Errors)
                    errors.Add(error.ErrorMessage);

            Collection<HttpParameterDescriptor> parameterDescriptors = Request.GetActionDescriptor().GetParameters();
            if (parameterDescriptors != null)
            {
                foreach (HttpParameterDescriptor parameterDescriptor in parameterDescriptors)
                {
                    if (parameterDescriptor.GetCustomAttributes<IdentifiedAttribute>().Count == 0)
                        continue;

                    object parameterValue = ActionContext.ActionArguments[parameterDescriptor.ParameterName];

                    if (parameterValue == null || !CheckIdentified(parameterValue))
                    {
                        errors.Add("All IdDto inheritors must specify id field in indentified-market requests");
                        break;
                    }
                }
            }

            if (errors.Count != 0)
                throw new ValidationException(errors);
        }

        private bool CheckIdentified(object obj)
        {
            if (obj is IdDto idObj && !idObj.Id.HasValue)
                return false;

            if (obj is JObject)
                return true;

            Type type = obj.GetType();
            if (obj is IEnumerable<object> collection)
            {
                if (!CheckTypeContainsIdentified(type.GetGenericArguments()[0]))
                    return true;

                return collection.All(item => CheckIdentified(item));
            }

            PropertyInfo[] properties = type.GetProperties().Where(
                property => !property.PropertyType.IsValueType && !property.PropertyType.Equals(typeof(string))
            ).ToArray();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value == null)
                {
                    if (property.PropertyType.IsGenericType)
                        continue;

                    if (CheckTypeContainsIdentified(property.PropertyType))
                        return false;
                }
                else if (!CheckIdentified(value))
                    return false;
            }

            return true;
        }

        private bool CheckTypeContainsIdentified(Type type)
        {
            if (typeof(IdDto).IsAssignableFrom(type))
                return true;

            return type.GetProperties().Where(
                property => !property.PropertyType.IsValueType && !property.PropertyType.Equals(typeof(string))
            ).Any(
                property => CheckTypeContainsIdentified(property.PropertyType)
            );
        }

        private HttpResponseMessage CreateErrorResponse(Exception ex)
        {
            Type type = ex.GetType();
            int index = ErrorStatusCodes.Keys.ToList().IndexOf(type);

            if (index != -1)
            {
                Response response = new Response() { Error = new ErrorPart(index + 1, ex) };
                return Request.CreateResponse(ErrorStatusCodes[type], response);
            }

            //log it
            throw ex;
        }
        private HttpResponseMessage CreateResponse(object data)
        {
            Response response = new Response() { Data = data };
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private async Task<HttpResponseMessage> ProtectedExecute<TOut>(Func<Task<TOut>> task, object parameter, bool mustHaveParameter, bool createResponseOnSuccess)
        {
            try
            {
                ValidateModel(parameter, mustHaveParameter);
                TOut result = await task();
                return createResponseOnSuccess ? CreateResponse(result) : null;
            }
            catch (ServerExceptionBase ex) { return CreateErrorResponse(ex); }
            catch (LogicExceptionBase ex) { return CreateErrorResponse(ex); }
        }
    }
}