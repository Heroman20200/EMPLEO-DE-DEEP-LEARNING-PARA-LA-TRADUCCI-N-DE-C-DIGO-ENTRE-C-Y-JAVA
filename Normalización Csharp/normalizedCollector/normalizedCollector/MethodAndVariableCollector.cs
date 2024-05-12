using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class MethodAndVariableCollector : CSharpSyntaxRewriter
{
    public List<string> methodNames = new List<string>();
    public List<string> variableNames = new List<string>();
    FindNormalized findNormalized = new FindNormalized();

    public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        methodNames.Add(node.Identifier.ValueText);
        return base.VisitMethodDeclaration(node);
    }


    public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        methodNames.Add(node.Identifier.ValueText);

        return base.VisitConstructorDeclaration(node);
    }

 
    public override SyntaxNode VisitParameter(ParameterSyntax node)
    {
        if (node.Type != null)
        {
            string parameterType = node.Type.ToString().ToUpper();
            if (findNormalized.Found(parameterType))
                variableNames.Add(node.Identifier.ValueText);
        }

        return base.VisitParameter(node);
    }


    public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
    {
        if (node.Parent is VariableDeclarationSyntax declaration && declaration.Type != null)
        {
            string variableType = declaration.Type.ToString().ToUpper();
            if (findNormalized.Found(variableType))
                variableNames.Add(node.Identifier.ValueText);
        }

        return base.VisitVariableDeclarator(node);
    }

    public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
    {
        var identifierValue = node.Identifier.ValueText;

        if (methodNames.Contains(identifierValue))
            methodNames.Add(identifierValue);

        else if (variableNames.Contains(identifierValue))
            variableNames.Add(identifierValue);

        return base.VisitIdentifierName(node);
    }
}
