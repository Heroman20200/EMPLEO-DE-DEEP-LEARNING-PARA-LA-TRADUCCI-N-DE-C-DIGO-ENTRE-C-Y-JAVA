from enum import Enum
import json

def obtenerIndicesNormalizados():
    
    with open('..\\..\\Data\\target_token_dict.json', 'r') as file:
       target_tokens = json.load(file)
        
    token_indices = {
        'METODOS': [],
        'VARIABLES': [],
        'LITERALES': []
    }

    for token, indice in target_tokens.items():
        if token.startswith('METHOD_') or token.startswith('CONSTRUCTOR_'):
            token_indices['METODOS'].append(indice)
        elif token.startswith('VAR_'):
            token_indices['VARIABLES'].append(indice)
        elif token.startswith('LITERAL_'):
            token_indices['LITERALES'].append(indice)
            
    return token_indices

