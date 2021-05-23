using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkAutomatorDataAccess.Exceptions
{
    public class DatabaseActionValidationException : Exception
    {
        public Type RepoType { get; set; }
        public Type EntityType { get; set; }

        public IEnumerable<ValidationResult> Errors { get; private set; }

        public DatabaseActionValidationException(IEnumerable<ValidationResult> errors)
        {
            Errors = errors;
        }
    }
}
