#include <cmath>
#include "glad/gl.h"
#include "arqanore/renderer.h"
#include "arqanore/shaders.h"
#include "arqanore/mathhelper.h"
#include "arqanore/exceptions.h"
#include "arqanore/font.h"
#include "arqanore/polygon.h"
#include "arqanore/utils.h"
#include "arqanore/shader.h"

arqanore::Shader* arqanore::Renderer::shader;
arqanore::Shader* arqanore::Renderer::shader_font;
arqanore::Shader* arqanore::Renderer::shader_polygon;
arqanore::Shader* arqanore::Renderer::shader_sprite;

void arqanore::Renderer::reset()
{
    shader = nullptr;
    shader_font = &Shaders::font;
    shader_polygon = &Shaders::polygon;
    shader_sprite = &Shaders::sprite;

    glUseProgram(0);
}

void arqanore::Renderer::set_shader(Shader* ptr, unsigned int target)
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

    throw ArqanoreException("Unknown render target " + std::to_string(target));
}

bool arqanore::Renderer::switch_shader(Shader* ptr)
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

void arqanore::Renderer::render_text(Window* window, Font* font, std::u16string text, Vector2 position, Vector2 scale, Color color)
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

void arqanore::Renderer::render_text(Window* window, Font* font, std::string text, Vector2 position, Vector2 scale, Color color)
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

void arqanore::Renderer::render_polygon(Window* window, Polygon* polygon, Texture* texture, Vector2 position, Vector2 scale, Vector2 origin, Vector2 offset, float angle, bool flip_hor, bool flip_vert, Color color)
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

void arqanore::Renderer::render_sprite(Window* window, Sprite* sprite, Vector2 position, Vector2 scale, Vector2 origin, float angle, int frame_hor, int frame_vert, bool flip_hor, bool flip_vert, Color color)
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