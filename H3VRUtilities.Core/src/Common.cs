using System;
using UnityEngine;

namespace H3VRUtilities
{
    /// <summary>
    /// Common Utilities for H3VR Utilities
    /// </summary>
    public static class Common
    {
        [Serializable]
        public struct KeyValuePair
        {
            public string key;
            public string value;
        }
        
        public enum Direction
        {
            X = 0x0,
            Y = 0x1,
            Z = 0x2
        }
        
        public enum CutoffType
        {
            Above = 0x0,
            Below = 0x1
        }
        
        public enum TransformType
        {
            Position,
            Rotation,
            Scale,
            Quaternion,
            QuaternionPresentedEuler
        }
    }
}