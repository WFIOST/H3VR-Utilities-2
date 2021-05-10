using FistVR;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace H3VRUtilities
{
	public class DisplayBulletCount : MonoBehaviour
	{
		public FVRFireArm firearm;
		public FVRFireArmMagazine magazine;
		[FormerlySerializedAs("UItext")] public Text uItext;
		public string textWhenNoMag;

		[FormerlySerializedAs("KeepLastRoundInfoOnNoMag")] [Tooltip("When there is no mag, the text will remain whatever it was before.")]
		public bool keepLastRoundInfoOnNoMag;

		[FormerlySerializedAs("AddMinCharLength")] public bool addMinCharLength;
		[FormerlySerializedAs("IncludeChamberRound")] public bool includeChamberRound;
		[FormerlySerializedAs("MinCharLength")] public int minCharLength;
		private bool _wasFull;
		private bool _wasLoaded;
		private FVRFireArmChamber _chamber;

		public void Start()
		{
			_chamber = firearm switch
			{
				ClosedBoltWeapon weapon		=> weapon.Chamber,
				OpenBoltReceiver receiver	=> receiver.Chamber,
				Handgun handgun				=> handgun.Chamber,
				_							=> _chamber
			};
		}

		public void FixedUpdate()
		{
			string txt;

			if (firearm == null)
			{
				txt = magazine.m_numRounds.ToString();
				if (addMinCharLength)
				{
					var lengthneedtoadd = minCharLength - txt.Length;
					for (var i = 0; i < lengthneedtoadd; i++) txt = "0" + txt;
				}

				uItext.text = txt;
			} else if (firearm.Magazine != null)
			{
				

				if (includeChamberRound)
				{
					if ((_wasFull && !_chamber.IsFull) || !_wasLoaded)
					{

					}
					else
					{
						_wasFull = _chamber.IsFull;
						return;
					}

					_wasFull = _chamber.IsFull;
				}

				txt = firearm.Magazine.m_numRounds.ToString();
				if (addMinCharLength)
				{
					var lengthneedtoadd = minCharLength - txt.Length;
					for (var i = 0; i < lengthneedtoadd; i++) txt = "0" + txt;
				}

				uItext.text = txt;
				_wasLoaded = true;
			}
			else
			{
				if (!keepLastRoundInfoOnNoMag)
				{
					uItext.text = textWhenNoMag;
				}

				_wasLoaded = false;
			}
		}
	}
}