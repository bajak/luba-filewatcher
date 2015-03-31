using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filewatcher.MDL
{
    public abstract class ValidationBase : PropertyBase, IDataErrorInfo
    {
        protected ValidationBase()
        {
            Errors = new ObservableDictionary<string, List<string>>();
        }

        [NotMapped]
        public ObservableDictionary<string, List<String>> Errors {get; private set;}

        public bool ValidateProperty(string propertyName, object value, string error, Func<object, bool> validator)
        {
            var isValid = true;

            if (validator(value))
            {
                AddError(propertyName, error, false);
                isValid = false;
            }
            else 
                RemoveError(propertyName, error);

            return isValid;
        }

        protected void AddError(string propertyName, string error, bool isWarning)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors[propertyName] = new List<string>();

            if (Errors[propertyName].Contains(error)) return;

            if (isWarning) Errors[propertyName].Add(error);
            else Errors[propertyName].Insert(0, error);
        }

        protected void RemoveError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName) || !Errors[propertyName].Contains(error)) return;

            Errors[propertyName].Remove(error);
            if (Errors[propertyName].Count == 0) Errors.Remove(propertyName);
        }

        [NotMapped]
        public string this[string propertyName]
        {
            get
            {
                return (!Errors.ContainsKey(propertyName) ? null :
                    String.Join(Environment.NewLine, Errors[propertyName]));
            }
        }

        [NotMapped]
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
