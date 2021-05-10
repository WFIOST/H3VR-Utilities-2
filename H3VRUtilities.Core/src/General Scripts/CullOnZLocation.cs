using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace H3VRUtilities
{

	public class CullOnZLocation : MonoBehaviour
	{
		public enum CutOffType
		{
			Above,
			Below
		}
		public CutOffType cutoff;
		public double loc;
		public GameObject objTarget;
		public enum DirectionType
		{
			X = 0,
			Y = 1,
			Z = 2
		}
		public DirectionType dir;
		private MeshRenderer objMeshRenderer;


		void Start()
		{
			objMeshRenderer = gameObject.GetComponent<MeshRenderer>();
		}

		void Update()
		{
			switch (cutoff) {
				case CutOffType.Below:
					objMeshRenderer.enabled = objTarget.transform.localPosition[(int)dir] > loc;
					break;
				case CutOffType.Above:
					objMeshRenderer.enabled = objTarget.transform.localPosition[(int)dir] < loc;
					break;
			}
		}
	}
}
