using FistVR;
using UnityEngine;
using UnityEngine.Events;

namespace H3VRUtilities.Attachments
{
    public class OnAttach : MonoBehaviour  
    {
        public GameObject displayOnAttach;
        public FVRFireArmAttachmentMount attachmentMount;
        public UnityAction action;

        private bool _fired;
        public void Update()
        {
            if (attachmentMount.HasAttachmentsOnIt() && !_fired)
            {
                action.Invoke();
                _fired = true;
            }
            else
            {
                _fired = false;
            }
        }
    }
}