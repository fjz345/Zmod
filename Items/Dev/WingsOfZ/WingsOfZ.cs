using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using System;

namespace Zmod.Items.Dev.WingsOfZ
{
    [AutoloadEquip(EquipType.Wings)]
    public class WingsOfZ : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("DEV");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.JimsWings);
            Item.value = 100000;
            Item.rare = ItemRarityID.Master;
            Item.accessory = true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Pwnhammer, 0);
			recipe.Register();
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 9999999;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.0f;
            ascentWhenRising = 1.0f;
            maxCanAscendMultiplier = 1.0f;
            maxAscentMultiplier = 3.0f;
            constantAscend = 1.0f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 30f;
            acceleration *= 20f;
        }
    }
}