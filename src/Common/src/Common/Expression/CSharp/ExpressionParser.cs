﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;

namespace Steeltoe.Common.Expression.CSharp
{
    public class ExpressionParser : IExpressionParser
    {
        public IExpression ParseExpression(string expressionString)
        {
            // TODO: SPEL
            // throw new NotImplementedException();
            if (int.TryParse(expressionString, out int result))
            {
                return new ValueExpression<int>(result);
            }

            return new ValueExpression<string>(expressionString);
        }

        public IExpression ParseExpression(string expressionString, IParserContext context)
        {
            // TODO: SPEL
            // throw new NotImplementedException();
            return null;
        }
    }
}
