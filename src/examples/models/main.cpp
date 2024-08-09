#include <arqanore/window.h>
#include <arqanore/keyboard.h>
#include <iostream>
#include "arqanore/exceptions.h"
#include "arqanore/model.h"
#include "arqanore/font.h"
#include "arqanore/camera.h"
#include "arqanore/scene.h"
#include "arqanore/renderer.h"

arqanore::Model* model;
arqanore::Font* font;
arqanore::Vector3 model_rot;

void on_open(arqanore::Window* window)
{
    window->set_vsync(false);

    try
    {
        font = new arqanore::Font("assets/fonts/default.ttf", 20, 20);

        model = new arqanore::Model("assets/models/axis.arqm");
        model->calculate_normals(false);

        arqanore::Camera& cam = arqanore::Scene::cameras[0];
        cam.position.z = -10;
    }
    catch (arqanore::ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_close(arqanore::Window* window)
{
    delete model;
    delete font;
}

void on_update(arqanore::Window* window, double dt)
{
    try
    {
        if (arqanore::Keyboard::key_pressed(arqanore::Keys::ESCAPE))
        {
            window->close();
        }

        model_rot += dt * 10;
    }
    catch (arqanore::ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_render2d(arqanore::Window* window)
{
    auto text = "FPS: " + std::to_string(window->get_fps());
    auto text_pos = arqanore::Vector2(32, 32);
    auto text_scale = arqanore::Vector2::ONE;
    auto text_color = arqanore::Color::WHITE;

    arqanore::Renderer::render_text(window, font, text, text_pos, text_scale, text_color);
}

void on_render3d(arqanore::Window* window)
{
    try
    {
        arqanore::Vector3 pos(0, 0, 0);
        arqanore::Quaternion rot = arqanore::Quaternion::rotate(arqanore::Quaternion(), model_rot);
        arqanore::Vector3 scale(1, 1, 1);

        arqanore::Renderer::render_model(window, model, pos, rot, scale, 0);
    }
    catch (arqanore::ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

int main()
{
    auto window = arqanore::Window(1440, 768, "Arqanore");
    window.on_open(on_open);
    window.on_close(on_close);
    window.on_update(on_update);
    window.on_render2d(on_render2d);
    window.on_render3d(on_render3d);
    window.open(false, true, true);

    return 0;
}
