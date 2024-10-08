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
        include/arqanore/vector2.h
        include/arqanore/sprite.h
        include/arqanore/shadersources.h
        include/arqanore/image.h
        include/arqanore/texture.h
)

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
        src/arqanore/vector2.cpp
        src/arqanore/sprite.cpp
        src/arqanore/image.cpp
        src/arqanore/texture.cpp
)

# Configure dependencies
set(LIBRARY_OUTPUT_PATH "${CMAKE_BINARY_DIR}")
set(EXECUTABLE_OUTPUT_PATH "${CMAKE_BINARY_DIR}")

add_subdirectory(lib/glfw)
add_subdirectory(lib/soloudw)
add_subdirectory(lib/freetype)

if (WIN32)
    link_libraries(glfw freetype soloudw)
else ()
    link_libraries(glfw freetype soloudw dl -Wl,-rpath='$ORIGIN:$ORIGIN/lib')
endif ()

include_directories("include/")
include_directories("lib/glfw/include/")
include_directories("lib/soloudw/include/")
include_directories("lib/freetype/include/")

# Configure arqanore
add_library(arqanore ${ARQ_INCLUDE} ${ARQ_SRC})
add_dependencies(arqanore glfw soloudw freetype)
set_target_properties(arqanore PROPERTIES POSITION_INDEPENDENT_CODE ON)
set_target_properties(arqanore PROPERTIES OUTPUT_NAME "arqanore")
set_target_properties(arqanore PROPERTIES VERSION ${ARQ_VERSION})

# Configure arqanore examples
if (WIN32)
    add_executable(example_window WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/window/main.cpp)
	add_dependencies(example_window arqanore)
    add_executable(example_sprites WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/sprites/main.cpp)
    add_executable(example_fonts WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/fonts/main.cpp)
    add_executable(example_audio WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/audio/main.cpp)
    add_executable(example_shaders WIN32 ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/shaders/main.cpp)
else ()
    add_executable(example_window ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/window/main.cpp)
    add_executable(example_sprites ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/sprites/main.cpp)
    add_executable(example_fonts ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/fonts/main.cpp)
    add_executable(example_audio ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/audio/main.cpp)
    add_executable(example_shaders ${ARQ_INCLUDE} ${ARQ_SRC} src/examples/shaders/main.cpp)
endif ()
