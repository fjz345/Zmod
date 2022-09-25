using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Zmod.Items
{
	public class QWE : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("QWE"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("This is a basic modded sword.");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ShadowbeamStaff);

			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.MowTheLawn;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.shootSpeed = 2.0f;
			Item.shoot = ModContent.ProjectileType<QWEProjectile>();
			Item.ammo = AmmoID.None;
			Item.rare = ItemRarityID.Master;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.Register();
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
		}
	}

	class QWEProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 200; // lowered from 300
			Projectile.penetrate = -1;
		}

		// Note, this Texture is actually just a blank texture, FYI.
		//public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false; // return false because we are handling collision
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Projectile.damage = (int)(Projectile.damage * 0.8);
		}

		public override void AI()
		{
		}
	}
}