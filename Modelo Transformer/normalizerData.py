import subprocess
from queue import Queue

# Define la ruta al archivo .exe
ruta_al_exe = 'Data\\normalizedCollector\\normalizedCollector.exe'

def normalizeCollector(codigo):
    # Ejecuta el .exe con el código como argumento y captura la salida
    resultado = subprocess.run([ruta_al_exe, codigo], capture_output=True, text=True, encoding='utf-8', errors='replace')
    # Obtiene los datos originales y los divide por líneas
    salida_normalizada = resultado.stdout.strip()
    lineas = salida_normalizada.split('\n')
    
    metodos = Queue()
    variables = Queue()
    literales = Queue()
    
    if len(lineas) > 0:
        cadenas = lineas[0].split()
        for cadena in cadenas:
            metodos.put(cadena)
        
    if len(lineas) > 1:
        cadenas = lineas[1].split()
        for cadena in cadenas:
            variables.put(cadena)
            
    if len(lineas) > 2:
        cadenas = lineas[2].split()
        current_string = False
        str_cadena = ""
        for cadena in cadenas:
            if '"' in cadena:
                if cadena.count('"') == 2 and cadena[0] == '"' and cadena[-1] == '"':
                    literales.put(cadena)
                    
                else:
                    if not current_string:
                        str_cadena = cadena
                        current_string = True                  
                    else:
                        current_string = False
                        literales.put(str_cadena + " " + cadena)
                        str_cadena = ""
                
            else:
                if not current_string:
                    literales.put(cadena)
                else:
                    str_cadena += " " + cadena  

    return metodos, variables, literales
