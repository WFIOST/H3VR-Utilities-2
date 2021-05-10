using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3VRUtils;
using UnityEngine;

namespace H3VRUtilities
{
	public class ConstantSpin : MonoBehaviour
	{
		public GameObject objectToSpin;
		public float spinRate;
		public CullOnZLocation.DirectionType directionOfSpeen;

		public void FixedUpdate()
		{
			var rot = new Vector3 { [(int) directionOfSpeen] = spinRate };
			objectToSpin.transform.Rotate(rot);
		}
	}
}
