using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace H3VRUtilities
{

	public class CullOnZLocation : MonoBehaviour
	{
		public Common.CutoffType cutoff;
		public double loc;
		public GameObject objTarget;
		public Common.Direction dir;
		private MeshRenderer objMeshRenderer;


		void Start()
		{
			objMeshRenderer = gameObject.GetComponent<MeshRenderer>();
		}

		void Update()
		{
			objMeshRenderer.enabled = cutoff switch
			{
				Common.CutoffType.Below => objTarget.transform.localPosition[(int) dir] > loc,
				Common.CutoffType.Above => objTarget.transform.localPosition[(int) dir] < loc,
				_ => objMeshRenderer.enabled
			};
		}
	}
}
