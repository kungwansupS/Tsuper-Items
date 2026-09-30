using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SuperItem.Common;
using SuperItem.Projectiles;
using SuperItem.Visuals;

namespace SuperItem.Items;

public abstract class SpectacleWeapon : ModItem
{
    protected abstract WeaponKind Kind { get; }
    internal WeaponKind VisualKind => Kind;

    protected void ConfigureProjectiles()
    {
        Item.shoot = ModContent.ProjectileType<SpectralBolt>();
        Item.shootSpeed = WeaponRules.ShotSpeed(Kind);
        Item.noMelee = true;
        Item.noUseGraphic = Kind != WeaponKind.Prism;
    }

    public override void UseAnimation(Player player)
    {
        if (Kind == WeaponKind.Prism || player.whoAmI != Main.myPlayer) return;
        int type = ModContent.ProjectileType<HeldSword>();
        foreach (Projectile old in Main.projectile)
            if (old.active && old.owner == player.whoAmI && old.type == type) old.Kill();
        Vector2 aim = (Main.MouseWorld - player.MountedCenter).SafeNormalize(new Vector2(player.direction, 0));
        int facing = Math.Abs(aim.X) < .01f ? player.direction : Math.Sign(aim.X);
        int duration = Math.Max(2, CombinedHooks.TotalAnimationTime(Item.useAnimation, player, Item));
        int id = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.MountedCenter, aim, type,
            player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI, (int)Kind, 0, duration);
        if (id < Main.maxProjectiles && Main.projectile[id].ModProjectile is HeldSword held)
        {
            held.Configure(player.GetModPlayer<SuperItemPlayer>().NextSwing(Kind), facing, player.gravDir, player.GetAdjustedItemScale(Item));
            Main.projectile[id].netUpdate = true;
        }
    }

    public override void UseStyle(Player player, Rectangle heldItemFrame)
    {
        if (Kind != WeaponKind.Prism) return;
        float p = 1 - player.itemAnimation / (float)Math.Max(1, player.itemAnimationMax);
        float aim = player.itemRotation + (player.direction < 0 ? MathHelper.Pi : 0);
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
            aim - MathHelper.PiOver2 + player.direction * (-.45f + .9f * VfxDraw.Ramp(p, 0, .4f)));
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
        Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.whoAmI != Main.myPlayer) return false;
        if (Kind == WeaponKind.Demonic) return false; // The wave releases during the heavy cut.
        int sequence = player.GetModPlayer<SuperItemPlayer>().NextShot(Kind);
        if (Kind == WeaponKind.Spectrum)
            velocity = velocity.RotatedBy(Math.Sin(sequence * MathHelper.TwoPi / 14f) * .13f);
        Vector2 direction = velocity.SafeNormalize(new Vector2(player.direction, 0));
        Projectile.NewProjectile(source, position + direction * 24f, velocity, type,
            damage, knockback, player.whoAmI, (int)Kind, sequence);
        return false;
    }
}
