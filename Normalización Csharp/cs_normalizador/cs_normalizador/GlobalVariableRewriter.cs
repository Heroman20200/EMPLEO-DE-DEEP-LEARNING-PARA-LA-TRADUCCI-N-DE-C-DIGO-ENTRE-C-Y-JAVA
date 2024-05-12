using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class GlobalVariableRewriter : CSharpSyntaxRewriter
{
    private Dictionary<string, string> identifiers = new Dictionary<string, string>();
    private FindNormalized findnormalized = new FindNormalized();

    public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        bool found = false;
        var newVariables = SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>();
        foreach (var variable in node.Declaration.Variables)
        {
            string variableType = node.Declaration.Type.ToString();
            if (findnormalized.Found(variableType))
            {
                found = true;
                string variableName = variable.Identifier.ValueText;
                if (!identifiers.ContainsKey(variableName))
                {
                    string normalizedVariableName = "VAR_" + variableType.ToUpper();
                    identifiers[variableName] = normalizedVariableName;
                    var newVariable = variable.WithIdentifier(SyntaxFactory.Identifier(normalizedVariableName));
                    newVariables = newVariables.Add(newVariable);
                }
            }
            else
                newVariables = newVariables.Add(variable);
        }

        if(found)
            return node.WithDeclaration(node.Declaration.WithVariables(newVariables));

        return base.VisitFieldDeclaration(node);
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
