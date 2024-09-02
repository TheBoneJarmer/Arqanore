#include <arqanore/window.h>
#include <arqanore/keyboard.h>
#include <iostream>

#include "arqanore/exceptions.h"
#include "arqanore/model.h"
#include "arqanore/font.h"
#include "arqanore/camera.h"
#include "arqanore/mathhelper.h"
#include "arqanore/scene.h"
#include "arqanore/renderer.h"

using namespace arqanore;

Model* model;
Font* font;

int max_frames = 0;
int frame = 0;
double frame_timer = 0;

void on_open(Window* window)
{
    window->set_vsync(false);

    try
    {
        Image icon("assets/icon.png");
        window->set_icon(icon);

        font = new Font("assets/fonts/arial.ttf", 20, 20);

        model = new Model("assets/models/wobble.arqm");
        model->calculate_normals(false);

        Camera& cam = Scene::cameras[0];
        cam.position.z = -10;
        cam.position.y = -3;

        for (const Bone& bone : model->bones)
        {
            if (bone.frames.size() > max_frames)
            {
                max_frames = bone.frames.size();
                break;
            }
        }
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
    try
    {
        if (frame_timer < 10)
        {
            frame_timer += dt;
        }
        else
        {
            frame_timer = 0;
            frame++;
        }

        if (frame == max_frames)
        {
            frame = 0;
        }

        if (Keyboard::key_pressed(Keys::ESCAPE))
        {
            window->close();
        }

        if (Keyboard::key_pressed(Keys::V))
        {
            window->set_vsync(!window->get_vsync());
        }
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

void on_render2d(Window* window)
{
    auto text_scale = Vector2::ONE;
    auto text_color = Color::WHITE;

    Renderer::render_text(window, font, "FPS: " + std::to_string(window->get_fps()), Vector2(32, 32), text_scale, text_color);
}

void on_render3d(Window* window)
{
    try
    {
        Vector3 pos(0, 0, 0);
        Quaternion rot = Quaternion();
        Vector3 scale(1, 1, 1);

        Renderer::render_model(window, model, pos, rot, scale, frame);
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        window->close();
    }
}

int main()
{
    try
    {
        auto window = Window(1440, 768, "Arqanore");
        window.on_open(on_open);
        window.on_close(on_close);
        window.on_update(on_update);
        window.on_render2d(on_render2d);
        window.on_render3d(on_render3d);
        window.open(false, true, true);
    }
    catch (ArqanoreException& ex)
    {
        std::cerr << ex.what() << std::endl;
        return 1;
    }
    catch (...)
    {
        std::cerr << "Unknown error occurred" << std::endl;
        return 2;
    }

    return 0;
}
