#version 330 core
layout (location = 0) in vec3 a_vertex;
layout (location = 1) in vec3 a_normal;
layout (location = 2) in vec2 a_texcoord;
layout (location = 3) in vec4 a_bone;
layout (location = 4) in vec4 a_weight;

uniform mat4 u_model_matrix;
uniform mat4 u_mesh_matrix;
uniform mat4 u_view_matrix;
uniform mat4 u_projection_matrix;
uniform mat4 u_bone[10];
uniform int u_bone_count;

out vec3 frag_pos;
out vec3 normal;
out vec2 texcoord;
out vec4 bone;

mat4 get_bone_matrix() {
    mat4 result = mat4(
    1, 0, 0, 0,
    0, 1, 0, 0,
    0, 0, 1, 0,
    0, 0, 0, 1
    );

    if (a_bone.x >= 0) {
        result += u_bone[int(a_bone.x)] * a_weight.x;
    }

    if (a_bone.y >= 0) {
        result += u_bone[int(a_bone.y)] * a_weight.y;
    }

    if (a_bone.z >= 0) {
        result += u_bone[int(a_bone.z)] * a_weight.z;
    }

    if (a_bone.w >= 0) {
        result *= u_bone[int(a_bone.w)] * a_weight.w;
    }

    return result;
}

void main() {
    vec4 vertex = get_bone_matrix() * vec4(a_vertex, 1.0);
    mat4 mat_model = u_model_matrix * u_mesh_matrix;
    mat4 mat_mvp = u_projection_matrix * u_view_matrix * mat_model;
    mat3 mat_normal = mat3(transpose(inverse(mat_model)));

    frag_pos = vec3(mat_model * vertex);
    normal = mat_normal * a_normal;
    texcoord = a_texcoord;
    bone = a_bone;

    gl_Position =  mat_mvp * vertex;
}
