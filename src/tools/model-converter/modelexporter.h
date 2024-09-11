#pragma once
#include "modelimporter.h"

namespace arqanore
{
    class ModelExporter
    {
    public:
        ModelExporter();

        void save(ModelImporterResult result, std::string path);
    };
}
