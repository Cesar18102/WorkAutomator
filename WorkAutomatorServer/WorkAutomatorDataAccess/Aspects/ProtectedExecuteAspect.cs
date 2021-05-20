﻿using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using WorkAutomatorDataAccess.Repos;
using WorkAutomatorDataAccess.Exceptions;

using MethodBoundaryAspect.Fody.Attributes;

namespace WorkAutomatorDataAccess.Aspects
{
    public sealed class ProtectedExecuteAspect : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Type instanceType = args.Instance.GetType();

            while (!instanceType.IsGenericType)
                instanceType = instanceType.BaseType;

            if (!typeof(RepoBase<>).IsAssignableFrom(instanceType.GetGenericTypeDefinition()))
                return;

            if (args.Exception is DatabaseActionValidationException ex)
            {
                Type entityType = instanceType.GetGenericArguments()[0];
                InvalidDataException invalidDataException = new InvalidDataException();

                foreach (ValidationResult validationResult in ex.Errors)
                {
                    if(validationResult.MemberNames.Count() == 0)
                    {
                        InvalidFieldInfo info = new InvalidFieldInfo(entityType, "", validationResult.ErrorMessage);
                        invalidDataException.InvalidFieldInfos.Add(info);
                    }

                    foreach (string invalidFieldName in validationResult.MemberNames)
                    {
                        InvalidFieldInfo info = new InvalidFieldInfo(
                            entityType, invalidFieldName, 
                            validationResult.ErrorMessage
                        );

                        invalidDataException.InvalidFieldInfos.Add(info);
                    }
                }

                args.Exception = invalidDataException;
            }
        }
    }
}
