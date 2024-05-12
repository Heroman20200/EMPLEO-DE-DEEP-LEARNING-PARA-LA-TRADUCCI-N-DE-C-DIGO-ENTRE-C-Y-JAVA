import json
from keras_transformer import get_model
from Inferencia import translate

with open('Data\\source_token_dict.json', 'r') as file:
    source_token_dict = json.load(file)
    
with open('Data\\target_token_dict.json', 'r') as file:
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
            
cs_tester = 'Dataset\\test.cs'    
cs_test_t = 'Dataset\\test.cs.json'
    
with open(cs_tester, 'r') as file:
    cs_test = file.readlines()
    
with open(cs_test_t, 'r') as file:
    cs_test_tokenized = json.load(file)
    
with open('Evaluador\\traducciones.java', 'w', encoding='utf-8') as f:
    for sequence, tokenized_sequence in zip(cs_test, cs_test_tokenized):
        traduccion = translate(tokenized_sequence, sequence, model)
        f.write(f'{traduccion}\n') 

