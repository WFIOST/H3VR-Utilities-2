using FistVR;
using UnityEngine;

#pragma warning disable 8618

namespace H3VRUtilities.Attachments
{
    public enum Actions
    {
        Attach,
        Detach
    }
    public enum Cap
    {
        AddTo,
        SetTo
    }
    
    public class AttachmentPlus : MonoBehaviour
    {
        [Header("Recoil Modifier")]
        public bool attachmentChangesRecoil;

        //public FVRFireArmRecoilProfile modifiedRecoil;
        //private FVRFireArmRecoilProfile _originalRecoil;
        

        [Header("Magazine Modifer")]
        public bool attachmentChangesMagazineCapacity;
        [Tooltip("Keep it off unless you're sure it should apply to non-internal mags.")]
        public bool affectsNonInternalMags;
        public int setCapacityTo;
        public Cap capacityModifier;
        
        private int _prevCapacity;

        [Header("Bolt Speed Modifier")]
        public bool attachmentChangesBoltSpeed;
        public bool attachmentChangesBoltSpeedForward;
        public bool attachmentChangesBoltSpeedRearward;
        public bool attachmentChangesBoltSpeedStiffness;
        public Cap boltSpeedModifier;
        public float newBoltSpeedForward;
        private float _prevBoltSpeedForward;
        public float newBoltSpeedBackwards;
        private float _prevBoltSpeedBackwards;
        public float newBoltSpringStiffness;
        private float _prevBoltSpringStiffness;
        
        /*
        [Header("Spread Modifier")]
        [HideInInspector]
        public bool attachmentChangesSpread;
        [HideInInspector]
        public float spreadMultiplier;
        */
        
        [Header("GrabPos Modifier")]
        public bool attachmentChangesGrabPosition;
        public Transform newPoseOverride;
        private Transform _oldPoseOverride;
        public Transform newPoseOverrideTouch;
        private Transform _oldPoseOverrideTouch;

        //private FVRFireArmAttachment _attachment;
        //private FVRFireArm _weapon;
    }
}