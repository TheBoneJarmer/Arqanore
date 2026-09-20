#!/usr/bin/env bash
sudo cp -a ./include/** /usr/local/include/
sudo cp -a ./lib/freetype/include/** /usr/local/include/
sudo cp -a ./lib/glfw/include/** /usr/local/include/
sudo cp -a ./lib/soloudw/include/** /usr/local/include/
sudo cp -a ./build/*.so* /usr/local/lib/