#version 330 core
layout (location = 0) in vec3 a_vertex;
layout (location = 1) in vec3 a_normal;
layout (location = 2) in vec2 a_texcoord;
uniform mat4 u_model_matrix;
uniform mat4 u_mesh_matrix;
uniform mat4 u_view_matrix;
uniform mat4 u_projection_matrix;

out vec3 frag_pos;
out vec3 vertex;
out vec3 normal;
out vec2 texcoord;

void main() {
    mat4 mat_model = u_model_matrix * u_mesh_matrix;
    mat4 mat_mvp = u_projection_matrix * u_view_matrix * mat_model;
    mat3 mat_normal = mat3(transpose(inverse(mat_model)));

    frag_pos = vec3(mat_model * vec4(a_vertex, 1.0));
    vertex = (u_view_matrix * mat_model * vec4(a_vertex, 1.0)).xyz;
    normal = mat_normal * a_normal;
    texcoord = a_texcoord;
    
    gl_Position =  mat_mvp * vec4(a_vertex, 1.0);
}
