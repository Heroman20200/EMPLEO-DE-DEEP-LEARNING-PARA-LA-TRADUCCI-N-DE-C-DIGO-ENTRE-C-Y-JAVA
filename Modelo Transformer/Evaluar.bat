cd Evaluador
cd Formato
@echo Aplicando el mismo formato...
javac *.java
timeout /t 5 /nobreak > NUL
java -cp . Main
cd ..
@echo Ejecutando evaluacion...
Evaluador.py

pause