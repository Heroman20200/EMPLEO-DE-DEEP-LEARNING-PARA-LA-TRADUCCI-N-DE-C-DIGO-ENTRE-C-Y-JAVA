import json
from keras_transformer import get_model
from Inferencia import translate

# Asegúrate de tener disponible el diccionario de tokens o cualquier otra configuración necesaria
# para reconstruir la arquitectura del modelo exactamente como estaba durante el entrenamiento
with open('..\\..\\Data\\source_token_dict.json', 'r') as file:
    source_token_dict = json.load(file)
    
with open('..\\..\\Data\\target_token_dict.json', 'r') as file:
    target_token_dict = json.load(file)

model = get_model(
    token_num=max(len(source_token_dict), len(target_token_dict)),
    embed_dim=32,
    encoder_num=2,
    decoder_num=2,
    head_num=4,
    hidden_dim=128,
    dropout_rate=0.2,
    use_same_embed=False
)

model.load_weights('traductor.h5')

def unir_lista_tokens(tokens_lists):
    joined_list = []
    for line in tokens_lists:
        joined_list.extend(line)  # Añade los tokens de la línea actual
    return joined_list
            
cs_tester = '..\\codigo.cs'    
cs_test_t = '..\\codigo.json'
    
with open(cs_tester, 'r') as file:
    cs_test = file.read()
    
with open(cs_test_t, 'r') as file:
    cs_test_tokenized = json.load(file)

with open('..\\codigo_traducido.java', 'w', encoding='utf-8') as f:
        traduccion = translate(unir_lista_tokens(cs_test_tokenized), cs_test, model)
        f.write(f'{traduccion.rstrip()}\n') 

