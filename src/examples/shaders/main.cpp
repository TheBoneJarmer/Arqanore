#include "arqanore/window.h"
#include "arqanore/keyboard.h"
#include "arqanore/shader.h"
#include "arqanore/sprite.h"
#include "arqanore/renderer.h"
#include "arqanore/exceptions.h"
#include <iostream>

arqanore::Shader* shader;
arqanore::Sprite* sprite;
arqanore::Vector2 position;
arqanore::Vector2 scale;

int frame_hor;
int frame_vert;
int frame_time;

void on_open(arqanore::Window *window) {
    try {
        arqanore::Image icon("assets/icon.png");
        window->set_icon(icon);

        shader = new arqanore::Shader();
        shader->add_vertex("assets/shaders/sprity_v.glsl", SHADER_SOURCE_TYPE_FILE);
        shader->add_fragment("assets/shaders/sprity_f.glsl", SHADER_SOURCE_TYPE_FILE);
        shader->compile();

        sprite = new arqanore::Sprite("assets/sprites/player.png", 16, 16);

        position = arqanore::Vector2(64, 64);
        scale = arqanore::Vector2(4, 4);

        frame_hor = 0;
        frame_time = 0;
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_close(arqanore::Window *window) {
    delete shader;
}

void on_update(arqanore::Window *window, double at) {
    if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE)) {
        window->close();
    }

    if (frame_time < 16) {
        frame_time++;
    } else {
        frame_time = 0;
        frame_hor++;
    }

    if (frame_hor == sprite->get_frames_hor()) {
        frame_hor = 0;
    }
}

void on_render(arqanore::Window *window) {
    try {
        arqanore::Renderer::set_shader(shader, RENDER_TARGET_SPRITE);
        arqanore::Renderer::render_sprite(window, sprite, position, scale, arqanore::Vector2::ZERO, 0, frame_hor, frame_vert, false, false, arqanore::Color::WHITE);
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

int main() {
    auto window = arqanore::Window(1440, 768, "Arqanore");
    window.on_open(on_open);
    window.on_close(on_close);
    window.on_update(on_update);
    window.on_render(on_render);
    window.open(false, false, true);

    return 0;
}
