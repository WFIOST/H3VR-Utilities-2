using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3VRUtils;
using UnityEngine;

namespace H3VRUtilities
{
	public class FollowDirection : MonoBehaviour
	{
		public GameObject leader;
		public GameObject follower;
		public Common.Direction direction;

		public Vector3 followerPosition;
		public Vector3 leaderPosition;
		public Vector3 resultPosition;

		void Update()
		{
			follower.transform.position = leader.transform.position;
		}
	}
}
