#include <iostream>
#include "arqanore/window.h"
#include "arqanore/keyboard.h"
#include "arqanore/renderer.h"
#include "arqanore/font.h"
#include "arqanore/exceptions.h"

arqanore::Font *font;
std::u16string text1;
std::u16string text2;

void on_open(arqanore::Window *window) {
    try {
        font = new arqanore::Font("assets/fonts/default.ttf", 0, 16);
        text1 = u"Hello, this is a normal piece of text! This text will continue to be rendered to the right even if it leaves the screen at some point.";
        text2 = u"And this text contains Unicode characters: Ö ö ó ò Ü ü ú ù ©";
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
    } catch (...) {
        std::cerr << "An unknown error occurred while loading assets" << std::endl;
    }
}

void on_close(arqanore::Window *window) {
    delete font;
}

void on_update(arqanore::Window *window, double at) {
    if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE)) {
        window->set_closed(true);
    }
}

void on_render_2d(arqanore::Window *window) {
    try {
        arqanore::Renderer::render_text(window, font, text1, arqanore::Vector2(32, 32), arqanore::Vector2::ONE, arqanore::Color::WHITE);
        arqanore::Renderer::render_text(window, font, text2, arqanore::Vector2(32, 64), arqanore::Vector2::ONE, arqanore::Color::WHITE);
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        window->set_closed(true);
    } catch (...) {
        std::cerr << "An unknown error occurred while rendering" << std::endl;
        window->set_closed(true);
    }
}

int main() {
    try {
        auto window = arqanore::Window(1440, 768, "Arqanore");
        window.on_open(on_open);
        window.on_close(on_close);
        window.on_update(on_update);
        window.on_render2d(on_render_2d);
        window.open(false, true, true);
    } catch (arqanore::ArqanoreException &ex) {
        std::cerr << ex.what() << std::endl;
        return 1;
    } catch (...) {
        std::cerr << "Unknown error occurred" << std::endl;
        return 2;
    }

    return 0;
}
