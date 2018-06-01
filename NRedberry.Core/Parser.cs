using System;
using System.Collections.Generic;
using NRedberry.Core.Parsers;

namespace NRedberry.Core
{
    /// <summary>
    /// Parser of mathematical expressions.
    /// </summary>
    public sealed class Parser
    {
        //public static Parser Default = new Parser(
        //    ParserBrackets.Instance,
        //    ParserSum.Instance,
        //    ParserProduct.Instance,
        //    ParserSimpleTensor.Instance,
        //    ParserTensorField.Instance,
        //    ParserPower.Instance,
        //    ParserNumber.Instance,
        //    ParserFunctions.Instance,
        //    ParserExpression.Instance,
        //    ParserPowerAst.Instance);

        private IEnumerable<ITokenParser> TokenParsers { get; }

        /// <summary>
        /// Constructs Parser from a given parsers of AST nodes.
        /// </summary>
        /// <param name="tokenParsers"></param>
        public Parser(params ITokenParser[] tokenParsers)
        {
            TokenParsers = tokenParsers;
        }

        public ParseToken parse(string expression)
        {
            if(string.IsNullOrEmpty(expression)) throw new ArgumentNullException(nameof(expression));

            foreach(var tokenParser in TokenParsers)
            {
                var node = tokenParser.ParseToken(expression.Trim(), this);
                if (node != null)
                    return node;
            }

            throw new ParserException($"No appropriate parser for expression: \"{expression}\"");
        }
    }
}
