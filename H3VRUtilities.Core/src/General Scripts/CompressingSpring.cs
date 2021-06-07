using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3VRUtils;
using UnityEngine;

namespace H3VRUtilities
{
	public class CompressingSpring : MonoBehaviour
	{
		public GameObject compressor;
		public GameObject spring;

		public Common.Direction directionOfCompression = Common.Direction.Z;

		public Common.Direction directionOfCompressor = Common.Direction.Z;

		[Tooltip("The directionOfCompression position where the scale will be 1.")]
		public float fullExtend;
		[Tooltip("The directionOfCompression position where the scale will be 0.")]
		public float fullCompress;
		void Update()
		{
			var localScale = spring.transform.localScale;
			float[] dir = new float[3];

			dir[0] = localScale.x;
			dir[1] = localScale.y;
			dir[2] = localScale.z;
			
			dir[(int)directionOfCompression] = (compressor.transform.localPosition[(int)directionOfCompressor] - fullCompress) * (1 / (fullExtend - fullCompress));

			localScale = new Vector3(dir[0], dir[1], dir[2]);
			spring.transform.localScale = localScale;
		}
	}
}
