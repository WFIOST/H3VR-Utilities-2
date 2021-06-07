using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace H3VRUtilities
{
	public class ManipulateObject : MonoBehaviour
	{
		[FormerlySerializedAs("ObservedObject")] [Header("Object Being Observed")]
		public GameObject observedObject;
		[FormerlySerializedAs("DirectionOfObservation")] public Common.Direction directionOfObservation;
		[FormerlySerializedAs("TransformationTypeOfObservedObject")] public Common.TransformType transformationTypeOfObservedObject;
		[FormerlySerializedAs("StartOfObservation")] public float startOfObservation;
		[FormerlySerializedAs("StopOfObservation")] public float stopOfObservation;

		[FormerlySerializedAs("AffectedObject")] [Header("Object Being Affected")]
		public GameObject affectedObject;
		[FormerlySerializedAs("DirectionOfAffection")] public Common.Direction directionOfAffection;
		[FormerlySerializedAs("TransformationTypeOfAffectedObject")] public Common.TransformType transformationTypeOfAffectedObject;
		[FormerlySerializedAs("StartOfAffected")] public float startOfAffected;
		[FormerlySerializedAs("StopOfAffected")] public float stopOfAffected;

		//[Header("Debug Values")]
		private float _observationpoint;
		private float _invertlerp;
		private float _lerppoint;
		private float _wiggleroom = 0.05f;

		[FormerlySerializedAs("SnapForwardsOnMax")]
		[Header("Special Affects")]
		[Tooltip("When the observed object reaches or exceeds the stopofobservation, the affected object will snap back to the startofaffected, and will only reset when the observed object reaches the startofobserved.")]
		public bool snapForwardsOnMax;



		private bool _snappedForwards;
		private bool _starttostopincreasesObservation;
		private bool _starttostopincreasesAffected;

		public void Update()
		{
			//define which is farther from the centre
			if (startOfObservation < stopOfObservation) _starttostopincreasesObservation = true;
			if (startOfAffected < stopOfAffected) _starttostopincreasesAffected = true;

			_observationpoint = transformationTypeOfObservedObject switch
			{
				Common.TransformType.Position					=> observedObject.transform.localPosition[(int) directionOfObservation],
				Common.TransformType.Rotation					=> observedObject.transform.localEulerAngles[(int) directionOfObservation],
				Common.TransformType.Scale						=> observedObject.transform.localScale[(int) directionOfObservation],
				Common.TransformType.Quaternion				=> observedObject.transform.localRotation[(int) directionOfObservation],
				Common.TransformType.QuaternionPresentedEuler	=> observedObject.transform.localRotation
					[(int) directionOfObservation] * 180,
				_ => _observationpoint
			};

			_invertlerp = Mathf.InverseLerp(startOfObservation, stopOfObservation, _observationpoint);

			if (snapForwardsOnMax)
			{
				//SnapForwardsOnMax test
				if (_starttostopincreasesObservation)
				{
					if (_observationpoint >= stopOfObservation - _wiggleroom)
					{
						_snappedForwards = true;
					}
				}
				else { if (_observationpoint <= stopOfObservation + _wiggleroom) _snappedForwards = true; }

				if (_starttostopincreasesObservation)
				{
					if (_observationpoint <= startOfObservation + _wiggleroom) _snappedForwards = false;
				}
				else { if (_observationpoint >= startOfObservation - _wiggleroom) _snappedForwards = false; }
				if (_snappedForwards) { _invertlerp = 0; }
				//end test
			}

			_lerppoint = Mathf.Lerp(startOfAffected, stopOfAffected, _invertlerp);



			Vector3 v3;

			switch (transformationTypeOfAffectedObject)
			{
				case Common.TransformType.Position:
					v3 = affectedObject.transform.localPosition;
					v3[(int)directionOfAffection] = _lerppoint;
					affectedObject.transform.localPosition = v3;
					break;
				case Common.TransformType.Rotation:
					v3 = affectedObject.transform.localEulerAngles;
					v3[(int)directionOfAffection] = _lerppoint;
					affectedObject.transform.localEulerAngles = v3;
					break;
				case Common.TransformType.Scale:
					v3 = affectedObject.transform.localScale;
					v3[(int)directionOfAffection] = _lerppoint;
					affectedObject.transform.localScale = v3;
					break;
				case Common.TransformType.Quaternion:
					var qt = affectedObject.transform.rotation;
					qt[(int)directionOfAffection] = _lerppoint;
					affectedObject.transform.localRotation = qt;
					break;
				case Common.TransformType.QuaternionPresentedEuler:
					v3 = affectedObject.transform.localEulerAngles;
					v3[(int)directionOfAffection] = _lerppoint;
					affectedObject.transform.localEulerAngles = v3;
					break;
			}
		}
	}
}
