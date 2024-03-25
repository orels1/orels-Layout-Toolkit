#r "nuget:Microsoft.CodeAnalysis.CSharp.Workspaces,4.9.2"

using static System.Console;
using System.Xml;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class MethodVisitor : CSharpSyntaxWalker
{
    public StringBuilder generatedMd = new StringBuilder();

    private void CreateHeader(MethodDeclarationSyntax node)
    {
        if (node.ReturnType.ToString() != "T" && node.ReturnType.ToString() != "void")
        {
            generatedMd.AppendLine($"### {Regex.Replace(node.ReturnType.ToString(), @"(\w+)<(\w+)>", "$1&lt;$2&gt;")}.{node.Identifier}")
                .AppendLine();
            return;
        }

        if (node.ReturnType.ToString() == "T")
        {
            if (node.ConstraintClauses.Any(c => c.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.TypeParameterConstraintClause))
            {
                var constraint = node.ConstraintClauses.Single(c => c.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.TypeParameterConstraintClause).Constraints.First();
                var typeName = ((TypeConstraintSyntax)constraint).Type.ToString();
                typeName = Regex.Replace(typeName, @"(\w+)<(\w+)>", "$1&lt;$2&gt;");
                generatedMd.AppendLine($"### {typeName}.{node.Identifier}")
                    .AppendLine();
                return;
            }
        }

        generatedMd.AppendLine($"### {node.Identifier}")
            .AppendLine();
    }

    // (?<!`\w*)(\w+)<(\w+)>(?!`\w*) to match non-codeblocked ones
    private XmlDocument GetDocumentationXml(MethodDeclarationSyntax node)
    {
        var trivia = node.GetLeadingTrivia().Single(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
        var xml = trivia.GetStructure().ToString();

        try
        {
            xml = "<xml>\r\n" + xml + "</xml>";
            var cleaned = new StringBuilder();

            // walk the documentation to escape generic parameters, e.g. `Func<T, ReactiveProperty<IEnumerable>>`
            foreach (var line in xml.Split(Environment.NewLine))
            {
                var local = line;
                if (local.Trim().StartsWith("///"))
                {
                    if (local.Trim().Length > 3)
                    {
                        local = local.Trim().Substring(4);
                    }
                    else
                    {
                        local = "";
                    }
                }

                var current = 0;
                while (current < local.Length)
                {
                    var c = local[current];
                    // We do not want to escape the XML tags
                    // we also do not want to excape inside <code> blocks
                    if (c == '<' && current != 0 && local[current - 1] != ' ' && local[current + 1] != '/')
                    {
                        var rest = local.Substring(current);
                        var genericPos = 1;
                        var genericLevels = 1;
                        while (genericPos < rest.Length)
                        {
                            if (rest[genericPos] == '<')
                            {
                                genericLevels++;
                            }
                            if (rest[genericPos] == '>')
                            {
                                genericLevels--;
                            }
                            if (genericLevels == 0)
                            {
                                break;
                            }
                            genericPos++;
                        }
                        local = local.Remove(current, 1).Insert(current, "&lt;");
                        local = local.Remove(current + genericPos + 3, 1).Insert(current + genericPos + 3, "&gt;");
                        current += 3;
                        continue;
                    }
                    current++;
                }

                cleaned.AppendLine(local);
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(cleaned.ToString());
            return xmlDoc;
        }
        catch (Exception)
        {
            WriteLine($"Error parsing the followng XML");
            WriteLine(xml);
            throw;
        }
    }

    private void CreateSummary(XmlDocument xmlDoc)
    {
        var summaryBlock = xmlDoc.SelectSingleNode("descendant::summary");
        if (summaryBlock == null) return;

        var summary = summaryBlock.InnerText.Trim();
        generatedMd.AppendLine(summary);
        generatedMd.AppendLine();
    }

    private void CreateCodeBlock(XmlDocument xmlDoc)
    {
        var codeBlock = xmlDoc.SelectSingleNode("descendant::code");
        if (codeBlock == null) return;

        var codeLines = codeBlock.InnerText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        var offset = codeLines[0].Length - codeLines[0].TrimStart().Length;
        var code = string.Join("\r\n", codeLines.Where(l => l.Trim() != string.Empty).Select(l => l.Substring(offset)));

        generatedMd.AppendLine("```csharp")
            .AppendLine(code)
            .AppendLine("```")
            .AppendLine();
    }

    private void CreateParams(XmlDocument xmlDoc)
    {
        var paramDocs = xmlDoc.SelectNodes("descendant::param");
        if (paramDocs.Count == 0) return;

        var hasNonEmptyParams = paramDocs.Cast<XmlNode>().Where(p => !string.IsNullOrWhiteSpace(p.InnerText)).ToList().Any();
        if (!hasNonEmptyParams) return;

        generatedMd.AppendLine("#### Parameters")
            .AppendLine();

        foreach (XmlNode param in paramDocs)
        {
            if (string.IsNullOrWhiteSpace(param.InnerText)) continue;
            generatedMd.AppendLine($"- **{param.Attributes["name"].Value}** - {param.InnerText.Trim()}");
        }

        generatedMd.AppendLine();
    }

    private void CreateNotes(XmlDocument xmlDoc)
    {
        var listBlock = xmlDoc.SelectSingleNode("descendant::list");
        if (listBlock == null) return;

        var listItems = listBlock.SelectNodes("descendant::item");
        generatedMd.AppendLine("#### Notes")
            .AppendLine();
        foreach (XmlNode item in listItems)
        {
            generatedMd.AppendLine($"- {item.InnerText.Trim()}");
        }
        generatedMd.AppendLine();
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (!node.HasLeadingTrivia) return;
        if (!node.GetLeadingTrivia().Any(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia)) return;

        CreateHeader(node);

        XmlDocument xmlDoc = null;
        try
        {
            xmlDoc = GetDocumentationXml(node);
        }
        catch (Exception)
        {
            WriteLine($"Error parsing XML for {node.Identifier}");
            throw;
        }

        CreateSummary(xmlDoc);

        CreateCodeBlock(xmlDoc);

        CreateParams(xmlDoc);

        CreateNotes(xmlDoc);
    }
}

var files = Directory.GetFiles("Packages/sh.orels.layout/Editor/Extensions", "*.cs").ToList();
var elementFiles = Directory.GetFiles("Packages/sh.orels.layout/Editor/Elements", "*.cs");
files.AddRange(elementFiles);

var visitor = new MethodVisitor();
var finalLines = new StringBuilder();

foreach (var file in files)
{
    var text = File.ReadAllText(file);

    SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

    visitor.Visit(root);
}

{
    using var fs = new FileStream("docs/src/app/docs/page.mdx", FileMode.Open, FileAccess.Read);
    using var sr = new StreamReader(fs);

    var targetLine = -1;
    var lineIndex = 0;
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine();
        if (line.Trim() == "## API Reference")
        {
            targetLine = lineIndex + 2;
        }
        finalLines.AppendLine(line);
        if (lineIndex == targetLine)
        {
            finalLines.AppendLine().AppendLine(visitor.generatedMd.ToString());
            break;
        }
        lineIndex++;
    }

}

File.WriteAllText("docs/src/app/docs/page.mdx", finalLines.ToString());