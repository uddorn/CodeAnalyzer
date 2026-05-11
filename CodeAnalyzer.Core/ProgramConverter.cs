using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeAnalyzer.Core
{
    public class ProgramConverter : IConvertible
    {
        private readonly Dictionary<string, string> _typeMappings = new Dictionary<string, string>
        {
            { "Integer", "int" },
            { "String", "string" },
            { "Boolean", "bool" },
            { "Double", "double" },
            { "Char", "char" },
            { "Long", "long" },
            { "Decimal", "decimal" },
            { "Single", "float" },
            { "Short", "short" },
            { "Byte", "byte" },
            { "Object", "object" },
            { "Date", "DateTime" },
            { "UInteger", "uint" },
            { "ULong", "ulong" },
            { "UShort", "ushort" },
            { "SByte", "sbyte" }
        };

        private string ProcessOutsideStrings(string code, Func<string, string> processAction)
        {
            var stringLiterals = new Dictionary<string, string>();
            int id = 0;

            string maskedCode = Regex.Replace(code, "\".*?\"", match =>
            {
                string placeholder = $"__STR{id++}__";
                stringLiterals[placeholder] = match.Value;
                return placeholder;
            });

            string processedCode = processAction(maskedCode);

            foreach (var kvp in stringLiterals)
            {
                processedCode = processedCode.Replace(kvp.Key, kvp.Value);
            }

            return processedCode;
        }

        public virtual string ConvertToCSharp(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code), "Код для конвертації не може бути порожнім.");

            string result = code.Trim();

            result = ProcessOutsideStrings(result, text =>
            {
                foreach (var mapping in _typeMappings)
                {
                    text = Regex.Replace(text, $@"Dim\s+(\w+)\s+As\s+{mapping.Key}", $"{mapping.Value} $1");
                    text = Regex.Replace(text, $@"\b{mapping.Key}\b", mapping.Value);
                }
                text = Regex.Replace(text, @"\bDim\b", "var");
                text = Regex.Replace(text, @"\bTrue\b", "true");
                text = Regex.Replace(text, @"\bFalse\b", "false");
                return text;
            });

            if (!result.EndsWith(";")) result += ";";

            return $"[Імітація] Перетворення на C# завершено.\nРезультат: {result}";
        }

        public virtual string ConvertToVB(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code), "Код для конвертації не може бути порожнім.");

            string result = code.Trim();
            if (result.EndsWith(";")) result = result.Substring(0, result.Length - 1);

            result = ProcessOutsideStrings(result, text =>
            {
                foreach (var mapping in _typeMappings)
                {
                    text = Regex.Replace(text, $@"\b{mapping.Value}\s+(\w+)", $"Dim $1 As {mapping.Key}");
                    text = Regex.Replace(text, $@"\b{mapping.Value}\b", mapping.Key);
                }

                text = Regex.Replace(text, @"\bvar\b(?=\s+\w+)", "Dim");

                text = Regex.Replace(text, @"\btrue\b", "True");
                text = Regex.Replace(text, @"\bfalse\b", "False");
                return text;
            });

            return $"[Імітація] Перетворення на VB завершено.\nРезультат: {result}";
        }
    }
}