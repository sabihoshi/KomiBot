using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using KomiBot.TypeReaders;
using ParameterInfo = Discord.Commands.ParameterInfo;

namespace KomiBot.Services.Help
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
            var (typeName, isNullable) = GetTypeInfo(parameter.Type);
            var name = parameter.Name;
            var summary = parameter.Summary;
            var options = parameter.Type.IsEnum
                ? FromEnum(parameter.Type.GetEnumNames())
                : FromNamedArgumentInfo(parameter.Type);

            return new ParameterHelpData(name, summary, typeName, isNullable || parameter.IsOptional, options);
        }

        private ParameterHelpData(string name, string? summary = null, string? type = null, bool isOptional = false,
            IReadOnlyCollection<ParameterHelpData>? options = null)
        {
            Name = name;
            Summary = summary;
            Type = type;
            IsOptional = isOptional;
            Options = options;
        }

        private static IReadOnlyCollection<ParameterHelpData> FromEnum(string[] names)
        {
            return names.Select(n => new ParameterHelpData(n)).ToList();
        }

        private static IReadOnlyCollection<ParameterHelpData>? FromNamedArgumentInfo(Type type)
        {
            if (type.GetCustomAttribute(typeof(NamedArgumentTypeAttribute)) is null)
                return null;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return properties.Select(p =>
            {
                var (typeName, isNullable) = GetTypeInfo(type);

                return new ParameterHelpData(p.Name, GetSummary(p), typeName, isNullable);
            }).ToList();
        }

        private static ValueTuple<string?, bool> GetTypeInfo(Type type)
        {
            var isNullable = type.IsGenericType &&
                             type.GetGenericTypeDefinition() == typeof(Nullable<>);
            var paramType = isNullable ? type.GetGenericArguments()[0] : type;
            var typeName = paramType.Name;
            if (paramType.IsInterface && paramType.Name.StartsWith('I')) typeName = typeName.Substring(1);

            return new ValueTuple<string?, bool>(typeName, isNullable);
        }

        private static string GetSummary(PropertyInfo property)
        {
            var byProperty = (property.GetCustomAttribute(typeof(NamedArgSummaryAttrib)) as SummaryAttribute)?.Text;
            return byProperty ?? string.Empty;
        }
    }
}