import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

public class Main {
	public static void main(String[] args) throws IOException {
			ArrayList<ArrayList<String>> tokenizado;
			
			String[] data = new String[6];
			
			data[0] = "Dataset\\train_normalizado.java";
			data[1] = "Dataset\\train_normalizado.cs";
			
			data[2] = "Dataset\\valid_normalizado.java";
			data[3] = "Dataset\\valid_normalizado.cs";
			
			data[4] = "Dataset\\test_normalizado.java";
			data[5] = "Dataset\\test_normalizado.cs";
			
			Tokenizer token = new Tokenizer();
			for(int i=0; i<6; i++)
			{
				BufferedReader br = new BufferedReader(new FileReader(data[i]));
				tokenizado = token.tokenize(br);
				
				int posInicial = data[i].indexOf("\\") + 1;
				String out = data[i].substring(posInicial, data[i].length());
				out = out.replace("_normalizado", "");
				token.saveTokenizedLines(tokenizado, "Dataset\\Tokenizado\\" + out + ".json");
			}
		}
}
