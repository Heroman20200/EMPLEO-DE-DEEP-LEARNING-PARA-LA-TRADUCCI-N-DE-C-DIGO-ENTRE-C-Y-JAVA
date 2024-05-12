using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class MethodRewriter : CSharpSyntaxRewriter
{
    private Dictionary<string, string> replacements = new Dictionary<string, string>();
	
    public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        string normalizedMethodName = "METHOD_NAME";
        replacements[node.Identifier.ValueText] = normalizedMethodName;

        node = node.WithIdentifier(SyntaxFactory.Identifier(normalizedMethodName));
        node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);

        return node;
    }

    public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        string normalizedMethodName = "CONSTRUCTOR_NAME";
        replacements[node.Identifier.ValueText] = normalizedMethodName;
		
        node = node.WithIdentifier(SyntaxFactory.Identifier(normalizedMethodName));
        node = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node);
        return node;
    }

    public override SyntaxNode VisitParameter(ParameterSyntax node)
    {
        if (node.Type != null)
        {
            string parameterType = node.Type.ToString().ToUpper();
            string normalizedParameterName = "VAR_" + parameterType;
            replacements[node.Identifier.ValueText] = normalizedParameterName;
			
            return node.WithIdentifier(SyntaxFactory.Identifier(normalizedParameterName));
        }
        return base.VisitParameter(node);
    }

    public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        if (node.Parent is VariableDeclarationSyntax declaration && declaration.Type != null)
        {
            string variableType = declaration.Type.ToString().ToUpper();
            string normalizedVariableName = "VAR_" + variableType;
            replacements[node.Identifier.ValueText] = normalizedVariableName;
            node = node.WithIdentifier(SyntaxFactory.Identifier(normalizedVariableName));

            if (node.Initializer != null)
            node = node.WithInitializer((EqualsValueClauseSyntax)Visit(node.Initializer));
        }
        return base.VisitVariableDeclarator(node);
    }

    public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (replacements.TryGetValue(node.Identifier.ValueText, out var newName))
        return node.WithIdentifier(SyntaxFactory.Identifier(newName));

        return base.VisitIdentifierName(node);
    }
}