﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp {
	public struct Timer {
		private uint endTime;

		public bool Active => Main.GameUpdateCount < endTime;
		public uint Value {
			get => (uint)Math.Max(0, (long)endTime - Main.GameUpdateCount);
			set => endTime = Main.GameUpdateCount + Math.Max(0, value);
		}

		public void Set(uint minValue) => Value = Math.Max(minValue, Value);

		public static implicit operator Timer(uint value) => new Timer() { Value = value };
		public static implicit operator Timer(int value) => new Timer() { Value = (uint)value };
	}

	public enum PlayerFrames {
		Idle,
		Use1,
		Use2,
		Use3,
		Use4,
		Jump,
		Walk1,
		Walk2,
		Walk3,
		Walk4,
		Walk5,
		Walk6,
		Walk7,
		Walk8,
		Walk9,
		Walk10,
		Walk11,
		Walk12,
		Walk13,
		Walk14,
		Count
	}
	public partial class tsorcRevampPlayer : ModPlayer {
		public static float DodgeTimeMax => 0.37f;
		public static uint DodgeDefaultCooldown => 30;

		public Timer dodgeCooldown;
		public sbyte dodgeDirection;
		public sbyte dodgeDirectionVisual;
		public sbyte wantedDodgerollDir;
		public float dodgeTime;
		public float dodgeStartRot;
		public float dodgeItemRotation;
		public bool isDodging;
		public float wantsDodgerollTimer;
		public bool forceDodgeroll;
		public bool noDodge;
		public float rotation;
		public float? forcedItemRotation;
		public PlayerFrames? forcedHeadFrame;
		public PlayerFrames? forcedBodyFrame;
		public PlayerFrames? forcedLegFrame;
		public int forcedDirection;

		//CanX
		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) => !isDodging;
		public override bool CanBeHitByProjectile(Projectile proj) => !isDodging;
		public override bool PreItemCheck() {
			UpdateDodging();
			UpdateSwordflip();

			//Stop umbrella and other things from working
			if (isDodging && player.HeldItem.type == ItemID.Umbrella) {
				return false;
			}

			return true;
		}
		public void QueueDodgeroll(float wantTime, sbyte direction, bool force = false) {
			wantsDodgerollTimer = wantTime;
			wantedDodgerollDir = direction;

			if (force) {
				dodgeCooldown = 0;
			}
		}

		public int KeyDirection(Player player) => player.controlLeft ? -1 : player.controlRight ? 1 : 0;
		public static bool OnGround(Player player) => player.velocity.Y == 0f;
		public static bool WasOnGround(Player player) => player.oldVelocity.Y == 0f;
		public static float StepTowards(float value, float goal, float step) {
			if (goal > value) {
				value += step;

				if (value > goal) {
					return goal;
				}
			}
			else if (goal < value) {
				value -= step;

				if (value < goal) {
					return goal;
				}
			}

			return value;
		}

		private bool TryStartDodgeroll() {
			bool isLocal = player.whoAmI == Main.myPlayer;

			if (isLocal && wantsDodgerollTimer <= 0f && tsorcRevamp.DodgerollKey.JustPressed && !player.mouseInterface && player.GetModPlayer<tsorcRevampStaminaPlayer>().staminaResourceCurrent > 30) {
				QueueDodgeroll(0.25f, (sbyte)KeyDirection(player));
				player.GetModPlayer<tsorcRevampStaminaPlayer>().staminaResourceCurrent -= 30;
			}

			if (!forceDodgeroll) {
				//Only initiate dodgerolls locally.
				if (!isLocal) {
					return false;
				}

				//Input & cooldown check. The cooldown can be enforced by other actions.
				if (wantsDodgerollTimer <= 0f || dodgeCooldown.Active) {
					return false;
				}

				//Don't allow dodging on mounts and during item use.
				if ((player.mount != null && player.mount.Active) || player.itemAnimation > 0) {
					return false;
				}
			}

			wantsDodgerollTimer = 0f;
			player.grappling[0] = -1;
			player.grapCount = 0;
			for (int p = 0; p < 1000; p++) {
				if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].aiStyle == 7) {
					Main.projectile[p].Kill();
				}
			}

			player.eocHit = 1;

			isDodging = true;
			player.immune = true;
			player.immuneTime = 15;
			dodgeStartRot = player.GetModPlayer<tsorcRevampPlayer>().rotation;
			dodgeItemRotation = player.itemRotation;
			dodgeTime = 0f;
			dodgeDirectionVisual = (sbyte)player.direction;
			dodgeDirection = wantedDodgerollDir != 0 ? wantedDodgerollDir : (sbyte)player.direction;
			dodgeCooldown = DodgeDefaultCooldown;

			if (!isLocal) {
				forceDodgeroll = false;
			}
			else if (Main.netMode != NetmodeID.SinglePlayer) {
				//MultiplayerSystem.SendPacket(new PlayerDodgerollPacket(player));
			}

			return true;
		}
		private void UpdateDodging() {
			wantsDodgerollTimer = StepTowards(wantsDodgerollTimer, 0f, (float)1 / 60);

			noDodge |= player.mount.Active;

			if (noDodge) {
				isDodging = false;
				noDodge = false;

				return;
			}

			bool onGround = OnGround(player);
			ref float rotation = ref player.GetModPlayer<tsorcRevampPlayer>().rotation;

			//Attempt to initiate a dodgeroll if the player isn't doing one already.
			if (!isDodging && !TryStartDodgeroll()) {
				return;
			}
			//Apply velocity
			if (dodgeTime < DodgeTimeMax * 0.5f) {
				float newVelX = (onGround ? 6f : 4f) * dodgeDirection;

				if (Math.Abs(player.velocity.X) < Math.Abs(newVelX) || Math.Sign(newVelX) != Math.Sign(player.velocity.X)) {
					player.velocity.X = newVelX;
				}

			}

			player.pulley = false;

			//Apply rotations & direction
			forcedItemRotation = dodgeItemRotation;
			forcedLegFrame = PlayerFrames.Jump;
			forcedDirection = dodgeDirectionVisual;

			rotation = dodgeDirection == 1
				? Math.Min(MathHelper.Pi * 2f, MathHelper.Lerp(dodgeStartRot, MathHelper.TwoPi, dodgeTime / (DodgeTimeMax * 1f)))
				: Math.Max(-MathHelper.Pi * 2f, MathHelper.Lerp(dodgeStartRot, -MathHelper.TwoPi, dodgeTime / (DodgeTimeMax * 1f)));
			  //Progress the dodgeroll
			dodgeTime += 1f / 60f;

			if (dodgeTime >= DodgeTimeMax * 0.6f) {
				player.velocity.X *= 0.9f;
            }

			if (dodgeTime >= DodgeTimeMax) {
				isDodging = false;
				player.eocDash = 0;

				//forceSyncControls = true;
			}
			else {
				player.runAcceleration = 0f;
			}
		}
	}
}
