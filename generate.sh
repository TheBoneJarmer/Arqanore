echo "Generating shadersources.h"
src_glsl=$(find ./shaders -type f -name "*.glsl")
dest_glsl="include/arqanore/shadersources.h"

echo "#pragma once" > $dest_glsl
echo "" >> $dest_glsl

# Write down all shader macros first
for file in $src_glsl
do
    file_name=$(basename "$file")
    file_base=${file_name/.glsl/}
    shader_name="SHADER_${file_base^^}"
    shader_content=""

    while read -r line
    do
        shader_content="$shader_content$line\n"
    done < "$file"

    echo "#define $shader_name \"$shader_content\"" >> $dest_glsl
done