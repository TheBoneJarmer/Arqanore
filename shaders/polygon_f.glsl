#version 330 core
out vec4 frag_color;

in vec4 color;
in vec2 texcoord;

uniform sampler2D u_texture;
uniform int u_texture_active;
uniform int u_flip_hor;
uniform int u_flip_vert;

void main() {
    vec4 final_color = color;
    vec2 final_texcoord = texcoord;

    if (u_flip_hor == 1) {
        final_texcoord.x *= -1;
    }

    if (u_flip_vert == 1) {
        final_texcoord.y *= -1;
    }

    if (u_texture_active == 1) {
        final_color *= texture(u_texture, final_texcoord);
    }

    frag_color = final_color;
}
