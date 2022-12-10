using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Zmod.Items.Weapons.TpSword
{
	public class TpSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a sword");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Muramasa);

			Item.damage = 500;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = false;
			Item.knockBack = 2.0f;
			Item.value = 10000;
			Item.ammo = AmmoID.None;
			Item.useAmmo = 0;
			Item.mana = 0;
			Item.rare = ItemRarityID.Master;
			Item.UseSound = SoundID.NPCHit4;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 0);
			recipe.Register();
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
        }

        public override bool? UseItem(Player player)
        {
            return null;
        }

        public virtual bool AltFunctionUse(Player player)
        {
            return false;
        }
    }
}