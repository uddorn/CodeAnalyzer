using System;

namespace CodeAnalyzer.Core
{
    public class ProgramHelper : ProgramConverter, ICodeChecker
    {
        public bool CodeCheckSyntax(string code, string language)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code), "Код для перевірки не може бути порожнім.");
            }

            if (language != "C#" && language != "VB")
            {
                throw new ArgumentException("Підтримуються лише мови 'C#' та 'VB'.", nameof(language));
            }

            // Базова логіка перевірки для тестів
            // Якщо це C#, рядок має закінчуватися крапкою з комою. Якщо VB - ні.
            if (language == "C#")
            {
                return code.Trim().EndsWith(";");
            }
            else // Для VB
            {
                return !code.Contains(";");
            }
        }
    }
}