#include <iostream>
#include <arqanore/window.h>
#include "arqanore/keyboard.h"
#include "arqanore/sprite.h"
#include "arqanore/renderer.h"
#include "arqanore/exceptions.h"

arqanore::Sprite *sprite;
arqanore::Vector2 position;
arqanore::Vector2 scale;

int frame_hor = 0;
int frame_vert = 0;
double frame_time = 0;
int direction = 2;

void on_open(arqanore::Window *window) {
    try {
        arqanore::Image icon("assets/icon.png");
        window->set_icon(icon);

        sprite = new arqanore::Sprite("assets/sprites/player.png", 16, 16);

        position = arqanore::Vector2(64, 64);
        scale = arqanore::Vector2(4, 4);
        frame_hor = direction * 4;
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_close(arqanore::Window *window) {
    delete sprite;
}

void on_update(arqanore::Window *window, double dt) {
    auto speed = dt * 100;

    if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE)) {
        window->close();
    }

    if (frame_time < 16) {
        frame_time += dt * 100;
    } else {
        frame_time = 0;
        frame_hor++;
    }

    if (frame_hor >= direction * 4 + 4) {
        frame_hor = direction * 4;
    }

    if (frame_hor < direction * 4) {
        frame_hor = direction * 4;
    }

    if (direction == 1) position.y -= speed;
    if (direction == 0) position.y += speed;
    if (direction == 2) position.x += speed;
    if (direction == 3) position.x -= speed;

    if (direction == 2 && position.x >= window->get_width() - 128) {
        direction = 0;
        position.x = window->get_width() - 128;
    }

    if (direction == 0 && position.y >= window->get_height() - 128) {
        direction = 3;
        position.y = window->get_height() - 128;
    }

    if (direction == 3 && position.x < 128) {
        direction = 1;
        position.x = 128;
    }

    if (direction == 1 && position.y < 128) {
        direction = 2;
        position.y = 128;
    }
}

void on_render_2d(arqanore::Window *window) {
    try {
        arqanore::Renderer::render_sprite(window, sprite, position, scale, arqanore::Vector2::ZERO, 0, frame_hor, frame_vert, false, false, arqanore::Color::WHITE);
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

int main() {
    auto window = arqanore::Window(1440, 768, "Arqanore - Sprites");
    window.on_open(on_open);
    window.on_close(on_close);
    window.on_update(on_update);
    window.on_render2d(on_render_2d);
    window.open(false, true, true);

    return 0;
}