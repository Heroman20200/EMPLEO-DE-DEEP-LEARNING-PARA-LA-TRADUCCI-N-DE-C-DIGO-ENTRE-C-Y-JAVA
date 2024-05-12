using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var (methodNames, variableNames, literalValues) = ProcessMethod(args[0]);

            Console.WriteLine(methodNames);
            Console.WriteLine(variableNames);
            Console.WriteLine(literalValues);
        }

        else
        {
            Console.WriteLine("No hay argumentos");
        }
    }

    private static (string MethodNames, string VariableNames, string LiteralValues) ProcessMethod(string methodCode)
    {
        string codeToParse;

        if (!methodCode.Contains("class"))
        {
            codeToParse = $@"
            public class TempClass 
            {{ 
                {methodCode}

            }}";
        }

        else
            codeToParse = methodCode;

        var tree = CSharpSyntaxTree.ParseText(codeToParse);
        var root = (CompilationUnitSyntax)tree.GetRoot();

        LiteralsCollector literalCollector = new LiteralsCollector();
        MethodAndVariableCollector MethodVariableCollector = new MethodAndVariableCollector();

       var normalizedRoot = literalCollector.Visit(root);
        MethodVariableCollector.Visit(normalizedRoot);

        string methodNames = string.Join(" ", MethodVariableCollector.methodNames);
        string variableNames = string.Join(" ", MethodVariableCollector.variableNames);
        string literalValues = string.Join(" ", literalCollector.Literals);

        return (methodNames, variableNames, literalValues);
    }
}