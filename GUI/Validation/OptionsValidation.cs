using System;
using System.IO;
using System.Text.RegularExpressions;
using Filewatcher.GUI.Properties;
using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    public class OptionsValidation
    {
        internal OptionsValidation(ValidationBase validationBase)
        {
            _validationBase = validationBase;
        }

        private readonly ValidationBase _validationBase;

        internal void Bind(bool bind)
        {
            ValidateOutputPathBinding(bind);
        }

        private void ValidateOutputPathBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Options>.GetProperty(x => x.OutputPath).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value =>
                    {
                        _validationBase.ValidateProperty(propertyName, value, Resources.MAIN_ERROR_OUTPUTPATH,
                            val =>    String.IsNullOrEmpty(value.ToString())
                                   || !Directory.Exists(Path.GetDirectoryName(val.ToString()))
                                   || !Regex.IsMatch(val.ToString(), @".*\\\w*[.]\w*"));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        public static OptionsValidation ValidateOptions(Options options, bool bind = true)
        {
            var optionsValidation = new OptionsValidation(options);
            optionsValidation.Bind(bind);
            return optionsValidation;
        }
    }
}
