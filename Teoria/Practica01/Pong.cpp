#include <GLUT/glut.h>
#include <OpenGL/gl.h>
#include <iostream>

// Parámetros de configuración de las paletas

const float PI = 3.1415926535898f; //PI
const float PADDLE_WIDTH = 4.0f;
const float PADDLE_HEIGHT = 22.0f;
const float PADDLE_SPEED = 3.0f; // Velocidad de movimiento
const float BALL_RADIUS = 3.0f; // Radio de la pelota

// Posiciones iniciales en el eje Y para los dos jugadores (en 60)
float paddle1_y = 60.0f;
float paddle2_y = 60.0f;

// Estado de la pelota
float ball_x = 80.0f;
float ball_y = 60.0f;
float ball_dir_x = 1.2f;
float ball_dir_y = 0.8f;

// Puntuaciones de los jugadores 
int score_player1 = 0;
int score_player2 = 0;

// Función para dibujar un círculo 
void draw_circle(float cx, float cy, float r) {
    glBegin(GL_POLYGON);
    for (int i = 0; i < 30; i++) {
        float theta = 2.0f * PI * float(i) / 30.0f;
        glVertex2f(cx + r * cosf(theta), cy + r * sinf(theta));
    }
    glEnd();
}

void reset_ball(int winner) {
    ball_x = 80.0f;
    ball_y = 60.0f;
    // sale en direccion al jugador que recibio el gol
    ball_dir_x = (winner == 1) ? 1.2f : -1.2f;
    ball_dir_y = 0.8f;

    std::cout << "--- ANOTACION ---" << std::endl;
    std::cout << "Puntaje actual --> Jugador 1: " << score_player1 << " | Jugador 2: " << score_player2 << std::endl;
    std::cout << "-----------------" << std::endl;
}

// registrar teclas mantenidas presionadas 
bool keys[256] = { false };
bool specialKeys[256] = { false };

// Dibuja una paleta rectangular a partir de su punto central (x, y)
void draw_paddle(float x, float y) {
    glBegin(GL_QUADS);
        glVertex2f(x - PADDLE_WIDTH / 2.0f, y - PADDLE_HEIGHT / 2.0f);
        glVertex2f(x + PADDLE_WIDTH / 2.0f, y - PADDLE_HEIGHT / 2.0f);
        glVertex2f(x + PADDLE_WIDTH / 2.0f, y + PADDLE_HEIGHT / 2.0f);
        glVertex2f(x - PADDLE_WIDTH / 2.0f, y + PADDLE_HEIGHT / 2.0f);
    glEnd();
}

void update_game() {
    // Controles Jugador 1
    if (keys['w'] || keys['W']) paddle1_y += PADDLE_SPEED;
    if (keys['s'] || keys['S']) paddle1_y -= PADDLE_SPEED;

    // Controles Jugador 2
    if (specialKeys[GLUT_KEY_UP]) paddle2_y += PADDLE_SPEED;
    if (specialKeys[GLUT_KEY_DOWN]) paddle2_y -= PADDLE_SPEED;

    // Restringir paleta 1 a los bordes 
    if (paddle1_y - PADDLE_HEIGHT / 2.0f < 0.0f) paddle1_y = PADDLE_HEIGHT / 2.0f;
    if (paddle1_y + PADDLE_HEIGHT / 2.0f > 120.0f) paddle1_y = 120.0f - PADDLE_HEIGHT / 2.0f;

    // Restringir paleta 2 a los bordes 
    if (paddle2_y - PADDLE_HEIGHT / 2.0f < 0.0f) paddle2_y = PADDLE_HEIGHT / 2.0f;
    if (paddle2_y + PADDLE_HEIGHT / 2.0f > 120.0f) paddle2_y = 120.0f - PADDLE_HEIGHT / 2.0f;

    // Movimiento pelota
    ball_x += ball_dir_x;
    ball_y += ball_dir_y;

    // Rebote superior e inferior
    if (ball_y + BALL_RADIUS >= 120.0f || ball_y - BALL_RADIUS <= 0.0f) {
        ball_dir_y = -ball_dir_y;
    }

    // Colision con Paleta 1 (Izquierda)
    float p1_right = 8.0f + PADDLE_WIDTH / 2.0f;
    if (ball_x - BALL_RADIUS <= p1_right && ball_x >= 8.0f - PADDLE_WIDTH / 2.0f) {
        if (ball_y >= paddle1_y - PADDLE_HEIGHT / 2.0f && ball_y <= paddle1_y + PADDLE_HEIGHT / 2.0f) {
            ball_dir_x = std::abs(ball_dir_x); // Invertir hacia la derecha
        }
    }

    // Colision con Paleta 2 (Derecha)
    float p2_left = 152.0f - PADDLE_WIDTH / 2.0f;
    if (ball_x + BALL_RADIUS >= p2_left && ball_x <= 152.0f + PADDLE_WIDTH / 2.0f) {
        if (ball_y >= paddle2_y - PADDLE_HEIGHT / 2.0f && ball_y <= paddle2_y + PADDLE_HEIGHT / 2.0f) {
            ball_dir_x = -std::abs(ball_dir_x); // Invertir hacia la izquierda
        }
    }

    // Logica de anotacion de puntos, que se imprime en terminal 
    if (ball_x < 0.0f) {
        score_player2++;
        reset_ball(2);
    } else if (ball_x > 160.0f) {
        score_player1++;
        reset_ball(1);
    }
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

    // Actualizamos posiciones de paletas en cada frame
    update_game();

    // Dibujamos la cancha detrás de los elementos
    draw_field();

    // Dibujamos la paleta izquierda (Jugador 1) y derecha (Jugador 2)
    glColor3f(1.0f, 1.0f, 1.0f);
    draw_paddle(8.0f, paddle1_y);
    draw_paddle(152.0f, paddle2_y);

    // Dibujar pelota en amarillo
    glColor3f(1.0f, 0.85f, 0.2f);
    draw_circle(ball_x, ball_y, BALL_RADIUS);

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
    std::cout << "------------------------------------------" << std::endl;
    std::cout << "        JUEGO PONG INICIANDO...            " << std::endl;
    std::cout << " Jugador 1: Teclas W / S                  " << std::endl;
    std::cout << " Jugador 2: Flecha Arriba / Flecha Abajo  " << std::endl;
    std::cout << "------------------------------------------" << std::endl;
}

void keyDown(unsigned char key, int x, int y) { keys[key] = true; }
void keyUp(unsigned char key, int x, int y) { keys[key] = false; }
void specialKeyDown(int key, int x, int y) { specialKeys[key] = true; }
void specialKeyUp(int key, int x, int y) { specialKeys[key] = false; }

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

    // Registro de callbacks de teclado para controles
    glutKeyboardFunc(keyDown);
    glutKeyboardUpFunc(keyUp);
    glutSpecialFunc(specialKeyDown);
    glutSpecialUpFunc(specialKeyUp);

    // Bucle principal
    glutMainLoop();
    return 0;
}