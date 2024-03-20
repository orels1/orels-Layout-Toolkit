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
            generatedMd.AppendLine($"### {node.ReturnType.ToString()}.{node.Identifier}")
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

    private XmlDocument GetDocumentationXml(MethodDeclarationSyntax node)
    {
        var trivia = node.GetLeadingTrivia().Single(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
        var xml = trivia.GetStructure().ToString();
        xml = xml.Replace("/// ", string.Empty);
        xml = Regex.Replace(xml, @"(\w+)<(\w+)>", "$1&lt;$2&gt;");
        xml = "<xml>\r\n" + xml + "\r\n</xml>";
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        return xmlDoc;
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

        var xmlDoc = GetDocumentationXml(node);

        CreateSummary(xmlDoc);

        CreateCodeBlock(xmlDoc);

        CreateParams(xmlDoc);

        CreateNotes(xmlDoc);
    }
}

var files = Directory.GetFiles(Path.Combine("..", "..", "Packages/sh.orels.layout/Editor/Extensions"), "*.cs");

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
    using var fs = new FileStream("../../docs/src/app/docs/page.mdx", FileMode.Open, FileAccess.Read);
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

File.WriteAllText("../../docs/src/app/docs/page.mdx", finalLines.ToString());