using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperItem.Items;

public sealed class ExcaliburZ : SpectacleWeapon
{
    protected override Common.WeaponKind Kind => Common.WeaponKind.Holy;
    public override void SetDefaults()
    {
        Item.damage = 1200;
        Item.DamageType = Terraria.ModLoader.DamageClass.Melee;
        Item.width = 50;
        Item.height = 50;
        Item.useTime = 7;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 20f;
        Item.value = 10000;
        Item.rare = ItemRarityID.Red;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        ConfigureProjectiles();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TrueExcalibur)
            .AddIngredient(ItemID.SoulofLight, 20)
            .AddIngredient(ItemID.FragmentSolar, 16)
            .AddIngredient(ItemID.LunarBar, 12)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
