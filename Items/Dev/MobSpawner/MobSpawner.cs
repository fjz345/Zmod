using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework.Input;

namespace Zmod.Items.Dev.MobSpawner
{
	public class MobSpawner : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("DEV");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Heart);

			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 0;
			Item.useAnimation = 0;

			Item.DamageType = DamageClass.Default;
			Item.noMelee = true;
			Item.knockBack = 0.0f;
			Item.value = 10000;
			Item.ammo = AmmoID.None;
			Item.useAmmo = 0;
			Item.mana = 0;
			Item.rare = ItemRarityID.Master;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Pwnhammer, 0);
			recipe.Register();
		}

		Int32[] SpawnMobTypes = { NPCID.RedSlime, NPCID.EyeofCthulhu, NPCID.Paladin};
		Int32 UseMode = 0;

        public override bool? UseItem(Player player)
        {
			NPC.NewNPC(player.GetSource_ItemUse(Item), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, SpawnMobTypes[UseMode]);
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            UseMode = (UseMode + 1) % SpawnMobTypes.GetLength(0);
			
			Main.NewText(NPC.GetFullnameByID(SpawnMobTypes[UseMode]));
            return false;
        }
    }
}