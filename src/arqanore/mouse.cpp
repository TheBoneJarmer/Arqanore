#include "arqanore/mouse.h"
#include <GLFW/glfw3.h>

unsigned int arqanore::Mouse::states[10];
float arqanore::Mouse::x = 0;
float arqanore::Mouse::y = 0;
float arqanore::Mouse::prev_x = 0;
float arqanore::Mouse::prev_y = 0;
float arqanore::Mouse::move_x = 0;
float arqanore::Mouse::move_y = 0;
int arqanore::Mouse::scroll_x = 0;
int arqanore::Mouse::scroll_y = 0;

void arqanore::Mouse::update() {
    scroll_x = 0;
    scroll_y = 0;
    move_x = 0;
    move_y = 0;

    for (unsigned int &i: states) {
        unsigned int *state = &i;

        if (*state == 1) *state = 2;
        if (*state == 3) *state = 0;
    }
}

float arqanore::Mouse::get_x() {
    return Mouse::x;
}

float arqanore::Mouse::get_y() {
    return Mouse::y;
}

float arqanore::Mouse::get_move_x() {
    return move_x;
}

float arqanore::Mouse::get_move_y() {
    return move_y;
}

int arqanore::Mouse::get_scroll_x() {
    return scroll_x;
}

int arqanore::Mouse::get_scroll_y() {
    return scroll_y;
}

bool arqanore::Mouse::button_down(unsigned int button) {
    return states[button] > 0 && states[button] < 3;
}

bool arqanore::Mouse::button_up(unsigned int button) {
    return states[button] == 3;
}

bool arqanore::Mouse::button_pressed(unsigned int button) {
    return states[button] == 1;
}

void arqanore::Mouse::hide(Window *win) {
    glfwSetInputMode(win->handle, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);
}

void arqanore::Mouse::disable(Window *win) {
    glfwSetInputMode(win->handle, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
}

void arqanore::Mouse::show(Window *win) {
    glfwSetInputMode(win->handle, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
}
