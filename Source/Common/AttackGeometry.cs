using Microsoft.Xna.Framework;

namespace SuperItem.Common;

// Shared by rendering and collision so a wide crescent cannot hit like a tiny bolt.
internal static class AttackGeometry
{
    internal static float CrescentRadius(float age) => 48f + 38f * Math.Clamp(age / 18f, 0f, 1f);
    internal static Vector2 CrescentPoint(Vector2 head, float rotation, float age, float progress)
    {
        float radius = CrescentRadius(age), angle = MathHelper.Lerp(-1.22f, 1.22f, progress);
        Vector2 local = new((MathF.Cos(angle) - 1f) * radius, MathF.Sin(angle) * radius * .85f);
        float cos = MathF.Cos(rotation), sin = MathF.Sin(rotation);
        return head + new Vector2(local.X * cos - local.Y * sin, local.X * sin + local.Y * cos);
    }
}
