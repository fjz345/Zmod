using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Zmod.Items.Weapons
{
	public class DivineBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a magic bow");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CopperBow);

			Item.damage = 500;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 60*3;
			Item.useTime = 60 * 3;

			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.knockBack = 1.0f;
			Item.value = 10000;
			Item.shootSpeed = 10.0f;
			Item.shoot = ModContent.ProjectileType<DivineBowMainProjectile>();
			Item.ammo = AmmoID.None;
			Item.useAmmo = 0;
			Item.mana = 0;
			Item.rare = ItemRarityID.Master;
			Item.UseSound = SoundID.GuitarC;
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
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}

	// Lives for full duration of weapon use. Only provides AI for the weapon
	public class DivineBowMainProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.timeLeft = 60*3; // 3 seconds
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override string Texture => "Zmod/Resources/None";

		public Projectile FirstProj;
		public Vector2 TargetPosition;
		public Vector2 ExplodePosition;

		// Modify Arrow lifetime to constant time
		public static float FirstProjLifeTime = 50;
		public override void AI()
		{
			// Timer
			Projectile.ai[0] += 1.0f;

			// FirstProjectile expires
			if (Projectile.ai[0] - 2.0f - FirstProjLifeTime >= 0)
			{
				// Spawn rain of arrows.
				const float ProjectileVelocity = 30.0f;

				// randomize within a span
				float MaxAngle = 3.14f / 16.0f;

				Vector2 VelDir = TargetPosition - ExplodePosition;
				VelDir.Normalize();

				// Random dir within span
				VelDir = VelDir.RotatedByRandom(MaxAngle);

				Vector2 vel = VelDir * ProjectileVelocity;
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), ExplodePosition, vel, ModContent.ProjectileType<DivineBowProjectile>(), 50, 1, Main.player[0].whoAmI);
			}
		}
		
		// Spawn the first projectile.
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);

			Vector2 MousePos = Main.MouseWorld;
			TargetPosition = MousePos;
			// Force target position min distance forward
			const float MinXDist = 450.0f;
			if (System.Math.Abs(TargetPosition.X - Projectile.position.X) <= MinXDist)
			{
				float DirSignX = Projectile.velocity.X / System.Math.Abs(Projectile.velocity.X);
				TargetPosition.X = Projectile.position.X + MinXDist * DirSignX;
			}
			Vector2 SpawnPosToTargetPos = TargetPosition - Projectile.position;
			float DistanceToMouse = SpawnPosToTargetPos.Length();

			float ShootHeight = -400.0f;
			float MaxShootX = 200.0f;

			// Vectors
			Vector2 ProjDir0 = new Vector2(DistanceToMouse / 3.0f, ShootHeight);
			ProjDir0.Normalize();
			Vector2 ProjDir1 = ProjDir0;
			ProjDir1.X *= -1.0f;

			// Lerp between 2 dir cases
			// Lerp amount depends on mouse distance
			float ProjDirLerpAmount = SpawnPosToTargetPos.X / MaxShootX;
			ProjDirLerpAmount = MathHelper.Clamp(ProjDirLerpAmount, 0.0f, 1.0f);
			Vector2 ProjDir = new Vector2(MathHelper.Lerp(ProjDir1.X, ProjDir0.X, ProjDirLerpAmount), MathHelper.Lerp(ProjDir1.Y, ProjDir0.Y, ProjDirLerpAmount));
			ProjDir.Normalize();

			Vector2 DistFromPlayerToPop = ProjDir * Projectile.velocity.Length() * FirstProjLifeTime;

			Main.NewText(ProjDir);

			// Be at the explode position at end of lifetime
			Vector2 vel = DistFromPlayerToPop / FirstProjLifeTime;
			ExplodePosition = Projectile.position + DistFromPlayerToPop;

			FirstProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel, ModContent.ProjectileType<DivineBowFirstProjectile>(), 50, 1, Main.player[0].whoAmI);

			// This is a dummy projectlie. Normalize vel for future use
			Projectile.velocity.Normalize();

			// Debug spawn target pos
			//Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), TargetPosition, new Vector2(0.0f), ModContent.ProjectileType<DivineBowFirstProjectile>(), 0, 0, Main.player[0].whoAmI);
			// Debug spawn explode pos
			//Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), ExplodePosition, new Vector2(0.0f), ModContent.ProjectileType<DivineBowFirstProjectile>(), 0, 0, Main.player[0].whoAmI);
		}

		public override void Kill(int timeLeft)
		{
		}
	}

	public class DivineBowFirstProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.damage = 50;
			Projectile.friendly = true;
			Projectile.timeLeft = 40;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}


		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);


		}

		public override void Kill(int timeLeft)
		{
			if(timeLeft != 0)
            {
				// Hit something
            }
			else
            {

            }
		}
	}

	public class DivineBowProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Projectile.damage = 50;
			Projectile.friendly = true;
			Projectile.timeLeft = 40;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);


		}
	}
}