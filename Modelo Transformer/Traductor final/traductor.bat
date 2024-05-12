echo Ejecutando Normalizer
cd Normalizer
call cs_normalizador.exe
cd ..

echo Ejecutando Tokenizer
cd Tokenizer
java -cp . Main
cd ..

echo Ejecutando Tester
cd Tester
python Tester.py
cd ..

echo Todos los programas se han ejecutado
pause
