#version 330 core
#define DIRECTIONAL_LIGHT 0
#define POINT_LIGHT 1

out vec4 frag_color;

struct Light {
    vec3 source;
    vec3 color;
    int type;
    bool enabled;
    float strength;
    float range;
};

struct Material {
    vec3 color;
    vec3 diffuse;
    vec3 ambient;
    vec3 specular;
    float shininess;

    sampler2D diffuse_map;
    sampler2D ambient_map;
    sampler2D specular_map;
    int diffuse_map_active;
    int ambient_map_active;
    int specular_map_active;
};

uniform vec3 u_view_pos;
uniform Material u_material;
uniform int u_light_count;
uniform Light u_light[99];

in vec3 frag_pos;
in vec3 vertex;
in vec3 normal;
in vec2 texcoord;

vec3 calc_dir_light(Light light) {
    vec3 light_normal = normalize(normal);
    vec3 light_dir = normalize(-light.source);
    float light_diffuse = max(dot(light_normal, light_dir), 0.0);

    vec3 view_dir = normalize(u_view_pos - frag_pos);
    vec3 reflect_dir = reflect(-light_dir, light_normal);
    float light_specular = pow(max(dot(view_dir, reflect_dir), 0.0), u_material.shininess);

    vec3 ambient = light.color * u_material.ambient;
    vec3 diffuse = light.color * light_diffuse * u_material.diffuse;
    vec3 specular = light.color * light_specular * u_material.specular;

    if (u_material.diffuse_map_active == 1) {
        vec3 diffuse_map_color = texture(u_material.diffuse_map, texcoord).xyz;

        diffuse *= diffuse_map_color;
        ambient *= diffuse_map_color;
        specular *= diffuse_map_color;
    }

    if (u_material.ambient_map_active == 1) {
        ambient *= texture(u_material.ambient_map, texcoord).xyz;
    }

    if (u_material.specular_map_active == 1) {
        specular *= texture(u_material.specular_map, texcoord).xyz;
    }

    return (ambient + diffuse) * u_material.color;
}

vec3 calc_point_light(Light light) {
    vec3 light_normal = normalize(normal);
    vec3 light_dir = normalize(light.source - frag_pos);
    float light_diffuse = max(dot(light_normal, light_dir), 0.0);

    vec3 view_dir = normalize(u_view_pos - frag_pos);
    vec3 halfway_dir = normalize(light_dir + view_dir);
    float light_specular = pow(max(dot(view_dir, halfway_dir), 0.0), u_material.shininess);

    float light_dist = length(light.source - frag_pos);
    float light_att = 1.0 / light_dist * light.strength;
    //float light_att = 1.0 / (1.0 + 0.09 * light_dist + 0.032 * (light_dist * light_dist));

    if (light_att > 1.0) {
        light_att = 1.0;
    }

    if (light_dist > light.range) {
        return vec3(0, 0, 0);
    }

    vec3 ambient = light.color * u_material.ambient * light_att;
    vec3 diffuse = light.color * light_diffuse * u_material.diffuse * light_att;
    vec3 specular = light.color * light_specular * u_material.specular * light_att;

    if (u_material.diffuse_map_active == 1) {
        vec3 diffuse_map_color = texture(u_material.diffuse_map, texcoord).xyz;

        diffuse *= diffuse_map_color;
        ambient *= diffuse_map_color;
        specular *= diffuse_map_color;
    }

    if (u_material.ambient_map_active == 1) {
        ambient *= texture(u_material.ambient_map, texcoord).xyz;
    }

    if (u_material.specular_map_active == 1) {
        specular *= texture(u_material.specular_map, texcoord).xyz;
    }

    return (ambient + diffuse + specular) * u_material.color;
}

void main() {
    vec4 result = vec4(0, 0, 0, 1);

    for (int i=0; i<u_light_count; i++) {
        if (!u_light[i].enabled) {
            continue;
        }

        if (u_light[i].type == DIRECTIONAL_LIGHT) {
            result.xyz += calc_dir_light(u_light[i]);
        }

        if (u_light[i].type == POINT_LIGHT) {
            result.xyz += calc_point_light(u_light[i]);
        }
    }

    frag_color = clamp(result, 0, 1);
}
