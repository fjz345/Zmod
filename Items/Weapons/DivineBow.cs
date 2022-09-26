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
			// Debug Get item
			//Rectangle rect = new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0);
			//Item.NewItem(player.GetSource_Loot(), rect, ItemID.WoodenArrow, 100);

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

		public static Projectile FirstProj;
		public static Vector2 TargetPosition;
		public static Vector2 ExplodePosition;

		// Modify Arrow lifetime to constant time
		public static float FirstProjLifeTime = 50;
		public override void AI()
		{
			// if First projectile is alive
			//if(FirstProj != null && FirstProj.timeLeft > 0)
            {
				// Timer
				Projectile.ai[0] += 1.0f;

				// FirstProjectile expires
				if(Projectile.ai[0] - 2.0f - FirstProjLifeTime >= 0)
                {
					// Spawn rain of arrows.
					const float ProjectileVelocity = 30.0f;

					// randomize within a span
					float SpanX = 200.0f;
					float t0 = SpanX / 2.0f;
					float t1 = -SpanX / 2.0f;

					Vector2 VelDir0 = new Vector2(TargetPosition.X + t0, TargetPosition.Y) - ExplodePosition;
					Vector2 VelDir1 = new Vector2(TargetPosition.X + t1, TargetPosition.Y) - ExplodePosition;

					float Angle = VelDir0.AngleTo(VelDir1);
					float Rand = Main.rand.NextFloat(1.0f);
					float Rand2 = Main.rand.NextFloat(1.0f);
					float VelDirX = MathHelper.Lerp(VelDir0.X, VelDir1.X, Rand);
					float VelDirY = MathHelper.Lerp(VelDir0.Y, VelDir1.Y, Rand2);
					Vector2 VelDir = new Vector2(VelDirX, VelDirY);
					VelDir.Normalize();

					Vector2 vel = VelDir * ProjectileVelocity;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), ExplodePosition, vel, ModContent.ProjectileType<DivineBowProjectile>(), 50, 1, Main.player[0].whoAmI);
				}
			}
		}
		
		// Spawn the first projectile.
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);

			// This is a dummy projectlie. Normalize vel for future use
			Projectile.velocity.Normalize();

			Vector2 MousePos = Main.MouseWorld;
			TargetPosition = MousePos;
			float MinDistance = 700.0f;
			float DistanceToMouse = MousePos.Distance(Projectile.position);
			DistanceToMouse = MathHelper.Max(DistanceToMouse, MinDistance);

			Vector2 DistFromPlayerToPop = new Vector2(0.3f, -0.4f) * DistanceToMouse;
			// Flip if shooting other way
			if (Projectile.velocity.X < 0.0f)
			{
				DistFromPlayerToPop.X *= -1.0f;
			}

			// Vectors
			Vector2 UpVector = new Vector2(0, -1);
			UpVector.Normalize();
			Vector2 AimDir = MousePos - Projectile.position;
			AimDir.Normalize();
			Vector2 ProjDir = DistFromPlayerToPop;
			ProjDir.Normalize();

			float ProjDirDist = (float)System.Math.Sqrt(DistFromPlayerToPop.X * DistFromPlayerToPop.X + DistFromPlayerToPop.Y * DistFromPlayerToPop.Y);

			// Be at the explode position at end of lifetime
			Vector2 vel = ProjDir * ProjDirDist / FirstProjLifeTime;
			ExplodePosition = Projectile.position + ProjDir * ProjDirDist;

			FirstProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, vel, ModContent.ProjectileType<DivineBowFirstProjectile>(), 50, 1, Main.player[0].whoAmI);

			// Force target position min distance forward
			const float MinSpanDist = 300.0f;
			if(ExplodePosition.Distance(TargetPosition) <= MinSpanDist)
            {
				Vector2 dir = -(TargetPosition - ExplodePosition);
				dir.Normalize();
				TargetPosition -= dir * MinSpanDist;
			}
		}

		public override void Kill(int timeLeft)
		{
			FirstProj = null;
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