#version 330 core
in vec2 a_position;
in vec2 a_texcoord;

uniform vec2 u_resolution;
uniform vec2 u_rotation;
uniform vec2 u_translation;
uniform vec2 u_scale;
uniform vec4 u_color;

out vec4 color;
out vec2 texcoord;

void main() {
    vec2 vertex = a_position;
    vertex = vertex * u_scale;
    vertex = vec2(vertex.x * u_rotation.y + vertex.y * u_rotation.x, vertex.y * u_rotation.y - vertex.x * u_rotation.x);
    vertex = vertex + u_translation;
    vertex = vertex / u_resolution;
    vertex = (vertex * 2.0) - 1.0;

    color = u_color;
    texcoord = a_texcoord;

    gl_Position = vec4(vertex.x, -vertex.y, 0, 1);
}
