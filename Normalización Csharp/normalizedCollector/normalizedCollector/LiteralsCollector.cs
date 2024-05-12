using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
internal class LiteralsCollector : CSharpSyntaxRewriter
{
    public List<string> Literals { get; } = new List<string>();

    public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        string literalType = GetLiteralType(node).ToUpper();

        if (literalType == "STRING" || literalType == "CHAR")
        {
            Literals.Add(node.Token.ToString());
        }
        else if (literalType == "FLOAT" || literalType == "INT")
        {
            Literals.Add(node.Token.ValueText);
        }

        return base.VisitLiteralExpression(node);
    }

    private string GetLiteralType(LiteralExpressionSyntax literal)
    {
        switch (literal.Kind())
        {
            case SyntaxKind.NumericLiteralExpression:
                return literal.Token.Value is float ? "float" : "int";
            case SyntaxKind.StringLiteralExpression:
                return "string";
            case SyntaxKind.CharacterLiteralExpression:
                return "char";
            default:
                return "unknown";
        }
    }
}
