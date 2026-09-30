using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SuperItem.Common;

namespace SuperItem.Visuals;

// A decaying, direction-varying camera kick for the local player's heaviest impacts.
// Client-only presentation: nothing here is synced or affects gameplay.
public sealed class ScreenShake : ModSystem
{
    private static float strength;
    private static int timeLeft, duration;
    private static uint seed;

    internal static void Kick(float power, int ticks)
    {
        if (Main.dedServ) return;
        power *= SuperItemClientConfig.Shake;
        if (power <= .01f) return;
        // A stronger kick replaces a weaker one instead of stacking without limit.
        if (power * ticks >= strength * timeLeft)
        {
            strength = Math.Min(power, 22f);
            timeLeft = duration = Math.Max(1, ticks);
            seed = Main.GameUpdateCount;
        }
    }

    public override void PostUpdateEverything()
    {
        if (timeLeft > 0) timeLeft--;
    }

    public override void ModifyScreenPosition()
    {
        // GameUpdateCount keeps running while paused; a paused camera must stay still.
        if (timeLeft <= 0 || duration <= 0 || Main.gamePaused) return;
        float life = timeLeft / (float)duration;
        float amount = strength * life * life;
        float t = (Main.GameUpdateCount - seed) * 1.9f;
        // Two incommensurate frequencies avoid a visibly periodic wobble.
        Main.screenPosition += new Vector2(MathF.Sin(t * 1.7f + 1.3f) + .45f * MathF.Sin(t * 4.1f),
            MathF.Cos(t * 2.3f) + .45f * MathF.Cos(t * 3.3f + .7f)) * amount * .7f;
    }

    public override void OnWorldUnload()
    {
        strength = 0f;
        timeLeft = duration = 0;
    }
}
