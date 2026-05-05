namespace CodeAnalyzer.Core
{
    public interface ICodeChecker
    {
        bool CodeCheckSyntax(string code, string language);
    }
}