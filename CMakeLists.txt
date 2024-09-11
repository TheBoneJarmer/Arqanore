cmake_minimum_required(VERSION 3.16)

file(STRINGS "VERSION" ARQ_VERSION)
project(arqanore VERSION ${ARQ_VERSION} DESCRIPTION "Arqanore is a 2D and 3D OpenGL game library")
add_compile_definitions(VERSION="${ARQ_VERSION}")

set(CMAKE_CXX_STANDARD 23)
set(ARQ_INCLUDE
        include/arqanore/arqanore.h
        include/arqanore/audio.h
        include/arqanore/color.h
        include/arqanore/exceptions.h
        include/arqanore/font.h
        include/arqanore/joystick.h
        include/arqanore/keyboard.h
        include/arqanore/mathhelper.h
        include/arqanore/mouse.h
        include/arqanore/polygon.h
        include/arqanore/renderer.h
        include/arqanore/shader.h
        include/arqanore/shaders.h
        include/arqanore/sound.h
        include/arqanore/utils.h
        include/arqanore/window.h
        include/arqanore/texture.h
        include/arqanore/vector2.h
        include/arqanore/sprite.h
        include/arqanore/vector3.h
        include/arqanore/model.h
        include/arqanore/modelparser.h
        include/arqanore/mesh.h
        include/arqanore/matrix4.h
        include/arqanore/matrix3.h
        include/arqanore/material.h
        include/arqanore/light.h
        include/arqanore/camera.h
        include/arqanore/scene.h
        include/arqanore/quaternion.h
        include/arqanore/shadersources.h
        include/arqanore/image.h
        include/arqanore/bone.h
        include/arqanore/vector4.h)
set(ARQ_SRC
        src/arqanore/glad/gl.c
        src/arqanore/arqanore.cpp
        src/arqanore/window.cpp
        src/arqanore/exceptions.cpp
        src/arqanore/utils.cpp
        src/arqanore/keyboard.cpp
        src/arqanore/mouse.cpp
        src/arqanore/joystick.cpp
        src/arqanore/audio.cpp
        src/arqanore/sound.cpp
        src/arqanore/mathhelper.cpp
        src/arqanore/shader.cpp
        src/arqanore/color.cpp
        src/arqanore/font.cpp
        src/arqanore/polygon.cpp
        src/arqanore/shaders.cpp
        src/arqanore/renderer.cpp
        src/arqanore/texture.cpp
        src/arqanore/vector2.cpp
        src/arqanore/sprite.cpp
        src/arqanore/camera.cpp
        src/arqanore/light.cpp
        src/arqanore/material.cpp
        src/arqanore/matrix3.cpp
        src/arqanore/matrix4.cpp
        src/arqanore/mesh.cpp
        src/arqanore/model.cpp
        src/arqanore/modelparser.cpp
        src/arqanore/vector3.cpp
        src/arqanore/scene.cpp
        src/arqanore/quaternion.cpp
        src/arqanore/image.cpp
        src/arqanore/bone.cpp
        src/arqanore/vector4.cpp)

# Generate variables for the compiler and linker
if (WIN32)
    if (NOT DEFINED WIN_INCLUDE_DIR)
        set(WIN_INCLUDE_DIR "C:/Include")
    endif ()

    if (NOT DEFINED WIN_LINK_DIR)
        set(WIN_LINK_DIR "C:/Lib")
    endif ()

    include_directories(include/)
    include_directories(${WIN_INCLUDE_DIR})
    link_directories(${WIN_LINK_DIR})
    link_libraries(glfw3 freetype soloudw)
else ()
    include_directories(include/)
    link_libraries(glfw freetype soloudw dl -Wl,-rpath='$ORIGIN:$ORIGIN/lib')
endif ()

# Configure arqanore
add_library(arqanore ${ARQ_INCLUDE} ${ARQ_SRC})
set_target_properties(arqanore PROPERTIES POSITION_INDEPENDENT_CODE ON)
set_target_properties(arqanore PROPERTIES OUTPUT_NAME "arqanore")
set_target_properties(arqanore PROPERTIES VERSION ${ARQ_VERSION})

# Configure arqanore tools
if (WIN32)
    add_executable(model_converter WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/tools/model-converter/main.cpp
            src/tools/model-converter/model-exporter.h
            src/tools/model-converter/model-exporter.cpp)
    set_target_properties(model_converter PROPERTIES OUTPUT_NAME "arq-mc")
    set_target_properties(model_converter PROPERTIES VERSION ${ARQ_VERSION})
    target_link_libraries(model_converter PUBLIC assimp)
else ()
    add_executable(model_converter ${ARQ_INCLUDE} ${ARQ_SRC}
            src/tools/model-converter/main.cpp
            src/tools/model-converter/model-importer.cpp
            src/tools/model-converter/model-importer.h
            src/tools/model-converter/model-exporter.h
            src/tools/model-converter/model-exporter.cpp)
    set_target_properties(model_converter PROPERTIES OUTPUT_NAME "arq-mc")
    set_target_properties(model_converter PROPERTIES VERSION ${ARQ_VERSION})
    target_link_libraries(model_converter PUBLIC assimp)
endif ()

# Configure arqanore examples
if (WIN32)
    add_executable(example_window WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/window/main.cpp)
    add_executable(example_sprites WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/sprites/main.cpp)
    add_executable(example_fonts WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/fonts/main.cpp)
    add_executable(example_models WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/models/main.cpp)
    add_executable(example_audio WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/audio/main.cpp)
    add_executable(example_shaders WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/shaders/main.cpp)
else ()
    add_executable(example_window ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/window/main.cpp)
    add_executable(example_sprites ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/sprites/main.cpp)
    add_executable(example_fonts ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/fonts/main.cpp)
    add_executable(example_models ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/models/main.cpp)
    add_executable(example_audio ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/audio/main.cpp)
    add_executable(example_shaders ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/shaders/main.cpp)
endif ()