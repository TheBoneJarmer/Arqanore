#!/bin/bash

mkdir build 2> /dev/null
cd build

cmake -DBUILD_SHARED_LIBS=ON -S .. -G "Unix Makefiles"
cmake --build .
