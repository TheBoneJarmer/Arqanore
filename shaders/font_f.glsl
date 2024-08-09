#version 330 core
out vec4 frag_color;

in vec4 color;
in vec2 texcoord;

uniform sampler2D u_image;

void main() {
    vec4 tex_color = texture(u_image, texcoord);
    vec4 result_color = vec4(1.0, 1.0, 1.0, tex_color.r) * color;

    frag_color = result_color;
}
