﻿using Terraria;
using tsorcRevamp.Items.Weapons.Melee.Broadswords.BroadswordRework.Common.Hooks.Items;
using tsorcRevamp.Items.Weapons.Melee.Broadswords.BroadswordRework.Core.ItemComponents;

namespace  tsorcRevamp.Items.Weapons.Melee.Broadswords.BroadswordRework.Common.Charging;

//TODO: Somehow make this have conditional instancing?
public sealed class ItemCharging : ItemComponent, IHoldItemWhileDead
{
	public delegate void ChargeAction(Item item, Player player, float chargeProgress);

	private ChargeAction? updateAction;
	private ChargeAction? endAction;
	private bool? allowTurning;

	public bool IsCharging { get; private set; }
	public int ChargeTime { get; private set; }
	public int ChargeTimeMax { get; private set; }

	public float ChargeProgress => IsCharging ? ChargeTime / (float)ChargeTimeMax : 0f;

	public override void HoldItem(Item item, Player player)
	{
		if (!player.dead) {
			UpdateCharging(item, player);
		}
	}

	public void StartCharge(int chargeTime, ChargeAction updateAction, ChargeAction endAction, bool? allowTurning = null)
	{
		IsCharging = true;
		ChargeTime = 0;
		ChargeTimeMax = chargeTime;

		this.updateAction = updateAction;
		this.endAction = endAction;
		this.allowTurning = allowTurning;
	}

	public void StopCharge(Item item, Player player, bool skipAction = false)
	{
		if (!skipAction) {
			endAction?.Invoke(item, player, ChargeProgress);
		}

		IsCharging = false;
		ChargeTime = ChargeTimeMax = 0;
		updateAction = null;
		endAction = null;
	}

	private void UpdateCharging(Item item, Player player)
	{
		if (!IsCharging) {
			return;
		}

		if (player.dead) {
			StopCharge(item, player, true);
			return;
		}

		updateAction?.Invoke(item, player, ChargeProgress);

		ChargeTime++;

		if (ChargeTime >= ChargeTimeMax) {
			StopCharge(item, player);
		}
	}

	void IHoldItemWhileDead.HoldItemWhileDead(Item item, Player player)
		=> UpdateCharging(item, player);
}
