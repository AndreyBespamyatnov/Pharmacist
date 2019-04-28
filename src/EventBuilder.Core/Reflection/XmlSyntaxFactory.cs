﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EventBuilder.Core.Reflection
{
    /// <summary>
    /// This class was originally from StyleCop. https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Helpers/XmlSyntaxFactory.cs
    /// All credit goes to the StyleCop team.
    /// </summary>
    internal static class XmlSyntaxFactory
    {
        static XmlSyntaxFactory()
        {
            // Make sure the newline is included. Otherwise the comment and the method will be on the same line.
            InheritdocSyntax = ParseLeadingTrivia(@"/// <inheritdoc />" + Environment.NewLine);
        }

        /// <summary>
        /// Gets a inheritdoc leading trivia comment.
        /// </summary>
        public static SyntaxTriviaList InheritdocSyntax { get; }

        public static SyntaxTriviaList GenerateSummaryComment(string summaryText, string parameterFormat, IMethod entity)
        {
            var parameters = entity.Parameters.Select(x => (x.Name, string.Format(CultureInfo.InvariantCulture, parameterFormat, x.Name)));

            return GenerateSummaryComment(summaryText, parameters);
        }

        public static SyntaxTriviaList GenerateSummarySeeAlsoComment(string summaryText, string seeAlsoText)
        {
            var text = string.Format(CultureInfo.InvariantCulture, summaryText, "<see cref=\"" + seeAlsoText.Replace("<", "{").Replace(">", "}") + "\" />");
            string template = "/// <summary>" + Environment.NewLine +
                              $"/// {text}" + Environment.NewLine +
                              "/// </summary>" + Environment.NewLine;

            return ParseLeadingTrivia(template);
        }

        public static SyntaxTriviaList GenerateSummarySeeAlsoComment(string summaryText, string seeAlsoText, params (string paramName, string paramText)[] parameters)
        {
            var text = string.Format(CultureInfo.InvariantCulture, summaryText, "<see cref=\"" + seeAlsoText.Replace("<", "{").Replace(">", "}") + "\" />");
            var sb = new StringBuilder("/// <summary>")
                .AppendLine()
                .Append("/// ").AppendLine(text)
                .AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                sb.AppendLine($"/// <param name=\"{parameter.paramName}\">{parameter.paramText}</param>");
            }

            return ParseLeadingTrivia(sb.ToString());
        }

        public static SyntaxTriviaList GenerateSummaryComment(string summaryText)
        {
            string template = "/// <summary>" + Environment.NewLine +
                              $"/// {summaryText}" + Environment.NewLine +
                              "/// </summary>" + Environment.NewLine;

            return ParseLeadingTrivia(template);
        }

        public static SyntaxTriviaList GenerateSummaryComment(string summaryText, string returnValueText)
        {
            string template = "/// <summary>" + Environment.NewLine +
                              $"/// {summaryText}" + Environment.NewLine +
                              "/// </summary>" + Environment.NewLine +
                              $"/// <returns>{returnValueText}///<returns>" + Environment.NewLine;

            return ParseLeadingTrivia(template);
        }

        public static SyntaxTriviaList GenerateSummaryComment(string summaryText, IEnumerable<(string paramName, string paramText)> parameters)
        {
            var sb = new StringBuilder("/// <summary>")
                .AppendLine()
                .Append("/// ").AppendLine(summaryText)
                .AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                sb.AppendLine($"/// <param name=\"{parameter.paramName}\">{parameter.paramText}</param>");
            }

            return ParseLeadingTrivia(sb.ToString());
        }

        public static SyntaxTriviaList GenerateSummaryComment(string summaryText, IEnumerable<(string paramName, string paramText)> parameters, string returnValueText)
        {
            var sb = new StringBuilder("/// <summary>")
                .AppendLine()
                .Append("/// ").AppendLine(summaryText)
                .AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                sb.AppendLine($"/// <param name=\"{parameter.paramName}\">{parameter.paramText}</param>");
            }

            sb.Append("/// <returns>").Append(returnValueText).AppendLine("</returns>");
            return ParseLeadingTrivia(sb.ToString());
        }
    }
}