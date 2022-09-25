using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Zmod.Items.Weapons
{
	public class IceBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a magic book");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CrystalStorm);

			Item.damage = 500;
			Item.reuseDelay = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 400;
			Item.height = 400;
			Item.knockBack = 1.0f;
			Item.value = 10000;
			Item.shootSpeed = 1.0f;
			Item.mana = 0;
			Item.shoot = ModContent.ProjectileType<IceBookProjectile>();
			Item.rare = ItemRarityID.Master;
			Item.UseSound = SoundID.GuitarF;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 0);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			// Default shooting
			return true;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 MousePos = Main.MouseWorld;
			position = MousePos;
		}

	}

	public class IceBookProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 100;
			Projectile.friendly = true;
			Projectile.width = 400;
			Projectile.height = 400;
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

			// Create visual effect
			Vector2 pos = Projectile.Hitbox.Center.ToVector2();
			Projectile VisProj = Projectile.NewProjectileDirect(source, pos, new Vector2(0.0f, 0.0f), ModContent.ProjectileType<IceBookProjectileVis1>(), 0, 0);
		}
	}

	public class IceBookProjectileVis1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 100;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(0);

			DrawOffsetX = -150;
			DrawOriginOffsetY = -309/2;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1.0f;

			Projectile.rotation += 0.1f;
			Projectile.alpha += (int)(1 * Projectile.ai[0] / 10.0f);
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
		}
	}
}