import json
import sys
import numpy as np
from keras_transformer import get_model
from keras.callbacks import EarlyStopping, ModelCheckpoint, TensorBoard
from tensorflow.keras.optimizers import Adam

csharp_file_path = 'Dataset\\train.cs.json'
java_file_path = 'Dataset\\train.java.json'

with open(csharp_file_path, 'r') as file:
    source_tokens = json.load(file)
    
with open(java_file_path, 'r') as file:
    target_tokens = json.load(file)

source_dict = 'Data\\source_token_dict.json'
target_dict = 'Data\\target_token_dict.json'
target_dict_inv = 'Data\\target_token_dict_inv.json'

with open(source_dict, 'r') as file:
    source_token_dict = json.load(file)

with open(target_dict, 'r') as file:
    target_token_dict = json.load(file)
    
with open(target_dict_inv, 'r') as file:
    target_token_dict_inv = json.load(file)

encoder_tokens = [['START'] + tokens + ['END'] for tokens in source_tokens]
decoder_tokens = [['START'] + tokens + ['END'] for tokens in target_tokens]
output_tokens = [tokens + ['END'] for tokens in target_tokens]

source_max_len = max(map(len, encoder_tokens))
target_max_len = max(map(len, decoder_tokens))

encoder_tokens = [tokens + ['PAD']*(source_max_len-len(tokens)) for tokens in encoder_tokens]
decoder_tokens = [tokens + ['PAD']*(target_max_len-len(tokens)) for tokens in decoder_tokens]
output_tokens = [tokens + ['PAD']*(target_max_len-len(tokens)) for tokens in output_tokens]

Einput = [list(map(lambda x: source_token_dict.get(x, source_token_dict['UNK']), tokens)) for tokens in encoder_tokens]
Dinput = [list(map(lambda x: target_token_dict.get(x, target_token_dict['UNK']), tokens)) for tokens in decoder_tokens]
output_decoded = [list(map(lambda x: target_token_dict.get(x, target_token_dict['UNK']), tokens)) for tokens in output_tokens]

valid_csharp_path = 'Dataset\\valid.cs.json'
valid_java_path = 'Dataset\\valid.java.json'

with open(valid_csharp_path, 'r') as file:
    source_valid = json.load(file)
    
with open(valid_java_path, 'r') as file:
    target_valid = json.load(file)

# Diccionario de validación
encoder_valid = [['START'] + tokens + ['END'] for tokens in source_valid]
decoder_valid = [['START'] + tokens + ['END'] for tokens in target_valid]
output_valid = [tokens + ['END'] for tokens in target_valid]

valid_source_max_len = max(map(len, encoder_valid))
valid_target_max_len = max(map(len, decoder_valid))

encoder_valid = [tokens + ['PAD']*(valid_source_max_len-len(tokens)) for tokens in encoder_valid]
decoder_valid = [tokens + ['PAD']*(valid_target_max_len-len(tokens)) for tokens in decoder_valid]
output_valid = [tokens + ['PAD']*(valid_target_max_len-len(tokens)) for tokens in output_valid]

valid_Einput = [list(map(lambda x: source_token_dict.get(x, source_token_dict['UNK']), tokens)) for tokens in encoder_valid]
valid_Dinput = [list(map(lambda x: target_token_dict.get(x, target_token_dict['UNK']), tokens)) for tokens in decoder_valid]
valid_output_decoded = [list(map(lambda x: target_token_dict.get(x, target_token_dict['UNK']), tokens)) for tokens in output_valid]

# Creación de la red Transformer
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
model.compile(optimizer=Adam(), loss='sparse_categorical_crossentropy')
model.summary()

# Entrenamiento
x = [np.array(Einput), np.array(Dinput)]
y = np.array(output_decoded)

valid_x = [np.array(valid_Einput), np.array(valid_Dinput)]
valid_y = np.array(valid_output_decoded)

# Redirigir stdout a un archivo de logs
log_file = open('logs.txt', 'w')
sys.stdout = log_file

model.fit(
    x, y,
    validation_data=(valid_x, valid_y),
    epochs=60,
    batch_size=32,
    callbacks = [
                    EarlyStopping(monitor='val_loss', patience=3, verbose=1, mode='min', restore_best_weights=True),
                    ModelCheckpoint('traductor.h5', monitor='val_loss', verbose=1, save_best_only=True, mode='min', save_freq='epoch'),
                    TensorBoard(log_dir="tensorboard_logs", histogram_freq=1)
                ]
)
log_file.close()  
model.save_weights('traductor_total.h5')