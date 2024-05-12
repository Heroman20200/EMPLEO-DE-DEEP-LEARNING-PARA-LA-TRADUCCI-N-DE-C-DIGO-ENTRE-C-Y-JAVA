using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class LiteralsRewriter : CSharpSyntaxRewriter
{
    public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        string literalType = GetLiteralType(node).ToUpper();

        if (literalType == "FLOAT" || literalType == "INT" || literalType == "STRING" || literalType == "CHAR")
        return SyntaxFactory.IdentifierName("LITERAL_" + literalType);

        return base.VisitLiteralExpression(node);
    }

    private string GetLiteralType(LiteralExpressionSyntax literal)
    {
        // Utilizar el tipo de nodo de SyntaxKind para determinar el tipo del literal
        switch (literal.Kind())
        {
            case SyntaxKind.NumericLiteralExpression:
                // Distingue entre enteros y flotantes.
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