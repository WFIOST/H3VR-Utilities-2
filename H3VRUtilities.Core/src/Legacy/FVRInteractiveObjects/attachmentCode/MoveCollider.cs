using System;
using FistVR;
using UnityEngine;

namespace H3VRUtils
{
    public class MoveCollider : MonoBehaviour
    {
        public Collider                     collider;
        public Transform                    locationToMoveTo;
        public FVRFireArmAttachmentMount    attachmentMount;

        private bool _hasMoved;

        private Transform _initPos;

        private void Awake()
        {
            _initPos = collider.transform;
        }

        private void Update()
        {
            if (attachmentMount.HasAttachmentsOnIt() && !_hasMoved)
            {
                collider.transform.position = locationToMoveTo.position;
                _hasMoved = true;
            }
            else if (!attachmentMount.HasAttachmentsOnIt() && _hasMoved)
            {
                collider.transform.position = _initPos.position;
                _hasMoved = false;
            }
        }
    }
}