using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Filewatcher.BLL;
using Filewatcher.MDL;
using P = Filewatcher.GUI.Properties.Resources;

namespace Filewatcher.GUI
{
    internal class WatchValidation
    {
        internal WatchValidation(ValidationBase validationBase)
        {
            _validationBase = validationBase;
        }

        private readonly ValidationBase _validationBase;

        internal void Bind(bool bind)
        {
            ValidateNameBinding(bind);
            ValidateWatchFolderBinding(bind);
            ValidateWorkingFolderBinding(bind);
            ValidateProcessBinding(bind);
            ValidateOutputPathBinding(bind);
            ValidateExtensionBinding(bind);
            ValidateExcludeBinding(bind);
            ValidateFilterBinding(bind);
        }

        private void ValidateNameBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.Name).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value =>
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_NAME_ASCII,
                            val => Regex.IsMatch(val.ToString(), "[^\x00-\x7F]"));
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_NAME_EMPTY,
                            val => String.IsNullOrEmpty(value.ToString()));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateWatchFolderBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.WatchFolder).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value => {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_FOLDER, 
                            val => !Directory.Exists(val.ToString()));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateWorkingFolderBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.WorkingFolder).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value =>
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_FOLDER,
                            val => !Directory.Exists(val.ToString()));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateProcessBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.Process).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value => 
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_PROCESS, 
                            val => !File.Exists(val.ToString())
                                || (val.ToString().Contains(@"/") && val.ToString().Contains(@"\")));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateExtensionBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.IncludeExtension).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value => 
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_EXTENSION, 
                            val => !val.ToString().Contains("."));
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateOutputPathBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.OutputPath).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value =>
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_OUTPUTPATH,
                            val => {
                                if (string.IsNullOrEmpty(val.ToString()))
                                    return false;
                                var path = Parameter.Parse(val.ToString(), @"c:\Test.txt");
                                try {
                                    var fileName = Path.GetFileName(path);
                                    if (string.IsNullOrEmpty(fileName))
                                        return true;
                                    if (!Path.GetInvalidFileNameChars().Any(fileName.Contains) 
                                    &&  Directory.Exists(Path.GetDirectoryName(path)))
                                        return false;
                                }
                                catch (Exception e) {}
                                return true;
                            });
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateExcludeBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.ExcludeExtension).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.AddPropertyAction(key, value =>
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_EXCLUDEEXTENSION,
                            val =>
                            {
                                if (string.IsNullOrEmpty(val.ToString()))
                                    return false;
                                try
                                {
                                    new Regex(val.ToString()).IsMatch("Test");
                                    return false;
                                }
                                catch (Exception e) { }
                                return true;
                            });
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        private void ValidateFilterBinding(bool bind = true)
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.Filter).Name;
            var key = PropertyHelper.GetCallerMemberName();
            if (bind)
            {
                if (!_validationBase.ContainsPropertyCondition(key, propertyName))
                    _validationBase.AddPropertyCondition(key, value => 
                    {
                        _validationBase.ValidateProperty(propertyName, value, P.WATCH_ERROR_FILTER, 
                            val => (NotifyFilters)val <= 0);
                        return true;
                    }, propertyName);
            }
            else
                if (_validationBase.ContainsPropertyAction(key, propertyName))
                    _validationBase.RemovePropertyAction(key, propertyName);
        }

        internal static void ValidateWatches(IEnumerable<Watch> watches, bool bind = true)
        {
            foreach (var watch in watches)
                ValidateWatch(watch, bind);
        }

        private static WatchValidation ValidateWatch(Watch watch, bool bind = true)
        {
            var watchValidation = new WatchValidation(watch);
            watchValidation.Bind(bind);
            return watchValidation;
        }
    }
}
