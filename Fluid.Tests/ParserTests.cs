﻿using System.Collections.Generic;
using System.Linq;
using Fluid.Ast;
using Irony.Parsing;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Fluid.Tests
{
    public class ParserTests
    {
        private static LanguageData _language = new LanguageData(new FluidGrammar());

        private List<Statement> Parse(string template)
        {
            new FluidParser().TryParse(new StringSegment(template), out var statements, out var errors);
            return statements;
        }

        [Fact]
        public void ShouldParseText()
        {
            var statements = Parse("Hello World");

            var textStatement = statements.First() as TextStatement;

            Assert.Equal(1, statements.Count);
            Assert.NotNull(textStatement);
            Assert.Equal("Hello World", textStatement.Text);
        }

        [Fact]
        public void ShouldParseOutput()
        {
            var statements = Parse("{{ 1 }}");

            var outputStatement = statements.First() as OutputStatement;

            Assert.Equal(1, statements.Count);
            Assert.NotNull(outputStatement);
        }

        [Theory]
        [InlineData("{{ a }}")]
        [InlineData("{{ a.b }}")]
        [InlineData("{{ a.b[1] }}")]
        public void ShouldParseOutputWithMember(string source)
        {
            var statements = Parse(source);

            var outputStatement = statements.First() as OutputStatement;

            Assert.Equal(1, statements.Count);
            Assert.NotNull(outputStatement);
        }

        [Fact]
        public void ShouldParseForTag()
        {
            var statements = Parse("{% for a in b %} {% endfor %}");

            Assert.IsType<ForStatement>(statements.ElementAt(0));
        }
    }
}