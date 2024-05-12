using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class GlobalVariableRewriter : CSharpSyntaxRewriter
{
    private Dictionary<string, string> identifiers = new Dictionary<string, string>();

    public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        var newVariables = SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>();
        foreach (var variable in node.Declaration.Variables)
        {
            string variableName = variable.Identifier.ValueText;
            string variableType = node.Declaration.Type.ToString().ToUpper();
            string normalizedVariableName = "VAR_" + variableType;

            if (!identifiers.ContainsKey(variableName))
            {
                identifiers[variableName] = normalizedVariableName;
                var newVariable = variable.WithIdentifier(SyntaxFactory.Identifier(normalizedVariableName));
                newVariables = newVariables.Add(newVariable);
            }
        }

        return node.WithDeclaration(node.Declaration.WithVariables(newVariables));
    }

    public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
    {
        var identifierValue = node.Identifier.ValueText;

        if (identifiers.TryGetValue(identifierValue, out var newName))
        {
            return node.WithIdentifier(SyntaxFactory.Identifier(newName));
        }

        return base.VisitIdentifierName(node);
    }
}
