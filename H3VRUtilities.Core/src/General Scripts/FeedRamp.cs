using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using UnityEngine.Serialization;

namespace H3VRUtilities
{
	public class FeedRamp : MonoBehaviour
	{
		[FormerlySerializedAs("Carrier")] public GameObject carrier;
		public FVRFireArm firearm;
		[FormerlySerializedAs("CarrierDetectDistance")] public float carrierDetectDistance;
		[FormerlySerializedAs("CarrierRots")] public Vector2 carrierRots;
		[FormerlySerializedAs("CarrierComparePoint1")] public Transform carrierComparePoint1;
		[FormerlySerializedAs("CarrierComparePoint2")] public Transform carrierComparePoint2;
		private float _mCurCarrierRot;
		private float _mTarCarrierRot;

		public void Update()
		{
			if (firearm.IsHeld)
			{
				if (firearm.m_hand.OtherHand.CurrentInteractable != null)
				{
					if (firearm.m_hand.OtherHand.CurrentInteractable is FVRFireArmRound)
					{
						var currentInteractableObject = firearm.m_hand.OtherHand.CurrentInteractable.transform;
						var num = Vector3.Distance
						(
							currentInteractableObject.position,
							firearm.GetClosestValidPoint
							(
								carrierComparePoint1.position,
								carrierComparePoint2.position,
								currentInteractableObject.position
							)
						);
						_mTarCarrierRot = num < carrierDetectDistance ? carrierRots.y : carrierRots.x;
					}
					else
					{
						_mTarCarrierRot = carrierRots.x;
					}
				}
				else
				{
					_mTarCarrierRot = carrierRots.x;
				}
			}
			else
			{
				_mTarCarrierRot = carrierRots.x;
			}

			if (Mathf.Abs(_mCurCarrierRot - _mTarCarrierRot) > 0.001f)
			{
				_mCurCarrierRot = Mathf.MoveTowards
				(
					_mCurCarrierRot,
					_mTarCarrierRot,
					270f * Time.deltaTime
				);
				carrier.transform.localEulerAngles = new Vector3(_mCurCarrierRot, 0f, 0f);
			}
		}
	}
}
