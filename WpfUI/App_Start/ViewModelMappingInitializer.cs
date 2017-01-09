using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.App_Start
{
    // http://www.ikriv.com/dev/wpf/DataTemplateCreation/
    internal static class ViewModelMappingInitializer
    {
        private const string VIEW_SUFFIX = "View";
        private const string VIEWMODEL_SUFFIX = "ViewModel";

        internal static void Initialize()
        {
            var resourceDictionary = CreateRawResourceDictionary();
            ApplyDataTemplates(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private static void ApplyDataTemplates(ResourceDictionary resourceDictionary)
        {
            var viewModelViewMappings = CreateViewModelViewMappings();

            foreach (var mapping in viewModelViewMappings)
            {
                var dt = CreateDataTemplate(mapping.ViewModelType, mapping.ViewType);
                resourceDictionary.Add(dt.DataTemplateKey, dt);
            }
        }

        private static DataTemplate CreateDataTemplate(Type viewModelType, Type viewType)
        {
            const string XAML_TEMPLATE = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";

            var xaml = string.Format(XAML_TEMPLATE, viewModelType.Name, viewType.Name);

            var context = new ParserContext();
            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add(string.Empty, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);

            return template;
        }

        private static ResourceDictionary CreateRawResourceDictionary()
        {
            const string XAML_TEMPLATE = "<ResourceDictionary></ResourceDictionary>";

            var context = new ParserContext();
            context.XmlnsDictionary.Add(string.Empty, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            var result = (ResourceDictionary)XamlReader.Parse(XAML_TEMPLATE, context);
            return result;
        }

        private static IEnumerable<ViewViewModelMapping> CreateViewModelViewMappings()
        {
            var result = new List<ViewViewModelMapping>();
            var viewModelTypes = ReflectionHelper.GetViewModels();
            var viewTypes = ReflectionHelper.GetViews();

            foreach (var viewModelType in viewModelTypes)
            {
                var mapping = new ViewViewModelMapping(viewTypes[viewModelType.NormalizedName].Type, viewModelType.Type);
                result.Add(mapping);
            }

            return result;
        }

        private static class ReflectionHelper
        {
            internal static TypeWithNormalizedNameList GetViewModels()
            {
                var viewModelTypes = Assembly.GetExecutingAssembly().GetTypes().Where(
                    f => typeof(ViewModelBase).IsAssignableFrom(f) &&
                         !f.IsAbstract &&
                         f.Name.EndsWith(VIEWMODEL_SUFFIX)).ToList();

                return MapToNormalizedNames(viewModelTypes, VIEWMODEL_SUFFIX);
            }

            internal static TypeWithNormalizedNameList GetViews()
            {
                var viewTypes = Assembly.GetExecutingAssembly().GetTypes().Where(
                    f => typeof(UserControl).IsAssignableFrom(f) &&
                         f.Name.EndsWith(VIEW_SUFFIX)).ToList();

                return MapToNormalizedNames(viewTypes, VIEW_SUFFIX);
            }

            private static TypeWithNormalizedNameList MapToNormalizedNames(IEnumerable<Type> types, string suffix)
            {
                var result = new TypeWithNormalizedNameList();

                foreach (var type in types)
                {
                    var normalizedName = type.Name.Replace(suffix, string.Empty);
                    result.Add(new TypeWithNormalizedName(type, normalizedName));
                }

                return result;
            }
        }

        private struct TypeWithNormalizedName
        {
            public TypeWithNormalizedName(Type type, string normalizedName)
            {
                Type = type;
                NormalizedName = normalizedName;
            }

            public string NormalizedName { get; }

            public Type Type { get; }
        }

        private class TypeWithNormalizedNameList : List<TypeWithNormalizedName>
        {
            public TypeWithNormalizedName this[string normalizedName]
            {
                get
                {
                    return this.First(f => f.NormalizedName == normalizedName);
                }
            }
        }

        private struct ViewViewModelMapping
        {
            public ViewViewModelMapping(Type viewType, Type viewModelType)
            {
                ViewModelType = viewModelType;
                ViewType = viewType;
            }

            public Type ViewModelType { get; }

            public Type ViewType { get; }
        }
    }
}