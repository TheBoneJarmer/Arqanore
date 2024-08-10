#include <arqanore/window.h>
#include <arqanore/keyboard.h>
#include <iostream>
#include "arqanore/exceptions.h"
#include "arqanore/model.h"
#include "arqanore/font.h"
#include "arqanore/camera.h"
#include "arqanore/scene.h"
#include "arqanore/renderer.h"

using namespace arqanore;

Model* model;
Font* font;
Vector3 model_rot;

void on_open(Window* window)
{
    window->set_vsync(false);

    try
    {
        font = new Font("assets/fonts/arial.ttf", 20, 20);

        model = new Model("assets/models/axis.arqm");
        model->calculate_normals(false);

        Camera& cam = Scene::cameras[0];
        cam.position.z = -10;
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_close(Window* window)
{
    delete model;
    delete font;
}

void on_update(Window* window, double dt)
{
    auto& sun = Scene::lights[0];

    try
    {
        if (Keyboard::key_pressed(Keys::ESCAPE))
        {
            window->close();
        }

        if (Keyboard::key_pressed(Keys::KP_1)) sun.range--;
        if (Keyboard::key_pressed(Keys::KP_3)) sun.range++;
        if (Keyboard::key_pressed(Keys::KP_7)) sun.strength--;
        if (Keyboard::key_pressed(Keys::KP_9)) sun.strength++;

        if (Keyboard::key_down(Keys::KP_2)) sun.source.z += dt;
        if (Keyboard::key_down(Keys::KP_8)) sun.source.z -= dt;
        if (Keyboard::key_down(Keys::KP_4)) sun.source.x -= dt;
        if (Keyboard::key_down(Keys::KP_6)) sun.source.x += dt;
        if (Keyboard::key_down(Keys::KP_SUBTRACT)) sun.source.y -= dt;
        if (Keyboard::key_down(Keys::KP_ADD)) sun.source.y += dt;

        if (Keyboard::key_pressed(Keys::KP_0))
        {
            if (sun.type == 0)
            {
                sun.type = 1;
            }
            else
            {
                sun.type = 0;
            }
        }

        if (Keyboard::key_pressed(Keys::R))
        {
            sun.type = DIRECTIONAL_LIGHT;
            sun.range = 10;
            sun.strength = 1;
            sun.source = Vector3(0.25f, -0.5f, -0.5f);
        }

        model_rot += dt * 10;
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_render2d(Window* window)
{
    auto& sun = Scene::lights[0];
    auto text_scale = Vector2::ONE;
    auto text_color = Color::WHITE;

    Renderer::render_text(window, font, "FPS: " + std::to_string(window->get_fps()), Vector2(32, 32), text_scale, text_color);
    Renderer::render_text(window, font, "Light.Range: " + std::to_string(sun.range), Vector2(32, 32 * 3), text_scale, text_color);
    Renderer::render_text(window, font, "Light.Strength: " + std::to_string(sun.strength), Vector2(32, 32 * 4), text_scale, text_color);
    Renderer::render_text(window, font, "Light.Source: " + std::to_string(sun.source.x) + "," + std::to_string(sun.source.y) + "," + std::to_string(sun.source.z), Vector2(32, 32 * 5), text_scale, text_color);
    Renderer::render_text(window, font, "Light.Type: " + std::to_string(sun.type), Vector2(32, 32 * 6), text_scale, text_color);
}

void on_render3d(Window* window)
{
    try
    {
        Vector3 pos(0, 0, 0);
        Quaternion rot = Quaternion::rotate(Quaternion(), model_rot);
        Vector3 scale(1, 1, 1);

        Renderer::render_model(window, model, pos, rot, scale, 0);
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

int main()
{
    auto window = Window(1440, 768, "Arqanore");
    window.on_open(on_open);
    window.on_close(on_close);
    window.on_update(on_update);
    window.on_render2d(on_render2d);
    window.on_render3d(on_render3d);
    window.open(false, true, true);

    return 0;
}
