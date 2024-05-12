using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
class Program
{
    public static void Main()
    {
        string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

        string filePath = Path.Combine(parentDirectory, "codigo.cs");

        string fileOut = Path.Combine(parentDirectory, "codigo_normalizado.cs"); ;

        string processedContent = ProcessMethod(File.ReadAllText(filePath));

        File.WriteAllText(fileOut, processedContent);
    }
    private static string ProcessMethod(string methodCode)
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

        LiteralsRewriter literalRewriter = new LiteralsRewriter();
        MethodRewriter methodRewriter = new MethodRewriter();
        GlobalVariableRewriter globalVariableRewriter = new GlobalVariableRewriter();

        var normalizedRoot = literalRewriter.Visit(root);
        normalizedRoot = methodRewriter.Visit(normalizedRoot);
        normalizedRoot = globalVariableRewriter.Visit(normalizedRoot);

        string normalizedCode = normalizedRoot.NormalizeWhitespace().ToFullString();

        var startOfMethod = normalizedCode.IndexOf('{') + 1;
        var endOfMethod = normalizedCode.LastIndexOf('}');
        string content = normalizedCode.Substring(startOfMethod, endOfMethod - startOfMethod).Trim();

        return content;
    }
}