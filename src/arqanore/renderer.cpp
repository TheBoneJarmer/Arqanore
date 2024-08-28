#include <cmath>
#include <iostream>
#include "glad/gl.h"
#include "arqanore/renderer.h"
#include "arqanore/shaders.h"
#include "arqanore/mathhelper.h"
#include "arqanore/exceptions.h"
#include "arqanore/font.h"
#include "arqanore/model.h"
#include "arqanore/polygon.h"
#include "arqanore/utils.h"
#include "arqanore/scene.h"

using namespace arqanore;

Shader* Renderer::shader;
Shader* Renderer::shader_font;
Shader* Renderer::shader_polygon;
Shader* Renderer::shader_sprite;
Shader* Renderer::shader_model;

void Renderer::reset()
{
    shader = nullptr;
    shader_font = &Shaders::font;
    shader_polygon = &Shaders::polygon;
    shader_sprite = &Shaders::sprite;
    shader_model = &Shaders::model;

    glUseProgram(0);
}

Matrix4 Renderer::generate_model_matrix(Vector3 pos, Quaternion rot, Vector3 scl)
{
    Matrix4 mat = Matrix4::identity();
    mat = Matrix4::scale(mat, scl);
    mat = Matrix4::translate(mat, pos);
    mat = Matrix4::rotate(mat, rot);

    return mat;
}

Matrix4 Renderer::generate_view_matrix(Camera& camera)
{
    Vector3& pos = camera.position;
    Quaternion& rot = camera.rotation;

    Matrix4 mat = Matrix4::identity();
    mat = Matrix4::rotate(mat, rot);
    mat = Matrix4::translate(mat, pos);

    return mat;
}

Matrix4 Renderer::generate_projection_matrix(Camera& camera, Window* window)
{
    float& fov = camera.fov;
    float width = static_cast<float>(window->get_width());
    float height = static_cast<float>(window->get_height());
    float& near = camera.near;
    float& far = camera.far;

    Matrix4 mat = Matrix4::identity();
    mat = Matrix4::perspective(mat, fov, width / height, near, far);

    return mat;
}

void Renderer::set_shader(Shader* ptr, unsigned int target)
{
    if (ptr == nullptr)
    {
        throw ArqanoreException("Shader cannot be null");
    }

    if (target == RENDER_TARGET_FONT)
    {
        shader_font = ptr;
        return;
    }

    if (target == RENDER_TARGET_POLYGON)
    {
        shader_polygon = ptr;
        return;
    }

    if (target == RENDER_TARGET_SPRITE)
    {
        shader_sprite = ptr;
        return;
    }

    if (target == RENDER_TARGET_MODEL)
    {
        shader_model = ptr;
        return;
    }

    throw ArqanoreException("Unknown render3D target " + std::to_string(target));
}

bool Renderer::switch_shader(Shader* ptr)
{
    if (ptr == nullptr)
    {
        return false;
    }

    if (shader == ptr)
    {
        return false;
    }

    shader = ptr;
    glUseProgram(ptr->id);
    return true;
}

void Renderer::render_text(Window* window, Font* font, std::u16string text, Vector2 position, Vector2 scale, Color color)
{
    switch_shader(shader_font);

    if (window == nullptr)
    {
        throw ArqanoreException("Window is null");
    }

    if (font == nullptr)
    {
        throw ArqanoreException("Font is null");
    }

    int advance = 0;

    shader->set_uniform_2f("u_resolution", window->get_width(), window->get_height());
    shader->set_uniform_2f("u_rotation", 0, 1);
    shader->set_uniform_vec2("u_scale", scale);
    shader->set_uniform_rgba("u_color", color);

    glBindVertexArray(font->vao);

    for (unsigned int c : text)
    {
        auto glyph = font->glyph(c);

        if (glyph == nullptr)
        {
            continue;
        }

        float glyph_left = glyph->left * scale.x;
        float glyph_top = glyph->top * scale.y;
        float glyph_height = font->pixel_height * scale.y;
        long glyph_advance = glyph->advance * scale.x;

        float text_x = position.x + glyph_left + advance;
        float text_y = position.y - glyph_top + glyph_height;

        shader->set_uniform_2f("u_translation", text_x, text_y);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, glyph->id);
        glDrawArrays(GL_TRIANGLES, c * 6, 6);
        glBindTexture(GL_TEXTURE_2D, 0);

        advance += glyph_advance >> 6;
    }

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, 0);
    glBindVertexArray(0);
}

void Renderer::render_text(Window* window, Font* font, std::string text, Vector2 position, Vector2 scale, Color color)
{
    switch_shader(shader_font);

    if (window == nullptr)
    {
        throw ArqanoreException("Window is null");
    }

    if (font == nullptr)
    {
        throw ArqanoreException("Font is null");
    }

    int advance = 0;

    shader->set_uniform_2f("u_resolution", window->get_width(), window->get_height());
    shader->set_uniform_2f("u_rotation", 0, 1);
    shader->set_uniform_vec2("u_scale", scale);
    shader->set_uniform_rgba("u_color", color);

    glBindVertexArray(font->vao);

    for (unsigned int c : text)
    {
        auto glyph = font->glyph(c);

        if (glyph == nullptr)
        {
            continue;
        }

        float glyph_left = glyph->left * scale.x;
        float glyph_top = glyph->top * scale.y;
        float glyph_height = font->pixel_height * scale.y;
        long glyph_advance = glyph->advance * scale.x;

        float text_x = position.x + glyph_left + advance;
        float text_y = position.y - glyph_top + glyph_height;

        shader->set_uniform_2f("u_translation", text_x, text_y);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, glyph->id);
        glDrawArrays(GL_TRIANGLES, c * 6, 6);
        glBindTexture(GL_TEXTURE_2D, 0);

        advance += glyph_advance >> 6;
    }

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, 0);
    glBindVertexArray(0);
}

void Renderer::render_polygon(Window* window, Polygon* polygon, Texture* texture, Vector2 position, Vector2 scale, Vector2 origin, Vector2 offset, float angle, bool flip_hor, bool flip_vert, Color color)
{
    switch_shader(shader_polygon);

    if (window == nullptr)
    {
        throw ArqanoreException("Window is null");
    }

    if (polygon == nullptr)
    {
        throw ArqanoreException("Polygon is null");
    }

    if (texture == nullptr && (flip_hor || flip_vert))
    {
        throw ArqanoreException("Unable to flip texture because texture is null");
    }

    float angle_cos = std::cos(MathHelper::radians(angle + 90));
    float angle_sin = std::sin(MathHelper::radians(angle + 90));

    glBindVertexArray(polygon->vao);
    shader->set_uniform_2f("u_resolution", window->get_width(), window->get_height());
    shader->set_uniform_2f("u_rotation", angle_cos, angle_sin);
    shader->set_uniform_vec2("u_translation", position);
    shader->set_uniform_vec2("u_scale", scale);
    shader->set_uniform_vec2("u_origin", origin);
    shader->set_uniform_vec2("u_offset", offset);
    shader->set_uniform_1i("u_flip_hor", flip_hor);
    shader->set_uniform_1i("u_flip_vert", flip_vert);
    shader->set_uniform_rgba("u_color", color);
    shader->set_uniform_1i("u_texture_active", texture != nullptr);

    if (texture != nullptr)
    {
        shader->set_texture(0, texture);
    }

    glDrawArrays(GL_TRIANGLES, 0, polygon->vertices.size() / 2);
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, 0);
    glBindVertexArray(0);
}

void Renderer::render_sprite(Window* window, Sprite* sprite, Vector2 position, Vector2 scale, Vector2 origin, float angle, int frame_hor, int frame_vert, bool flip_hor, bool flip_vert, Color color)
{
    switch_shader(shader_sprite);

    if (window == nullptr)
    {
        throw ArqanoreException("Window is null");
    }

    if (sprite == nullptr)
    {
        throw ArqanoreException("Sprite is null");
    }

    float angle_cos = std::cos(MathHelper::radians(angle + 90));
    float angle_sin = std::sin(MathHelper::radians(angle + 90));
    float offset_x = (1.0f / sprite->get_frames_hor()) * frame_hor;
    float offset_y = (1.0f / sprite->get_frames_vert()) * frame_vert;

    glBindVertexArray(sprite->vao);

    shader->set_uniform_2f("u_resolution", window->get_width(), window->get_height());
    shader->set_uniform_2f("u_rotation", angle_cos, angle_sin);
    shader->set_uniform_vec2("u_translation", position);
    shader->set_uniform_vec2("u_scale", scale);
    shader->set_uniform_vec2("u_origin", origin);
    shader->set_uniform_2f("u_offset", offset_x, offset_y);
    shader->set_uniform_1i("u_flip_hor", flip_hor);
    shader->set_uniform_1i("u_flip_vert", flip_vert);
    shader->set_uniform_rgba("u_color", color);
    shader->set_sprite(0, sprite);

    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindTexture(GL_TEXTURE_2D, 0);
    glBindVertexArray(0);
}

void Renderer::render_model(Window* window, Model* model, Vector3 position, Quaternion rotation, Vector3 scale, int frame)
{
    if (Scene::active_camera >= Scene::cameras.size())
    {
        return;
    }

    if (window == nullptr)
    {
        throw ArqanoreException("Window is null");
    }

    if (model == nullptr)
    {
        throw ArqanoreException("Model is null");
    }

    if (switch_shader(shader_model))
    {
        Camera& camera = Scene::cameras[Scene::active_camera];
        Matrix4 view_matrix = generate_view_matrix(camera);
        Vector3& view_position = camera.position;
        Matrix4 projection_matrix = generate_projection_matrix(camera, window);

        shader->set_uniform_mat4("u_projection_matrix", projection_matrix);
        shader->set_uniform_mat4("u_view_matrix", view_matrix);
        shader->set_uniform_vec3("u_view_pos", view_position);

        // Process all lights
        shader->set_uniform_1i("u_light_count", Scene::lights.size());

        for (int i = 0; i < Scene::lights.size(); i++)
        {
            Light& light = Scene::lights[i];
            std::string index = std::to_string(i);

            shader->set_uniform_1i("u_light[" + index + "].type", light.type);
            shader->set_uniform_1f("u_light[" + index + "].strength", light.strength);
            shader->set_uniform_1f("u_light[" + index + "].range", light.range);
            shader->set_uniform_1i("u_light[" + index + "].enabled", light.enabled);
            shader->set_uniform_vec3("u_light[" + index + "].source", light.source);
            shader->set_uniform_rgb("u_light[" + index + "].color", light.color);
        }
    }

    // Render mesh per mesh
    for (Mesh& mesh : model->meshes)
    {
        Color& mat_color = mesh.material.color;
        Color& mat_ambient = mesh.material.ambient;
        Color& mat_diffuse = mesh.material.diffuse;
        Color& mat_specular = mesh.material.specular;
        float& mat_shininess = mesh.material.shininess;
        Texture* mat_diffuse_map = mesh.material.diffuse_map;
        Texture* mat_ambient_map = mesh.material.ambient_map;
        Texture* mat_specular_map = mesh.material.specular_map;
        Matrix4 mesh_matrix = generate_model_matrix(mesh.location, mesh.rotation, mesh.scale);
        Matrix4 model_matrix = generate_model_matrix(position, rotation, scale);

        shader->set_uniform_mat4("u_model_matrix", model_matrix);
        shader->set_uniform_mat4("u_mesh_matrix", mesh_matrix);
        shader->set_uniform_rgb("u_material.color", mat_color);
        shader->set_uniform_rgb("u_material.ambient", mat_ambient);
        shader->set_uniform_rgb("u_material.diffuse", mat_diffuse);
        shader->set_uniform_rgb("u_material.specular", mat_specular);
        shader->set_uniform_1f("u_material.shininess", mat_shininess);
        shader->set_uniform_1i("u_material.diffuse_map_active", mat_diffuse_map != nullptr);
        shader->set_uniform_1i("u_material.ambient_map_active", mat_ambient_map != nullptr);
        shader->set_uniform_1i("u_material.specular_map_active", mat_specular_map != nullptr);

        if (mat_diffuse_map != nullptr)
        {
            shader->set_texture(0, mat_diffuse_map);
        }

        if (mat_ambient_map != nullptr)
        {
            shader->set_texture(1, mat_ambient_map);
        }

        if (mat_specular_map != nullptr)
        {
            shader->set_texture(2, mat_specular_map);
        }

        glBindVertexArray(mesh.vao);
        glDrawElements(GL_TRIANGLES, mesh.indices.size(), GL_UNSIGNED_INT, nullptr);
        glBindVertexArray(0);

        for (int i = 0; i < 2; i++)
        {
            glActiveTexture(GL_TEXTURE0 + i);
            glBindTexture(GL_TEXTURE_2D, 0);
        }
    }
}
