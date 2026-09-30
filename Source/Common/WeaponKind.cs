namespace SuperItem.Common;

public enum WeaponKind { Demonic, Holy, Spectrum, Prism }

public static class WeaponRules
{
    public static bool IsValid(WeaponKind kind) => (uint)kind < 4;
    public static int CooldownTicks(WeaponKind kind) => kind switch
    {
        WeaponKind.Demonic => 120,
        WeaponKind.Holy => 180,
        WeaponKind.Spectrum => 240,
        WeaponKind.Prism => 90,
        _ => 240
    };
    public static float ShotSpeed(WeaponKind kind) => kind switch
    {
        WeaponKind.Demonic => 25f,
        WeaponKind.Holy => 29f,
        WeaponKind.Spectrum => 23f,
        WeaponKind.Prism => 34f,
        _ => 20f
    };
    public static int SkillDamageMultiplier(WeaponKind kind) => kind switch
    {
        WeaponKind.Demonic => 20,
        WeaponKind.Holy => 24,
        WeaponKind.Spectrum => 18,
        WeaponKind.Prism => 40,
        _ => 1
    };
    public static int SkillDamage(WeaponKind kind, int normalDamage)
        => (int)Math.Clamp((long)normalDamage * SkillDamageMultiplier(kind), 1L, int.MaxValue);
}
