using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SuperItem.Common;

// Gameplay tuning shared by every player in a world; the server's values are synced to clients.
public sealed class SuperItemServerConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    internal static SuperItemServerConfig? Instance => ModContent.GetInstance<SuperItemServerConfig>();

    [Header("OmegaRift")]
    [DefaultValue(8)]
    [Range(0, 40)]
    [Slider]
    public int OmegaMaxLifeDamagePercent;

    [DefaultValue(12)]
    [Range(0, 40)]
    [Slider]
    public int OmegaExecuteThresholdPercent;

    [DefaultValue(true)]
    public bool OmegaIgnoresDamageReduction;
}

// Presentation preferences for this computer only.
public sealed class SuperItemClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    internal static SuperItemClientConfig? Instance => ModContent.GetInstance<SuperItemClientConfig>();

    [Header("Visuals")]
    [DefaultValue(100)]
    [Range(0, 100)]
    [Increment(10)]
    [Slider]
    public int ScreenShakePercent;

    [DefaultValue(100)]
    [Range(10, 100)]
    [Increment(10)]
    [Slider]
    public int FlashPercent;

    internal static float Shake => (Instance?.ScreenShakePercent ?? 100) / 100f;
    internal static float Flash => (Instance?.FlashPercent ?? 100) / 100f;
}
