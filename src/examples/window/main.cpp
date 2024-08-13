#include <arqanore/window.h>
#include <arqanore/keyboard.h>

bool closed;

void on_open(arqanore::Window *window) {
    arqanore::Image icon("assets/icon.png");
    window->set_icon(icon);

    closed = false;
}

void on_close(arqanore::Window *window) {
    if (!closed) {
        window->set_closed(false);
    }
}

void on_update(arqanore::Window *window, double at) {
    if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE)) {
        window->set_closed(true);
        closed = true;
    }

    if (arqanore::Keyboard::key_pressed(arqanore::Keys::M)) {
        if (arqanore::Keyboard::key_down(arqanore::Keys::LEFT_ALT)) {
            window->maximize();
        } else {
            window->minimize();
        }
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
    window.open(false, false, true);

    return 0;
}