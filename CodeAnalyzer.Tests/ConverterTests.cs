using System;
using Xunit;
using CodeAnalyzer.Core;

namespace CodeAnalyzer.Tests
{
    public class ConverterTests
    {

        // Architecture & contracts

        [Fact]
        public void ProgramConverter_DoesNotImplement_ICodeChecker()
        {
            var converter = new ProgramConverter();

            Assert.False(converter is ICodeChecker);
        }

        [Fact]
        public void ProgramHelper_Implements_ICodeChecker()
        {
            var helper = new ProgramHelper();

            Assert.True(helper is ICodeChecker);
        }

        // Error handling

        [Fact]
        public void ConvertToCSharp_ThrowsArgumentNullException_WhenInputIsNull()
        {
            var converter = new ProgramConverter();

            Assert.Throws<ArgumentNullException>(() => converter.ConvertToCSharp(null));
        }

        [Fact]
        public void CodeCheckSyntax_ThrowsArgumentException_WhenLanguageIsUnsupported()
        {
            var helper = new ProgramHelper();

            Assert.Throws<ArgumentException>(() => helper.CodeCheckSyntax("code", "Python"));
        }

        // Behavior        

        [Theory]
        [InlineData("var a = 5;", "C#", true)]
        [InlineData("var a = 5", "C#", false)]
        [InlineData("Dim a = 5", "VB", true)]
        [InlineData("Dim a = 5;", "VB", false)]
        public void CodeCheckSyntax_ReturnsExpectedResult(string code, string language, bool expected)
        {
            var helper = new ProgramHelper();

            var result = helper.CodeCheckSyntax(code, language);

            Assert.Equal(expected, result);
        }
    }
}