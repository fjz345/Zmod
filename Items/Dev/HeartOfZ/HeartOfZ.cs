using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using System;

namespace Zmod.Items.Dev.HeartOfZ
{
	public class HeartOfZ : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("DEV");
		}

		public override void SetDefaults()
		{
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
			//player.AddImmuneTime(ImmunityCooldownID.General, 60);
			player.SetImmuneTimeForAllTypes(1);
        }
    }
}