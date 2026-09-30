using Microsoft.Xna.Framework;

namespace SuperItem.Common;

internal readonly record struct SwordPose(float Angle, float Reach, bool Active);
internal readonly record struct BladeFrame(Vector2 Grip, float Angle, float Scale);

internal static class SwordMotion
{
    internal static Vector2 GripPixel(WeaponKind kind) => kind == WeaponKind.Spectrum ? new(12, 69) : new(12, 68);
    internal static Vector2 TipPixel(WeaponKind kind) => kind switch
    { WeaponKind.Demonic => new(76, 3), WeaponKind.Holy => new(77, 2), _ => new(72, 6) };
    internal static float Length(WeaponKind kind) => Vector2.Distance(GripPixel(kind), TipPixel(kind));
    internal static Vector2 Tip(WeaponKind kind, BladeFrame frame)
        => frame.Grip + new Vector2(MathF.Cos(frame.Angle), MathF.Sin(frame.Angle)) * Length(kind) * frame.Scale;
    private static float Smooth(float p) { p = Math.Clamp(p, 0, 1); return p * p * (3 - 2 * p); }
    internal static SwordPose Pose(WeaponKind kind, float progress, float aim, int direction, int combo)
    {
        float p = Math.Clamp(progress, 0, 1), turn = direction;
        if (kind == WeaponKind.Demonic)
        {
            turn *= combo % 2 == 0 ? 1 : -1;
            float offset = p < .2f ? MathHelper.Lerp(-1.75f, -2.05f, Smooth(p / .2f))
                : p < .78f ? MathHelper.Lerp(-2.05f, 1.55f, Smooth((p - .2f) / .58f))
                : MathHelper.Lerp(1.55f, 1.7f, Smooth((p - .78f) / .22f));
            return new(aim + offset * turn, 2 + 5 * MathF.Sin(MathF.PI * p), p is >= .2f and <= .86f);
        }
        if (kind == WeaponKind.Holy)
        {
            // Three short thrusts, then a rising cut. The whole phrase scales
            // with the original 30-tick animation and the player's attack speed.
            if (p < .7f)
            {
                float wave = MathF.Sin(p / .7f * MathF.PI * 3);
                float pulse = wave * wave;
                return new(aim + turn * .06f * MathF.Sin(p * 18), 2 + pulse * 23, p >= .03f);
            }
            return new(aim + turn * MathHelper.Lerp(.35f, -1.5f, Smooth((p - .7f) / .3f)), 7, p < .96f);
        }
        // One continuous full cut per 15-tick use; no phase reversal on repeat.
        return new(aim + turn * (-MathHelper.PiOver2 + MathHelper.TwoPi * p), 4, p is >= .03f and <= .97f);
    }
}
