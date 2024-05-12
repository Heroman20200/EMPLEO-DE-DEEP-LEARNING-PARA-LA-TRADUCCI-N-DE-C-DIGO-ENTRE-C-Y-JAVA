import json
import numpy as np

csharp_file = 'Dataset\\Tokenizado\\train.cs.json'
java_file = 'Dataset\\Tokenizado\\train.java.json'

with open(csharp_file, 'r') as file:
    tokenized_csharp = json.load(file)
    
with open(java_file, 'r') as file:
    tokenized_java = json.load(file)
      
token_list_csharp = tokenized_csharp[:-501]
token_list_java = tokenized_java[:-501]

def build_token_dict(token_list):
    token_dict = {
          'PAD': 0,
          'START': 1,
          'END': 2,
          'UNK': 3
    }
  
    for tokens in token_list:
      for token in tokens:
        if token not in token_dict:
          token_dict[token] = len(token_dict)
    return token_dict
    
source_token_dict = build_token_dict(token_list_csharp)
target_token_dict = build_token_dict(token_list_java)
target_token_dict_inv = {v:k for k,v in target_token_dict.items()}

with open('Data\\source_token_dict.json', 'w') as file:
    json.dump(source_token_dict, file)

with open('Data\\target_token_dict.json', 'w') as file:
    json.dump(target_token_dict, file)

with open('Data\\target_token_dict_inv.json', 'w') as file:
    json.dump(target_token_dict_inv, file)
