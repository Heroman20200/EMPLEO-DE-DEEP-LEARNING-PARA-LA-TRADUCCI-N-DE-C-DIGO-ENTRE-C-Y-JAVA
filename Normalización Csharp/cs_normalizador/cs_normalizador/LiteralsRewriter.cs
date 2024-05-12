using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class LiteralsRewriter : CSharpSyntaxRewriter
{
    public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        string literalType = GetLiteralType(node).ToUpper();

        if (literalType == "FLOAT" || literalType == "INT" || literalType == "STRING" || literalType == "NULL" || literalType == "CHAR")
        return SyntaxFactory.IdentifierName("LITERAL_" + literalType);

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
            case SyntaxKind.NullLiteralExpression:
                return "null";
            default:
                return "unknown";
        }
    }
}