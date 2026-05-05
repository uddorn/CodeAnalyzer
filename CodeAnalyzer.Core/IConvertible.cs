namespace CodeAnalyzer.Core
{
    public interface IConvertible
    {
        string ConvertToCSharp(string code);
        string ConvertToVB(string code);
    }
}