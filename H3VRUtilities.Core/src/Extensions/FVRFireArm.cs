using System.Collections.Generic;
using System.Linq;
using FistVR;

namespace H3VRUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns a list of all attached objects on the given firearm. This includes attached magazines
        /// </summary>
        /// <param name="fireArm">The firearm that is being scanned for attachmnets</param>
        /// <param name="includeSelf">If true, includes the given firearm as the first item in the list of attached objects</param>
        /// <returns>A list containing every attached item on the given firearm</returns>
        public static IEnumerable<FVRPhysicalObject> GetAllAttachedObjects(this FVRFireArm fireArm, bool includeSelf = false)
        {
            List<FVRPhysicalObject> detectedObjects = new List<FVRPhysicalObject>();

            if (includeSelf) detectedObjects.Add(fireArm);

            if (fireArm.Magazine is not null && !fireArm.Magazine.IsIntegrated && fireArm.Magazine.ObjectWrapper is not null)
            {
                detectedObjects.Add(fireArm.Magazine);
            }

            detectedObjects.AddRange(fireArm.Attachments.Where(attachment => attachment.ObjectWrapper is not null).Cast<FVRPhysicalObject>());

            return detectedObjects;
        }
    }
}