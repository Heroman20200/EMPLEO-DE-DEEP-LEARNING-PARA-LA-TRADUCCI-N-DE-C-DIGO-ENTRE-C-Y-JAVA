import json
from keras_transformer import decode
from queue import Queue
from normalizerData import *
from normalizedEnum import obtenerIndicesNormalizados

with open('Data\\source_token_dict.json', 'r') as file:
    source_token_dict = json.load(file)
    
with open('Data\\target_token_dict.json', 'r') as file:
    target_token_dict = json.load(file)
    
with open('Data\\target_token_dict_inv.json', 'r') as file:
    target_token_dict_inv = json.load(file)
    target_token_dict_inv = {int(k): v for k, v in target_token_dict_inv.items()}

indices_normalizados = obtenerIndicesNormalizados()
unk_index = source_token_dict['UNK']

def translate(tokenized_sequence, sequence, model):
    metodos, variables, literales = normalizeCollector(sequence)
    sequence_tokens = tokenized_sequence + ['END', 'PAD']
    
    unk_tokens = Queue()
    tr_input = []
    for token in sequence_tokens:
        token_index = source_token_dict.get(token, unk_index)
        tr_input.append(token_index)
        
        if token_index == unk_index:
            unk_tokens.put(token)
            
    decoded = decode(
        model, 
        tr_input, 
        start_token=target_token_dict['START'],
        end_token=target_token_dict['END'],
        pad_token=target_token_dict['PAD']
    )
    reconstructed_sequence = []
    for token_index in decoded[1:-1]:
        translated_token = ''
        if token_index == unk_index and not unk_tokens.empty():
            translated_token = unk_tokens.get()
            
        elif token_index in indices_normalizados['METODOS'] and not metodos.empty():
            translated_token = metodos.get()
            
        elif token_index in indices_normalizados['VARIABLES'] and not variables.empty():
            translated_token = variables.get()
        
        elif token_index in indices_normalizados['LITERALES'] and not literales.empty():
            translated_token = literales.get()
            
        # Traduce usando target_token_dict_inv para obtener el token a partir del índice
        if translated_token == '':
            translated_token = target_token_dict_inv.get(token_index, '?')
            
        reconstructed_sequence.append(translated_token)
    reconstructed_sequence_str = ' '.join(reconstructed_sequence)
    return reconstructed_sequence_str