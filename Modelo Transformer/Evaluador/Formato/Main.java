import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

public class Main {
	public static void main(String[] args) throws IOException {
			String[] data = new String[2];
			
			data[0] = "..\\traducciones.java";
			data[1] = "..\\test.java";
			
			Formatear formatear = new Formatear();
			for(int i=0; i<2; i++)
			{
				BufferedReader br = new BufferedReader(new FileReader(data[i]));
				ArrayList<ArrayList<String>> formateado = formatear.format(br);
				
				int posIntermedia = data[i].indexOf(".java");
				String out = data[i].substring(0, posIntermedia) + "_formateado.java";
				formatear.saveFormattedLines(formateado, out);
			}
		}
}
