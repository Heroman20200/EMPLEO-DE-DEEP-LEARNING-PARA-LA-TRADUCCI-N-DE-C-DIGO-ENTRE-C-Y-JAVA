from difflib import SequenceMatcher

def compute_similarity(reference_lines, translation_lines):
    # Convertir las líneas de referencia y traducción en cadenas de texto
    reference_text = "\n".join(reference_lines)
    translation_text = "\n".join(translation_lines)
    
    # Calcular la similitud entre las cadenas de texto
    matcher = SequenceMatcher(None, reference_text, translation_text)
    similarity_ratio = matcher.ratio() * 100
    
    return round(similarity_ratio)

def compute_similarity_line_by_line(reference_lines, translation_lines):
    total_similarity = 0
    num_lines = min(len(reference_lines), len(translation_lines))

    # Comparar línea por línea
    for ref_line, trans_line in zip(reference_lines[:num_lines], translation_lines[:num_lines]):
        similarity = compute_similarity([ref_line], [trans_line])
        total_similarity += similarity

    # Calcular el promedio de similitud
    average_similarity = total_similarity / num_lines
    return round(average_similarity, 2)

with open("test_formateado.java", "r") as file_ref:
    reference_lines = file_ref.readlines()

with open("traducciones_formateado.java", "r") as file_trans:
    translation_lines = file_trans.readlines()
    
similarity = compute_similarity_line_by_line(reference_lines, translation_lines)
print(f"Porcentaje de similitud: {similarity}%")
