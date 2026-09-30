namespace SuperItem.Visuals;

// Shared easing curves. Every timeline in the skill renderers is built from these so that
// phases overlap and cross-fade instead of switching abruptly between frames.
internal static class Easing
{
    internal static float Sat(float t) => Math.Clamp(t, 0f, 1f);
    internal static float Range(float t, float from, float to) => Sat((t - from) / (to - from));
    internal static float InQuad(float t) { t = Sat(t); return t * t; }
    internal static float InCubic(float t) { t = Sat(t); return t * t * t; }
    internal static float OutQuad(float t) { t = Sat(t); return 1f - (1f - t) * (1f - t); }
    internal static float OutCubic(float t) { t = Sat(t); float u = 1f - t; return 1f - u * u * u; }
    internal static float OutQuint(float t) { t = Sat(t); float u = 1f - t; return 1f - u * u * u * u * u; }
    internal static float OutExpo(float t) { t = Sat(t); return t >= 1f ? 1f : 1f - MathF.Pow(2f, -10f * t); }
    internal static float InOutSine(float t) => .5f - .5f * MathF.Cos(MathF.PI * Sat(t));
    internal static float InOutCubic(float t)
    {
        t = Sat(t);
        return t < .5f ? 4f * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 3f) * .5f;
    }
    // Overshoots past 1 before settling: snappy "arrival" for rings, brackets and crystals.
    internal static float OutBack(float t, float overshoot = 1.70158f)
    {
        t = Sat(t) - 1f;
        return 1f + t * t * ((overshoot + 1f) * t + overshoot);
    }
    internal static float Smooth(float t) { t = Sat(t); return t * t * (3f - 2f * t); }
    internal static float Smoother(float t) { t = Sat(t); return t * t * t * (t * (t * 6f - 15f) + 10f); }
    internal static float Bell(float t) => MathF.Sin(MathF.PI * Sat(t));
    // Smooth attack over [attackStart, attackEnd] multiplied by a smooth release over [releaseStart, releaseEnd].
    internal static float Envelope(float t, float attackStart, float attackEnd, float releaseStart, float releaseEnd)
        => Smooth(Range(t, attackStart, attackEnd)) * (1f - Smooth(Range(t, releaseStart, releaseEnd)));
    // A fast decay used for flashes: full at start, quadratic falloff to zero at start + life.
    internal static float Flash(float t, float start, float life)
    {
        if (t < start) return 0f;
        float p = 1f - Sat((t - start) / life);
        return p * p;
    }
    // Deterministic per-index noise in [0, 1): identical on every client for synced visuals.
    internal static float Hash(int a, int b = 0)
    {
        uint h = (uint)(a * 374761393 + b * 668265263);
        h = (h ^ (h >> 13)) * 1274126177u;
        return ((h ^ (h >> 16)) & 0xFFFFFF) / 16777216f;
    }
}
