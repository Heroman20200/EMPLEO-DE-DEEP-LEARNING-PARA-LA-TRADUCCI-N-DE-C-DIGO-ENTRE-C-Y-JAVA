using Newtonsoft.Json;

    internal class FindNormalized
    {
        private Dictionary<string, int> tokenDictionary;

        public FindNormalized()
        {
            string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            string generalDirectory = Directory.GetParent(parentDirectory).FullName;

            string filePath = Path.Combine(generalDirectory, "Data\\source_token_dict.json");

            string jsonData = File.ReadAllText(filePath);
            tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
        }

        public bool Found(string type)
        {
            string aux_type = type.Replace("[", "").Replace("]", "");
            string normalizedToken = "VAR_" + aux_type.ToUpper();

            // Realiza la búsqueda en el diccionario
            return tokenDictionary.ContainsKey(normalizedToken);
        }
    }
