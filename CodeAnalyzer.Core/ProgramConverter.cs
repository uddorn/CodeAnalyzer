using System;

namespace CodeAnalyzer.Core
{
    public class ProgramConverter : IConvertible
    {
        public virtual string ConvertToCSharp(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code), "Код для конвертації не може бути порожнім.");
            }

            // Імітація перетворення на C#
            string result = code.Replace("Dim", "var");
            if (!result.EndsWith(";"))
            {
                result += ";";
            }

            return $"[Імітація] Перетворення на C# завершено.\nРезультат: {result}";
        }

        public virtual string ConvertToVB(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code), "Код для конвертації не може бути порожнім.");
            }

            // Імітація перетворення на VB
            string result = code.Replace("var", "Dim").Replace(";", "");

            return $"[Імітація] Перетворення на VB завершено.\nРезультат: {result}";
        }
    }
}