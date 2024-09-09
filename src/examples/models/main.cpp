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

Vector3 model_rot;

int max_frames = 0;
int frame = 0;
double frame_timer = 0;
bool play = false;

void on_open(Window* window)
{
    window->set_vsync(false);

    try
    {
        Image icon("assets/icon.png");
        window->set_icon(icon);
        window->set_vsync(true);

        font = new Font("assets/fonts/arial.ttf", 20, 20);

        model = new Model("assets/models/wobble.arqm");
        model->calculate_normals(false);

        Camera& cam = Scene::cameras[0];
        cam.position.z = -10;
        //cam.position.y = -3;

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
        if (Keyboard::key_pressed(Keys::ESCAPE))
        {
            window->close();
        }

        if (Keyboard::key_pressed(Keys::V))
        {
            window->set_vsync(!window->get_vsync());
        }

        if (Keyboard::key_pressed(Keys::R))
        {
            model_rot = Vector3(0, 0, 0);
            frame = 0;
        }

        if (Keyboard::key_pressed(Keys::P))
        {
            play = !play;
        }

        if (!play)
        {
            if (Keyboard::key_pressed(Keys::KP_ADD)) frame++;
            if (Keyboard::key_pressed(Keys::KP_SUBTRACT)) frame--;
        }
        else
        {
            if (frame_timer < 1)
            {
                frame_timer++;
            }
            else
            {
                frame_timer = 0;
                frame++;
            }
        }

        if (Keyboard::key_down(Keys::UP)) model_rot.x -= dt * 100;
        if (Keyboard::key_down(Keys::DOWN)) model_rot.x += dt * 100;
        if (Keyboard::key_down(Keys::LEFT)) model_rot.y -= dt * 100;
        if (Keyboard::key_down(Keys::RIGHT)) model_rot.y += dt * 100;

        if (frame == max_frames) frame = 0;
        if (frame == -1) frame = max_frames;
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
    Renderer::render_text(window, font, "Frame: " + std::to_string(frame), Vector2(32, 64), text_scale, text_color);
}

void on_render3d(Window* window)
{
    try
    {
        Vector3 pos(0, 0, 0);
        Quaternion rot = Quaternion::rotate(Quaternion(), model_rot);
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
