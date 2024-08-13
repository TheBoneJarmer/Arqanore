#include <arqanore/window.h>
#include <arqanore/keyboard.h>
#include "arqanore/sound.h"

arqanore::Sound* sound;

void on_open(arqanore::Window *window) {
    arqanore::Image icon("assets/icon.png");
    window->set_icon(icon);

    sound = new arqanore::Sound("assets/sounds/08_sea.wav");
}

void on_close(arqanore::Window *window) {
    sound->stop();
    delete sound;
}

void on_update(arqanore::Window *window, double at) {
    if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE)) {
        window->set_closed(true);
    }

    if (arqanore::Keyboard::key_pressed(arqanore::Keys::SPACE)) {
        sound->play();
        sound->looping(true);
    }
}

void on_render2d(arqanore::Window *window) {

}

void on_render3d(arqanore::Window *window) {

}

int main() {
    auto window = arqanore::Window(1440, 768, "Arqanore");
    window.on_open(on_open);
    window.on_close(on_close);
    window.on_update(on_update);
    window.on_render2d(on_render2d);
    window.on_render3d(on_render3d);
    window.open(false, true, true);

    return 0;
}