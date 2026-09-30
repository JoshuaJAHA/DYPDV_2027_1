# Tarea 1: Juego Pong 

## Herramientas Utilizadas
* **Editor de Código:** VS Code para la edición de código C++
* **Sistema Operativo:** macOS.
* **Compilador:** `clang++` / `g++` mediante Xcode Command Line Tools 
* **Gráficos:** Frameworks nativos de macOS `OpenGL.framework` y `GLUT.framework`.
 En macOS, las librerías de gráficos OpenGL y GLUT vienen preinstaladas en el sistema, por lo que no requieren dependencias externas.

---

## Requisitos Previos
Tener instaladas las **Xcode Command Line Tools** en macOS 

---

## Compilación y Ejecución (macOS)

### 1. Compilación
Abre la terminal integrada de VS Code en la carpeta `Teoria/Practica01/` y ejecuta:

clang++ -std=c++11 Pong.cpp -o Pong -framework OpenGL -framework GLUT -Wno-deprecated-declarations

* `-framework OpenGL` y `-framework GLUT`: Enlazan las librerías gráficas nativas de macOS.
* `-Wno-deprecated-declarations`: Silencia advertencias sobre funciones antiguas de GLUT en versiones recientes de macOS.

### 2. Ejecución
Una vez generado el ejecutable `Pong`, corre en la terminal:

./Pong

---

## Controles del Juego

* **Jugador 1 (Paleta Izquierda):**
  * Tecla Arriba: `W` / `w`
  * Tecla Abajo: `S` / `s`
* **Jugador 2 (Paleta Derecha):**
  * Tecla Arriba: `Flecha Arriba`
  * Tecla Abajo: `Flecha Abajo`

---

