import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

public class Main {
	public static void main(String[] args) throws IOException {
			ArrayList<ArrayList<String>> tokenizado;
			
			String data = "..\\codigo_normalizado.cs";
			
			Tokenizer token = new Tokenizer();
			BufferedReader br = new BufferedReader(new FileReader(data));
			tokenizado = token.tokenize(br);
			token.saveTokenizedLines(tokenizado, "..\\codigo.json");
		}
}
