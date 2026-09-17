#include <GLUT/glut.h>
#include <OpenGL/gl.h>
#include <iostream>

// Parámetros de configuración de las paletas
const float PADDLE_WIDTH = 4.0f;
const float PADDLE_HEIGHT = 22.0f;

// Posiciones iniciales en el eje Y para los dos jugadores (en 60)
float paddle1_y = 60.0f;
float paddle2_y = 60.0f;

// Dibuja una paleta rectangular a partir de su punto central (x, y)
void draw_paddle(float x, float y) {
    glBegin(GL_QUADS);
        glVertex2f(x - PADDLE_WIDTH / 2.0f, y - PADDLE_HEIGHT / 2.0f);
        glVertex2f(x + PADDLE_WIDTH / 2.0f, y - PADDLE_HEIGHT / 2.0f);
        glVertex2f(x + PADDLE_WIDTH / 2.0f, y + PADDLE_HEIGHT / 2.0f);
        glVertex2f(x - PADDLE_WIDTH / 2.0f, y + PADDLE_HEIGHT / 2.0f);
    glEnd();
}

// Renderiza una red punteada al centro de la cancha
void draw_field() {
    glColor3f(0.4f, 0.4f, 0.4f); // Color gris para la red
    glLineWidth(2.0f);
    
    glBegin(GL_LINES);
    for (float y = 0.0f; y < 120.0f; y += 6.0f) {
        glVertex2f(80.0f, y);
        glVertex2f(80.0f, y + 3.0f);
    }
    glEnd();
}

// Funcion de renderizado
// Dibujado del escenario y elementos
void Display(void) {
    // Limpiamos la pantalla con el color de fondo 
    glClear(GL_COLOR_BUFFER_BIT);

    // Dibujamos la cancha detrás de los elementos
    draw_field();

    // Dibujamos la paleta izquierda (Jugador 1) y derecha (Jugador 2)
    glColor3f(1.0f, 1.0f, 1.0f);
    draw_paddle(8.0f, paddle1_y);
    draw_paddle(152.0f, paddle2_y);

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
    glutCreateWindow("Pong Game");

    init();

    // Registro de callbacks 
    glutDisplayFunc(Display);
    glutReshapeFunc(reshape);

    // Bucle principal
    glutMainLoop();
    return 0;
}