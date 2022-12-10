using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using System;

namespace Zmod.Items.Dev.KillAll
{
	public class KillAll : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("DEV");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CrystalStorm);

			Item.damage = 999999;
			Item.reuseDelay = 10;
			Item.DamageType = DamageClass.Magic;
			Item.width = 400;
			Item.height = 400;
			Item.knockBack = 1.0f;
			Item.value = 10000;
			Item.shootSpeed = 1.0f;
			Item.mana = 0;
			Item.shoot = ModContent.ProjectileType<KillAllProjectile>();
			Item.rare = ItemRarityID.Master;
			Item.UseSound = SoundID.GuitarEm;
			Item.autoReuse = true;
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Pwnhammer, 0);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 MousePos = Main.MouseWorld;
            position = MousePos;
        }

        public override bool? UseItem(Player player)
        {
            
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

    }

	public class KillAllProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.width = 4000;
			Projectile.height = 4000;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override string Texture => "Zmod/Resources/None";

		public override void AI()
		{
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
		}
	}
}