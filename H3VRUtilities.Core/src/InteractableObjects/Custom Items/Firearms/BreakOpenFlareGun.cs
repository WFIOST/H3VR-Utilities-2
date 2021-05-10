using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable CheckNamespace
namespace H3VRUtilities.InteractableObjects.CustomItems.Firearms
{
	public class BreakOpenFlareGun : FVRFireArm
	{
		[FormerlySerializedAs("GunUndamaged")] [Header("Flaregun Params")]
		public Renderer[] gunUndamaged;

		[FormerlySerializedAs("GunDamaged")] public Renderer[] gunDamaged;

		[FormerlySerializedAs("Chamber")] public FVRFireArmChamber chamber;

		[FormerlySerializedAs("HingeAxis")] public Axis hingeAxis;

		[FormerlySerializedAs("Hinge")] public Transform hinge;

		[FormerlySerializedAs("RotOut")] public float rotOut = 35f;

		[FormerlySerializedAs("CanUnlatch")] public bool canUnlatch = true;

		[FormerlySerializedAs("IsHighPressureTolerant")] public bool isHighPressureTolerant;

		private bool _mIsHammerCocked;

		private bool _mIsTriggerReset = true;

		private bool _mIsLatched = true;

		private bool _mIsDestroyed;

		private float _triggerFloat;

		[FormerlySerializedAs("Hammer")] public Transform hammer;

		[FormerlySerializedAs("HasVisibleHammer")] public bool hasVisibleHammer = true;

		[FormerlySerializedAs("CanCockHammer")] public bool canCockHammer = true;

		[FormerlySerializedAs("CocksOnOpen")] public bool cocksOnOpen;

		private float _mHammerXRot;

		[FormerlySerializedAs("HammerAxis")] public Axis hammerAxis;

		[FormerlySerializedAs("HammerInterp")] public InterpStyle hammerInterp = InterpStyle.Rotation;

		[FormerlySerializedAs("HammerMinRot")] public float hammerMinRot;

		[FormerlySerializedAs("HammerMaxRot")] public float hammerMaxRot = -70f;

		[FormerlySerializedAs("Trigger")] public Transform trigger;

		[FormerlySerializedAs("TriggerForwardBackRots")] public Vector2 triggerForwardBackRots;

		[FormerlySerializedAs("Muzzle")] public Transform muzzle;

		[FormerlySerializedAs("SmokePSystem")] public ParticleSystem smokePSystem;

		[FormerlySerializedAs("DestroyPSystem")] public ParticleSystem destroyPSystem;

		[FormerlySerializedAs("DeletesCartridgeOnFire")] public bool deletesCartridgeOnFire;
		
		// Token: 0x06003434 RID: 13364 RVA: 0x0016D073 File Offset: 0x0016B473
		public override void Awake()
		{
			base.Awake();
			if (canUnlatch)
			{
				chamber.IsAccessible = false;
			}
			else
			{
				chamber.IsAccessible = true;
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x0016D0A4 File Offset: 0x0016B4A4
		public override void FVRUpdate()
		{
			if (hasVisibleHammer)
			{
				_mHammerXRot = _mIsHammerCocked
					? Mathf.Lerp
					(
						_mHammerXRot,
						hammerMaxRot,
						Time.deltaTime * 12f
					)
					: Mathf.Lerp
					(
						_mHammerXRot,
						0f,
						Time.deltaTime * 25f
					);
				hammer.localEulerAngles = new Vector3(_mHammerXRot, 0f, 0f);
			}
			if (!_mIsLatched && Vector3.Angle(Vector3.up, chamber.transform.forward) < 70f && chamber.IsFull && chamber.IsSpent)
			{
				PlayAudioEvent(FirearmAudioEventType.MagazineEjectRound);
				chamber.EjectRound
				(
					chamber.transform.position + chamber.transform.forward * -0.06f,
					chamber.transform.forward * -0.01f,
					Vector3.right,
					false
				);
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0016D1E6 File Offset: 0x0016B5E6
		private void ToggleLatchState()
		{
			if (_mIsLatched)
				Unlatch();
			else if (!_mIsLatched)
				Latch();
			
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x0016D210 File Offset: 0x0016B610
		public override void UpdateInteraction(FVRViveHand hand)
		{
			base.UpdateInteraction(hand);
			var num = 0f;
			var hingeAxis = this.hingeAxis;
			if (hingeAxis != Axis.X)
			{
				if (hingeAxis != Axis.Y)
				{
					if (hingeAxis == Axis.Z)
					{
						num = transform.InverseTransformDirection(hand.Input.VelAngularWorld).z;
					}
				}
				else
				{
					num = transform.InverseTransformDirection(hand.Input.VelAngularWorld).y;
				}
			}
			else
			{
				num = transform.InverseTransformDirection(hand.Input.VelAngularWorld).x;
			}
			if (num > 15f && canUnlatch)
			{
				Unlatch();
			}
			else if (num < -15f && canUnlatch)
			{
				Latch();
			}
			if (hand.Input.TouchpadDown && !IsAltHeld)
			{
				var touchpadAxes = hand.Input.TouchpadAxes;
				if (touchpadAxes.magnitude > 0.2f && Vector2.Angle(touchpadAxes, Vector2.down) < 45f && canCockHammer)
				{
					CockHammer();
				}
				else if (touchpadAxes.magnitude > 0.2f && (Vector2.Angle(touchpadAxes, Vector2.left) < 45f || Vector2.Angle(touchpadAxes, Vector2.right) < 45f) && canUnlatch)
				{
					ToggleLatchState();
				}
			}
			if (_mIsDestroyed)
			{
				return;
			}
			if (m_hasTriggeredUpSinceBegin && !IsAltHeld)
			{
				_triggerFloat = hand.Input.TriggerFloat;
			}
			else
			{
				_triggerFloat = 0f;
			}
			var x = Mathf.Lerp(triggerForwardBackRots.x, triggerForwardBackRots.y, _triggerFloat);
			trigger.localEulerAngles = new Vector3(x, 0f, 0f);
			if (_triggerFloat > 0.7f)
			{
				if (_mIsTriggerReset && _mIsHammerCocked)
				{
					_mIsTriggerReset = false;
					_mIsHammerCocked = false;
					if (hammer != null)
					{
						SetAnimatedComponent(hammer, hammerMinRot, hammerInterp, hammerAxis);
					}
					PlayAudioEvent(FirearmAudioEventType.HammerHit, 1f);
					Fire();
				}
			}
			else if (_triggerFloat < 0.2f && !_mIsTriggerReset)
			{
				_mIsTriggerReset = true;
			}
		}

		private void Fire()
		{
			if (!_mIsLatched)
			{
				return;
			}
			if (!chamber.Fire())
			{
				return;
			}
			base.Fire(chamber, GetMuzzle(), true, 1f);
			FireMuzzleSmoke();
			var twoHandStabilized = IsTwoHandStabilized();
			var foregripStabilized = AltGrip != null;
			var shoulderStabilized = IsShoulderStabilized();
			if (chamber.GetRound().IsHighPressure && !isHighPressureTolerant)
			{
				Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
				Destroy();
			}
			else if (isHighPressureTolerant)
			{
				Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
			}
			PlayAudioGunShot(chamber.GetRound(), GM.CurrentPlayerBody.GetCurrentSoundEnvironment(), 1f);
			if (GM.CurrentSceneSettings.IsAmmoInfinite || GM.CurrentPlayerBody.IsInfiniteAmmo)
			{
				chamber.IsSpent = false;
				chamber.UpdateProxyDisplay();
			}
			else if (chamber.GetRound().IsCaseless)
			{
				chamber.SetRound(null);
			}
			if (deletesCartridgeOnFire)
			{
				chamber.SetRound(null);
			}
		}

		public void Unlatch()
		{
			if (_mIsLatched)
			{
				PlayAudioEvent(FirearmAudioEventType.BreachOpen, 1f);
				_mIsLatched = false;
				chamber.IsAccessible = true;
				if (cocksOnOpen)
				{
					CockHammer();
				}
			}
		}

		public void Latch()
		{
			if (!_mIsLatched)
			{
				PlayAudioEvent(FirearmAudioEventType.BreachClose, 1f);
				_mIsLatched = true;
				chamber.IsAccessible = false;
			}
		}

		private void CockHammer()
		{
			if (!_mIsHammerCocked)
			{
				PlayAudioEvent(FirearmAudioEventType.Prefire, 1f);
				_mIsHammerCocked = true;
				if (hammer != null)
				{
					SetAnimatedComponent(hammer, hammerMaxRot, hammerInterp, hammerAxis);
				}
			}
		}

		private void Destroy()
		{
			if (!_mIsDestroyed)
			{
				_mIsDestroyed = true;
				destroyPSystem.Emit(25);
				for (int i = 0; i < gunUndamaged.Length; i++)
				{
					gunUndamaged[i].enabled = false;
					gunDamaged[i].enabled = true;
				}
			}
		}

		public override List<FireArmRoundClass>? GetChamberRoundList()
		{
			if (chamber.IsFull && !chamber.IsSpent)
			{
				return new List<FireArmRoundClass>
				{
					chamber.GetRound().RoundClass
				};
			}
			return null;
		}

		public override void SetLoadedChambers(List<FireArmRoundClass> rounds)
		{
			if (rounds.Count > 0)
			{
				chamber.Autochamber(rounds[0]);
			}
		}

		public override List<string> GetFlagList()
		{
			return null;
		}

		public override void SetFromFlagList(List<string> flags)
		{
		}


	}
}
