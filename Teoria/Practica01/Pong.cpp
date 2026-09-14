#include <GLUT/glut.h>
#include <OpenGL/gl.h>
#include <iostream>

// Funcion de renderizado
void Display(void) {
    // Limpiamos la pantalla con el color de fondo 
    glClear(GL_COLOR_BUFFER_BIT);

    // buffers para evitar parpadeos
    glutSwapBuffers();
    glutPostRedisplay();
}

// sistema de coordenadas al redimensionar la ventana
void reshape(int w, int h) {
    glViewport(0, 0, (GLsizei)w, (GLsizei)h);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();

    // sistema de coordenadas en 160x120
    gluOrtho2D(0.0, 160.0, 0.0, 120.0);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
}

// Configuración inicial 
void init(void) {
    // color de fondo (Negro)
    glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
}

int main(int argc, char* argv[]) {
    // GLUT
    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
    glutInitWindowSize(640, 480);
    glutCreateWindow("Pong Game - OpenGL");

    init();

    // Registro de callbacks 
    glutDisplayFunc(Display);
    glutReshapeFunc(reshape);

    // Bucle principal
    glutMainLoop();
    return 0;
}