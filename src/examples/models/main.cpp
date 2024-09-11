#include <cmath>
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
bool play = false;

Vector3 camera_rot;
Vector3 camera_pos;

void on_open(Window* window)
{
    window->set_vsync(false);

    try
    {
        Image icon("assets/icon.png");
        window->set_icon(icon);
        window->set_vsync(true);

        font = new Font("assets/fonts/arial.ttf", 20, 20);

        model = new Model("assets/models/cube.arqm");
        model->calculate_normals(false);

        for (auto& mesh : model->meshes)
        {
            mesh.material.ambient = Color(200, 200, 200);
        }

        for (const Bone& bone : model->bones)
        {
            if (bone.frames.size() > max_frames)
            {
                max_frames = bone.frames.size();
                break;
            }
        }

        camera_pos = Vector3(0, 0, -10);
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
        Camera& camera = Scene::cameras[0];

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
            frame = 0;
            frame_timer = 0;
            play = false;
        }

        if (Keyboard::key_pressed(Keys::SPACE))
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

        if (Keyboard::key_down(Keys::W))
        {
            camera_pos.z += std::cos(MathHelper::radians(camera_rot.y)) * dt * 4;
            camera_pos.x -= std::sin(MathHelper::radians(camera_rot.y)) * dt * 4;
        }

        if (Keyboard::key_down(Keys::S))
        {
            camera_pos.z -= std::cos(MathHelper::radians(camera_rot.y)) * dt * 4;
            camera_pos.x += std::sin(MathHelper::radians(camera_rot.y)) * dt * 4;
        }

        if (Keyboard::key_down(Keys::A))
        {
            camera_pos.z += std::cos(MathHelper::radians(camera_rot.y - 90)) * dt * 4;
            camera_pos.x -= std::sin(MathHelper::radians(camera_rot.y - 90)) * dt * 4;
        }

        if (Keyboard::key_down(Keys::D))
        {
            camera_pos.z += std::cos(MathHelper::radians(camera_rot.y + 90)) * dt * 4;
            camera_pos.x -= std::sin(MathHelper::radians(camera_rot.y + 90)) * dt * 4;
        }

        if (Keyboard::key_down(Keys::LEFT))
        {
            camera_rot.y -= dt * 100;
        }

        if (Keyboard::key_down(Keys::RIGHT))
        {
            camera_rot.y += dt * 100;
        }

        if (Keyboard::key_down(Keys::UP))
        {
            camera_rot.x -= dt * 100;
        }

        if (Keyboard::key_down(Keys::DOWN))
        {
            camera_rot.x += dt * 100;
        }

        camera.rotation = Quaternion::rotate(Quaternion(), camera_rot);
        camera.position = camera_pos;

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
    Vector2 text_scale = Vector2::ONE;
    Color text_color = Color::WHITE;

    Renderer::render_text(window, font, "FPS: " + std::to_string(window->get_fps()), Vector2(32, 32), text_scale, text_color);
    Renderer::render_text(window, font, "Frame: " + std::to_string(frame), Vector2(32, 64), text_scale, text_color);
}

void on_render3d(Window* window)
{
    try
    {
        Vector3 pos(0, 0, 0);
        Quaternion rot = Quaternion(0, 0, 0, 1);
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
