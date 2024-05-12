using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
class Program
{
    public static void Main()
    {
        string trainOutput = "Dataset/CSharp_Normalizado/train_normalizado.cs";
        string validOutput = "Dataset/CSharp_Normalizado/valid_normalizado.cs";

        File.WriteAllText(trainOutput, string.Empty);
        File.WriteAllText(validOutput, string.Empty);

        foreach (string line in File.ReadLines("Dataset/train.cs"))
        {
            string processedLine = ProcessMethod(line);
            File.AppendAllText(trainOutput, processedLine + Environment.NewLine);
        }

        foreach (string line in File.ReadLines("Dataset/valid.cs"))
        {
            string processedLine = ProcessMethod(line);
            File.AppendAllText(validOutput, processedLine + Environment.NewLine);
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