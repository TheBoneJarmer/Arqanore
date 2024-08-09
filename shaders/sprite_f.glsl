#version 330 core
out vec4 frag_color;

in vec4 color;
in vec2 texcoord;

uniform sampler2D u_texture;
uniform int u_flip_hor;
uniform int u_flip_vert;

void main() {
    vec2 final_texcoord = texcoord;

    if (u_flip_hor == 1) {
        final_texcoord.x *= -1;
    }

    if (u_flip_vert == 1) {
        final_texcoord.y *= -1;
    }

    frag_color = color * texture(u_texture, final_texcoord);
}
