#pragma once

namespace arqanore {
    class MathHelper {
    public:
        static float radians(float deg);

        static float degrees(float rad);

        static float lerp(float start, float end, float t);

        static float clamp(float value, float min, float max);
    };
}
