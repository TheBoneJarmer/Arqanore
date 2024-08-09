#include "arqanore/shaders.h"
#include "arqanore/shadersources.h"

void arqanore::Shaders::init() {
    font.add_vertex(SHADER_FONT_V, SHADER_SOURCE_TYPE_RAW);
    font.add_fragment(SHADER_FONT_F, SHADER_SOURCE_TYPE_RAW);
    font.compile();

    polygon.add_vertex(SHADER_POLYGON_V, SHADER_SOURCE_TYPE_RAW);
    polygon.add_fragment(SHADER_POLYGON_F, SHADER_SOURCE_TYPE_RAW);
    polygon.compile();

    sprite.add_vertex(SHADER_SPRITE_V, SHADER_SOURCE_TYPE_RAW);
    sprite.add_fragment(SHADER_SPRITE_F, SHADER_SOURCE_TYPE_RAW);
    sprite.compile();

    model.add_vertex(SHADER_MODEL_V, SHADER_SOURCE_TYPE_RAW);
    model.add_fragment(SHADER_MODEL_F, SHADER_SOURCE_TYPE_RAW);
    model.compile();
}

arqanore::Shader arqanore::Shaders::font;
arqanore::Shader arqanore::Shaders::polygon;
arqanore::Shader arqanore::Shaders::sprite;
arqanore::Shader arqanore::Shaders::model;
