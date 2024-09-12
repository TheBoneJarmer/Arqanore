#pragma once

#include "soloudw/soloud.h"

namespace arqanore {
    class Audio {
        friend class Window;
        friend class Sound;

    private:
        static SoLoud::Soloud* engine;

        static void init();

        static void destroy();

    public:
        static void stop();
    };
}
