using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using Komi.Services.Core.Attributes;
using Komi.Services.Utilities;
using ParameterInfo = Discord.Commands.ParameterInfo;

namespace Komi.Services.Help
{
    public class ParameterHelpData
    {
        public string Name { get; set; }

        public string? Summary { get; set; }

        public string? Type { get; set; }

        public bool IsOptional { get; set; }

        public IReadOnlyCollection<ParameterHelpData>? Options { get; set; }

        public static ParameterHelpData FromParameterInfo(ParameterInfo parameter)
        {
            (string typeName, bool isNullable) = GetTypeInfo(parameter.Type);
            string name = parameter.Name;
            string summary = parameter.Summary;
            var options = parameter.Type switch
            {
                var t when t.IsEnum => FromEnum(t.GetEnumValues()),
                var t when t.GetAttribute<NamedArgumentTypeAttribute>() != null =>
                FromNamedArgumentInfo(parameter.Type),
                _ => null
            };

            return new ParameterHelpData(name, summary, typeName, isNullable || parameter.IsOptional, options);
        }

        private ParameterHelpData(
            string name,
            string? summary = null,
            string? type = null,
            bool isOptional = false,
            IReadOnlyCollection<ParameterHelpData>? options = null)
        {
            Name = name;
            Summary = summary;
            Type = type;
            IsOptional = isOptional;
            Options = options;
        }

        private static IReadOnlyCollection<ParameterHelpData> FromEnum(IEnumerable names)
        {
            var result = new List<ParameterHelpData>();

            foreach (Enum? n in names)
            {
                if (n is null)
                    continue;

                string name = n.ToString();
                string summary = n.GetAttributeOfEnum<DescriptionAttribute>()?.Text;
                result.Add(new ParameterHelpData(name, summary));
            }

            return result;
        }

        private static IReadOnlyCollection<ParameterHelpData> FromNamedArgumentInfo(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return properties.Select(
                    p =>
                    {
                        (string typeName, bool isNullable) = GetTypeInfo(type);

                        return new ParameterHelpData(
                            p.Name, p.GetAttribute<DescriptionAttribute>()?.Text, typeName, isNullable);
                    })
               .ToList();
        }

        private static ValueTuple<string?, bool> GetTypeInfo(Type type)
        {
            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            var paramType = isNullable ? type.GetGenericArguments()[0] : type;
            string typeName = paramType.Name;
            if (paramType.IsInterface && paramType.Name.StartsWith('I'))
                typeName = typeName.Substring(1);

            return new ValueTuple<string?, bool>(typeName, isNullable);
        }
    }
}