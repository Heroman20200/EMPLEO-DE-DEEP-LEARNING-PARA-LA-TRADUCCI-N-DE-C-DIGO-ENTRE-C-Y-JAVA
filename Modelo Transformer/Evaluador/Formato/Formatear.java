import java.io.BufferedReader;
import java.io.FileOutputStream;
import java.io.OutputStreamWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Formatear
{ 
	public ArrayList<ArrayList<String>> format(BufferedReader br) throws IOException 
	{
        ArrayList<ArrayList<String>> formattedLines = new ArrayList<>();
        String linea;

        while ((linea = br.readLine()) != null) formattedLines.add(liner(linea));

        return formattedLines;
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

    public void saveFormattedLines(ArrayList<ArrayList<String>> formattedLines, String filePath) throws IOException {
        OutputStreamWriter writer = new OutputStreamWriter(new FileOutputStream(filePath), "UTF-8");
		for (ArrayList<String> line : formattedLines) 
		{
			for (int j = 0; j < line.size(); j++) 
			{
				writer.write(line.get(j));
				if (j < line.size() - 1) 
				{
					writer.write(" ");
				}
			}
			writer.write("\n");
		}
    }
}