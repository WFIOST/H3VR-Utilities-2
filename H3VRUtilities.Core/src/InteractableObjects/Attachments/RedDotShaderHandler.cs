using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FistVR;
using UnityEngine;
using UnityEngine.UI;

namespace H3VRUtilities.Attachments
{
	public class RedDotShaderHandler : FVRFireArmAttachment
	{
		public bool enableMagnificationSettings;
		public MeshRenderer		reticle;
		public MeshRenderer		magnifier;
		public Canvas			settingsTextCanvas;
		public Text				reticleYOffsetText;
		public Text				reticleXOffsetText;
		public Text				magnifierMagnificationText;
		public Transform		highlightPosOffsetX;
		public Transform		highlightPosOffsetY;
		public Transform		highlightPosMagnification;
		public List<float>		offsetXNums;
		public List<string>		offsetXNames;
		public List<float>		offsetYNums;
		public List<string>		offsetYNames;
		public List<float>		magnification;
		public List<string>		magnificationNames;
		private Material		_matReticle;
		private Material		_matMagnifier;
		private Shader			_scopeShader;
		private Shader			_redDotShader;
		private int				_selectedtxt;

		public void Start()
		{
			_redDotShader = Shader.Find("RedDot(Unlit)");
			_scopeShader = Shader.Find("Magnification");
			_matReticle = reticle.material;
			_matMagnifier = magnifier.material;
			_matReticle.SetFloat("_OffsetX", 4f);
		}

		public static bool IfPressedInDir(FVRViveHand hand, Vector2 dir)
		{
			return Vector2.Angle
				(hand.Input.TouchpadAxes, dir) <= 
				45f && hand.Input.TouchpadDown && hand.Input.TouchpadAxes.magnitude > 0.2f;
		}

		public override void BeginInteraction(FVRViveHand hand)
		{
			base.BeginInteraction(hand);
			settingsTextCanvas.enabled = true;
		}

		public override void UpdateInteraction(FVRViveHand hand)
		{
			base.UpdateInteraction(hand);

			if (IfPressedInDir(hand, Vector2.up))
			{
				_selectedtxt++;
			}
			if (IfPressedInDir(hand, Vector2.left))
			{

			}
			var val = 1;
			if (enableMagnificationSettings) val++;
			if (_selectedtxt > val) _selectedtxt = 0;
		}

		public override void EndInteraction(FVRViveHand hand)
		{
			base.EndInteraction(hand);
			settingsTextCanvas.enabled = false;
		}

	}
}
