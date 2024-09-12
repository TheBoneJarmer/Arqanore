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

out vec3 frag_pos;
out vec3 normal;
out vec2 texcoord;
out vec4 bone;

void main() {
    vec4 final_pos = vec4(0);
    mat4 mat_model = u_model_matrix * u_mesh_matrix;
    mat4 mat_mvp = u_projection_matrix * u_view_matrix * mat_model;
    mat3 mat_normal = mat3(transpose(inverse(mat_model)));

    for (int i=0; i<4; i++) {
        if (a_bone[i] == -1) {
            continue;
        }

        if (a_bone[i] >= 10) {
            final_pos = vec4(a_vertex, 1.0);
            break;
        }

        vec4 local_pos = u_bone[int(a_bone[i])] * vec4(a_vertex, 1.0);
        final_pos += local_pos * a_weight[i];
    }

    // If there are no bones the final_pos will be untouched so we can just set it
    if (a_bone[0] == -1) {
        final_pos = vec4(a_vertex, 1.0);
    }

    frag_pos = vec3(mat_model * final_pos);
    normal = mat_normal * a_normal;
    texcoord = a_texcoord;
    bone = a_bone;

    gl_Position =  mat_mvp * final_pos;
}
