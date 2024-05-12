using Newtonsoft.Json;

    internal class FindNormalized
    {
        private Dictionary<string, int> tokenDictionary;

        public FindNormalized()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string filePath = Path.Combine(currentDirectory, "Dataset\\source_token_dict.json");

            string jsonData = File.ReadAllText(filePath);
            tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
        }

        public bool Found(string type)
        {
            string aux_type = type.Replace("[", "").Replace("]", "");
            string normalizedToken = "VAR_" + aux_type.ToUpper();

            return tokenDictionary.ContainsKey(normalizedToken);
        }
    }
	