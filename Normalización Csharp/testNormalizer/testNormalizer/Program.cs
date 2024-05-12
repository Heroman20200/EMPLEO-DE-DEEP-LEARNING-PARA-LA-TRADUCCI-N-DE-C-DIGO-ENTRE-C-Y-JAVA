using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
class Program
{
    public static void Main()
    {
        string testOutput = "Dataset/CSharp_Normalizado/test_normalizado.cs";

        File.WriteAllText(testOutput,  string.Empty);

        foreach (string line in File.ReadLines("Dataset/test.cs"))
        {
            string processedLine = ProcessMethod(line);
            File.AppendAllText(testOutput, processedLine + Environment.NewLine);
        }
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

        content = Regex.Replace(content, @"\s+", " ");
        return content;
    }
}
