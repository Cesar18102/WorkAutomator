using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net;
using System.Net.Http;

using System.Web.Http;
using System.Web.Http.ModelBinding;

using Attributes;

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

            { typeof(DataValidationException), HttpStatusCode.BadRequest },
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

            { typeof(PointsBusyException), HttpStatusCode.Conflict },
            { typeof(InvalidPointsException), HttpStatusCode.BadRequest },
            { typeof(PlacementException), HttpStatusCode.BadRequest },
            { typeof(DataTypeException), HttpStatusCode.BadRequest }
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

            object[] base64Objects = parameter.GetMarkedValues<Base64Attribute>();
            foreach(object base64Object in base64Objects)
            {
                if (base64Object == null)
                    continue;

                try { Convert.FromBase64String(base64Object.ToString()); }
                catch { errors.Add("Not base64 string"); }
            }

            foreach (KeyValuePair<string, ModelState> fieldState in ModelState)
                foreach (ModelError error in fieldState.Value.Errors)
                    errors.Add(error.ErrorMessage);

            if (errors.Count != 0)
                throw new ValidationException(errors);
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
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new Response() { Error = new ErrorPart(500, ex)}
                );
            }
        }
    }
}