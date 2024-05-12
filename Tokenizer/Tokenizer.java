import java.io.BufferedReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Tokenizer 
{
    public ArrayList<ArrayList<String>> tokenize(BufferedReader br) throws IOException 
	{
        ArrayList<ArrayList<String>> tokenizedLines = new ArrayList<>();
        String linea;
        while ((linea = br.readLine()) != null) tokenizedLines.add(liner(linea));

        return tokenizedLines;
    }
	
    public ArrayList<String> liner(String input) 
	{
		ArrayList<String> linea = new ArrayList<>();

		String regex =
				"\\b\\w+\\b" +                   				// Identificadores
				"|\\d+" +                        				// Literales numéricos
				"|\"[^\"]*\"" +                  				// Cadenas de texto
				"|[{}()\\[\\];,:?@.]" +            				// Símbolos de puntuación
				"|\\+=|-=|==|!=|<=|>=|&&|\\|\\||\\+\\+|--" + 	// Operadores compuestos
				"|[+\\-*/=<>!&|^%]+";            				// Operadores simples

		Pattern pattern = Pattern.compile(regex);
		Matcher matcher = pattern.matcher(input);

		while (matcher.find()) linea.add(matcher.group());

		return linea;
	}

	public void saveTokenizedLines(ArrayList<ArrayList<String>> tokenizedLines, String filePath) throws IOException 
	{
		FileWriter writer = new FileWriter(filePath);
		writer.write("[\n");
		for (int i = 0; i < tokenizedLines.size(); i++) 
		{
			writer.write("  [");
			ArrayList<String> line = tokenizedLines.get(i);
			for (int j = 0; j < line.size(); j++) 
			{
				String token = escapeJson(line.get(j));
				writer.write("\"" + token + "\"");
				if (j < line.size() - 1) 
				{
					writer.write(", ");
				}
			}
			writer.write("]");
			if (i < tokenizedLines.size() - 1) 
			{
				writer.write(",");
			}
			writer.write("\n");
		}
		writer.write("]");
		writer.close();
    }

    private String escapeJson(String str) 
	{
        return str.replace("\\", "\\\\")
                  .replace("\"", "\\\"")
                  .replace("\b", "\\b")
                  .replace("\f", "\\f")
                  .replace("\n", "\\n")
                  .replace("\r", "\\r")
                  .replace("\t", "\\t");
    }
}
