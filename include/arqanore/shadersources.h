#pragma once

#define SHADER_POLYGON_F "#version 330 core\nout vec4 frag_color;\n\nin vec4 color;\nin vec2 texcoord;\n\nuniform sampler2D u_texture;\nuniform int u_texture_active;\nuniform int u_flip_hor;\nuniform int u_flip_vert;\n\nvoid main() {\nvec4 final_color = color;\nvec2 final_texcoord = texcoord;\n\nif (u_flip_hor == 1) {\nfinal_texcoord.x *= -1;\n}\n\nif (u_flip_vert == 1) {\nfinal_texcoord.y *= -1;\n}\n\nif (u_texture_active == 1) {\nfinal_color *= texture(u_texture, final_texcoord);\n}\n\nfrag_color = final_color;\n}\n"
#define SHADER_SPRITE_V "#version 330 core\nprecision mediump float;\n\nlayout (location = 0) in vec2 a_vertex;\nlayout (location = 1) in vec2 a_texcoord;\n\nuniform vec2 u_resolution;\nuniform vec2 u_rotation;\nuniform vec2 u_translation;\nuniform vec2 u_scale;\nuniform vec4 u_color;\nuniform vec2 u_origin;\nuniform vec2 u_offset;\n\nout vec4 color;\nout vec2 texcoord;\n\nvoid main() {\nvec2 vertex = a_vertex - u_origin;\nvertex = vertex * u_scale;\nvertex = vec2(vertex.x * u_rotation.y + vertex.y * u_rotation.x, vertex.y * u_rotation.y - vertex.x * u_rotation.x);\nvertex = vertex + u_translation;\nvertex = vertex / u_resolution;\nvertex = (vertex * 2.0) - 1.0;\n\ncolor = u_color;\ntexcoord = a_texcoord + u_offset;\n\ngl_Position = vec4(vertex.x, -vertex.y, 0, 1);\n}\n"
#define SHADER_SPRITE_F "#version 330 core\nout vec4 frag_color;\n\nin vec4 color;\nin vec2 texcoord;\n\nuniform sampler2D u_texture;\nuniform int u_flip_hor;\nuniform int u_flip_vert;\n\nvoid main() {\nvec2 final_texcoord = texcoord;\n\nif (u_flip_hor == 1) {\nfinal_texcoord.x *= -1;\n}\n\nif (u_flip_vert == 1) {\nfinal_texcoord.y *= -1;\n}\n\nfrag_color = color * texture(u_texture, final_texcoord);\n}\n"
#define SHADER_MODEL_F "#version 330 core\n#define DIRECTIONAL_LIGHT 0\n#define POINT_LIGHT 1\n\nout vec4 frag_color;\n\nstruct Light {\nvec3 source;\nvec3 color;\nint type;\nbool enabled;\nfloat strength;\nfloat range;\n};\n\nstruct Material {\nvec3 color;\nvec3 diffuse;\nvec3 ambient;\nvec3 specular;\nfloat shininess;\n\nsampler2D diffuse_map;\nsampler2D ambient_map;\nsampler2D specular_map;\nint diffuse_map_active;\nint ambient_map_active;\nint specular_map_active;\n};\n\nuniform vec3 u_view_pos;\nuniform Material u_material;\nuniform int u_light_count;\nuniform Light u_light[99];\n\nin vec3 frag_pos;\nin vec3 normal;\nin vec2 texcoord;\nin vec4 bone;\n\nvec3 calc_dir_light(Light light) {\nvec3 light_normal = normalize(normal);\nvec3 light_dir = normalize(-light.source);\nfloat light_diffuse = max(dot(light_normal, light_dir), 0.0);\n\nvec3 view_dir = normalize(u_view_pos - frag_pos);\nvec3 reflect_dir = reflect(-light_dir, light_normal);\nfloat light_specular = pow(max(dot(view_dir, reflect_dir), 0.0), u_material.shininess);\n\nvec3 ambient = light.color * u_material.ambient;\nvec3 diffuse = light.color * light_diffuse * u_material.diffuse;\nvec3 specular = light.color * light_specular * u_material.specular;\n\nif (u_material.diffuse_map_active == 1) {\nvec3 diffuse_map_color = texture(u_material.diffuse_map, texcoord).xyz;\n\ndiffuse *= diffuse_map_color;\nambient *= diffuse_map_color;\nspecular *= diffuse_map_color;\n}\n\nif (u_material.ambient_map_active == 1) {\nambient *= texture(u_material.ambient_map, texcoord).xyz;\n}\n\nif (u_material.specular_map_active == 1) {\nspecular *= texture(u_material.specular_map, texcoord).xyz;\n}\n\nreturn (ambient + diffuse) * u_material.color;\n}\n\nvec3 calc_point_light(Light light) {\nvec3 light_normal = normalize(normal);\nvec3 light_dir = normalize(light.source - frag_pos);\nfloat light_diffuse = max(dot(light_normal, light_dir), 0.0);\n\nvec3 view_dir = normalize(u_view_pos - frag_pos);\nvec3 halfway_dir = normalize(light_dir + view_dir);\nfloat light_specular = pow(max(dot(view_dir, halfway_dir), 0.0), u_material.shininess);\n\nfloat light_dist = length(light.source - frag_pos);\nfloat light_att = 1.0 / light_dist * light.strength;\n//float light_att = 1.0 / (1.0 + 0.09 * light_dist + 0.032 * (light_dist * light_dist));\n\nif (light_att > 1.0) {\nlight_att = 1.0;\n}\n\nif (light_dist > light.range) {\nreturn vec3(0, 0, 0);\n}\n\nvec3 ambient = light.color * u_material.ambient * light_att;\nvec3 diffuse = light.color * light_diffuse * u_material.diffuse * light_att;\nvec3 specular = light.color * light_specular * u_material.specular * light_att;\n\nif (u_material.diffuse_map_active == 1) {\nvec3 diffuse_map_color = texture(u_material.diffuse_map, texcoord).xyz;\n\ndiffuse *= diffuse_map_color;\nambient *= diffuse_map_color;\nspecular *= diffuse_map_color;\n}\n\nif (u_material.ambient_map_active == 1) {\nambient *= texture(u_material.ambient_map, texcoord).xyz;\n}\n\nif (u_material.specular_map_active == 1) {\nspecular *= texture(u_material.specular_map, texcoord).xyz;\n}\n\nreturn (ambient + diffuse + specular) * u_material.color;\n}\n\nvoid main() {\nvec4 result = vec4(0, 0, 0, 1);\n\nfor (int i=0; i<u_light_count; i++) {\nif (!u_light[i].enabled) {\ncontinue;\n}\n\nif (u_light[i].type == DIRECTIONAL_LIGHT) {\nresult.xyz += calc_dir_light(u_light[i]);\n}\n\nif (u_light[i].type == POINT_LIGHT) {\nresult.xyz += calc_point_light(u_light[i]);\n}\n}\n\nfrag_color = clamp(result, 0, 1);\n}\n"
#define SHADER_FONT_F "#version 330 core\nout vec4 frag_color;\n\nin vec4 color;\nin vec2 texcoord;\n\nuniform sampler2D u_image;\n\nvoid main() {\nvec4 tex_color = texture(u_image, texcoord);\nvec4 result_color = vec4(1.0, 1.0, 1.0, tex_color.r) * color;\n\nfrag_color = result_color;\n}\n"
#define SHADER_FONT_V "#version 330 core\nin vec2 a_position;\nin vec2 a_texcoord;\n\nuniform vec2 u_resolution;\nuniform vec2 u_rotation;\nuniform vec2 u_translation;\nuniform vec2 u_scale;\nuniform vec4 u_color;\n\nout vec4 color;\nout vec2 texcoord;\n\nvoid main() {\nvec2 vertex = a_position;\nvertex = vertex * u_scale;\nvertex = vec2(vertex.x * u_rotation.y + vertex.y * u_rotation.x, vertex.y * u_rotation.y - vertex.x * u_rotation.x);\nvertex = vertex + u_translation;\nvertex = vertex / u_resolution;\nvertex = (vertex * 2.0) - 1.0;\n\ncolor = u_color;\ntexcoord = a_texcoord;\n\ngl_Position = vec4(vertex.x, -vertex.y, 0, 1);\n}\n"
#define SHADER_POLYGON_V "#version 330 core\nprecision mediump float;\n\nlayout (location = 0) in vec2 a_vertex;\nlayout (location = 1) in vec2 a_texcoord;\n\nuniform vec2 u_resolution;\nuniform vec2 u_rotation;\nuniform vec2 u_translation;\nuniform vec2 u_scale;\nuniform vec4 u_color;\nuniform vec2 u_origin;\nuniform vec2 u_offset;\n\nout vec4 color;\nout vec2 texcoord;\n\nvoid main() {\nvec2 vertex = a_vertex - u_origin;\nvertex = vertex * u_scale;\nvertex = vec2(vertex.x * u_rotation.y + vertex.y * u_rotation.x, vertex.y * u_rotation.y - vertex.x * u_rotation.x);\nvertex = vertex + u_translation;\nvertex = vertex / u_resolution;\nvertex = (vertex * 2.0) - 1.0;\n\ncolor = u_color;\ntexcoord = a_texcoord + u_offset;\n\ngl_Position = vec4(vertex.x, -vertex.y, 0, 1);\n}\n"
#define SHADER_MODEL_V "#version 330 core\nlayout (location = 0) in vec3 a_vertex;\nlayout (location = 1) in vec3 a_normal;\nlayout (location = 2) in vec2 a_texcoord;\nlayout (location = 3) in vec4 a_bone;\nlayout (location = 4) in vec4 a_weight;\n\nuniform mat4 u_model_matrix;\nuniform mat4 u_mesh_matrix;\nuniform mat4 u_view_matrix;\nuniform mat4 u_projection_matrix;\nuniform mat4 u_bone[10];\nuniform int u_bone_count;\n\nout vec3 frag_pos;\nout vec3 normal;\nout vec2 texcoord;\nout vec4 bone;\n\nmat4 get_bone_matrix() {\nmat4 result = mat4(\n1, 0, 0, 0,\n0, 1, 0, 0,\n0, 0, 1, 0,\n0, 0, 0, 1\n);\n\nif (a_bone.x >= 0) {\nresult += u_bone[int(a_bone.x)] * a_weight.x;\n}\n\nif (a_bone.y >= 0) {\nresult += u_bone[int(a_bone.y)] * a_weight.y;\n}\n\nif (a_bone.z >= 0) {\nresult += u_bone[int(a_bone.z)] * a_weight.z;\n}\n\nif (a_bone.w >= 0) {\nresult *= u_bone[int(a_bone.w)] * a_weight.w;\n}\n\nreturn result;\n}\n\nvoid main() {\nvec4 vertex = get_bone_matrix() * vec4(a_vertex, 1.0);\nmat4 mat_model = u_model_matrix * u_mesh_matrix;\nmat4 mat_mvp = u_projection_matrix * u_view_matrix * mat_model;\nmat3 mat_normal = mat3(transpose(inverse(mat_model)));\n\nfrag_pos = vec3(mat_model * vertex);\nnormal = mat_normal * a_normal;\ntexcoord = a_texcoord;\nbone = a_bone;\n\ngl_Position =  mat_mvp * vertex;\n}\n"
