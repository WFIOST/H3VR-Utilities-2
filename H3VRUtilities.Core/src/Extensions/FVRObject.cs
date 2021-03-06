using System.Collections.Generic;
using System.Linq;
using FistVR;

namespace H3VRUtilities.Extensions
{
    public static partial class Extensions
    {
		/// <summary>
		/// Returns a list of magazines, clips, or speedloaders compatible with the firearm, and also within any of the optional criteria
		/// </summary>
		/// <param name="firearm">The FVRObject of the firearm</param>
		/// <param name="minCapacity">The minimum capacity for desired containers</param>
		/// <param name="maxCapacity">The maximum capacity for desired containers. If this values is zero or negative, it is interpreted as no capacity ceiling</param>
		/// <param name="smallestIfEmpty">If true, when the returned list would normally be empty, will instead return the smallest capacity magazine compatible with the firearm</param>
		/// <param name="blacklistedContainers">A list of ItemIDs for magazines, clips, or speedloaders that will be excluded</param>
		/// <returns> A list of ammo container FVRObjects that are compatible with the given firearm </returns>
		public static IEnumerable<FVRObject> GetCompatibleAmmoContainers(this FVRObject firearm, int minCapacity = 0, int maxCapacity = 9999, bool smallestIfEmpty = true, IEnumerable<string>? blacklistedContainers = null)
		{
			//Refresh the FVRObject to have data directly from object dictionary
			firearm = IM.OD[firearm.ItemID];

			//If the max capacity is zero or negative, we iterpret that as no limit on max capacity
			if (maxCapacity <= 0) maxCapacity = 9999;

			//Create a list containing all compatible ammo containers
			List<FVRObject> compatibleContainers = new List<FVRObject>();
			if (firearm.CompatibleMagazines is not null) compatibleContainers.AddRange(firearm.CompatibleMagazines);
			if (firearm.CompatibleClips is not null) compatibleContainers.AddRange(firearm.CompatibleClips);
			if (firearm.CompatibleSpeedLoaders is not null) compatibleContainers.AddRange(firearm.CompatibleSpeedLoaders);

			//Go through these containers and remove any that don't fit given criteria
			for (int i = compatibleContainers.Count - 1; i >= 0; i--)
			{
				if (blacklistedContainers is not null && blacklistedContainers.Contains(compatibleContainers[i].ItemID))
				{
					compatibleContainers.RemoveAt(i);
				}

				if (compatibleContainers[i].MagazineCapacity < minCapacity || compatibleContainers[i].MagazineCapacity > maxCapacity)
				{
					compatibleContainers.RemoveAt(i);
				}
			}

			//If the resulting list is empty, and smallestIfEmpty is true, add the smallest capacity magazine to the list
			if (compatibleContainers.Count != 0 || !smallestIfEmpty || firearm.CompatibleMagazines is null)
				return compatibleContainers;
			FVRObject? magazine = GetSmallestCapacityMagazine(firearm.CompatibleMagazines);
			if (magazine is not null) compatibleContainers.Add(magazine);

			return compatibleContainers;
		}
        
		/// <summary>
		/// Returns the smallest capacity magazine from the given list of magazine FVRObjects
		/// </summary>
		/// <param name="magazines">A list of magazine FVRObjects</param>
		/// <param name="blacklistedMagazines">A list of ItemIDs for magazines that will be excluded</param>
		/// <returns>An FVRObject for the smallest magazine. Can be null if magazines list is empty</returns>
		public static FVRObject? GetSmallestCapacityMagazine(this IEnumerable<FVRObject> magazines, IEnumerable<string>? blacklistedMagazines = null)
		{
			if (magazines is null || !magazines.Any()) return null;

			//This was done with a list because whenever there are multiple smallest magazines of the same size, we want to return a random one from those options
			List<FVRObject> smallestMagazines = new List<FVRObject>();

			foreach (FVRObject magazine in magazines)
			{
				if (blacklistedMagazines is not null && blacklistedMagazines.Contains(magazine.ItemID)) continue;

				else if (smallestMagazines.Count == 0) smallestMagazines.Add(magazine);

				//If we find a new smallest mag, clear the list and add the new smallest
				else if (magazine.MagazineCapacity < smallestMagazines[0].MagazineCapacity)
				{
					smallestMagazines.Clear();
					smallestMagazines.Add(magazine);
				}

				//If the magazine is the same capacity as current smallest, add it to the list
				else if (magazine.MagazineCapacity == smallestMagazines[0].MagazineCapacity)
				{
					smallestMagazines.Add(magazine);
				}
			}


			return smallestMagazines.Count == 0 ? null : smallestMagazines.GetRandom();

			//Return a random magazine from the smallest
		}
		
		/// <summary>
		/// Returns the smallest capacity magazine that is compatible with the given firearm
		/// </summary>
		/// <param name="firearm">The FVRObject of the firearm</param>
		/// <param name="blacklistedMagazines">A list of ItemIDs for magazines that will be excluded</param>
		/// <returns>An FVRObject for the smallest magazine. Can be null if firearm has no magazines</returns>
		public static FVRObject? GetSmallestCapacityMagazine(this FVRObject firearm, IEnumerable<string>? blacklistedMagazines = null)
		{
			//Refresh the FVRObject to have data directly from object dictionary
			firearm = IM.OD[firearm.ItemID];

			return GetSmallestCapacityMagazine(firearm.CompatibleMagazines, blacklistedMagazines);
		}
		
		/// <summary>
		/// Returns true if the given FVRObject has any compatible rounds, clips, magazines, or speedloaders
		/// </summary>
		/// <param name="item">The FVRObject that is being checked</param>
		/// <returns>True if the FVRObject has any compatible rounds, clips, magazines, or speedloaders. False if it contains none of these</returns>
		public static bool FVRObjectHasAmmoObject(this FVRObject item)
		{
			if (item == null) return false;

			//Refresh the FVRObject to have data directly from object dictionary
			item = IM.OD[item.ItemID];

			return (item.CompatibleSingleRounds != null && item.CompatibleSingleRounds.Count != 0) || (item.CompatibleClips != null && item.CompatibleClips.Count > 0) || (item.CompatibleMagazines != null && item.CompatibleMagazines.Count > 0) || (item.CompatibleSpeedLoaders != null && item.CompatibleSpeedLoaders.Count != 0);
		}
		
		/// <summary>
		/// Returns true if the given FVRObject has any compatible clips, magazines, or speedloaders
		/// </summary>
		/// <param name="item">The FVRObject that is being checked</param>
		/// <returns>True if the FVRObject has any compatible clips, magazines, or speedloaders. False if it contains none of these</returns>
		public static bool FVRObjectHasAmmoContainer(this FVRObject item)
		{
			if (item == null) return false;

			//Refresh the FVRObject to have data directly from object dictionary
			item = IM.OD[item.ItemID];

			return (item.CompatibleClips is {Count: > 0}) || (item.CompatibleMagazines != null && item.CompatibleMagazines.Count > 0) || (item.CompatibleSpeedLoaders != null && item.CompatibleSpeedLoaders.Count != 0);
		}
		
	    /// <summary>
		/// Returns the next largest magazine when compared to the current magazine. Only magazines from the possibleMagazines list are considered as next largest magazine candidates
		/// </summary>
		/// <param name="currentMagazine">The base magazine FVRObject, for which we are getting the next largest magazine</param>
		/// <param name="possibleMagazines">A list of magazine FVRObjects, which are the candidates for being the next largest magazine</param>
		/// <param name="blacklistedMagazines">A list of ItemIDs for magazines that will be excluded</param>
		/// <returns>An FVRObject for the next largest magazine. Can be null if no next largest magazine is found</returns>
		public static FVRObject? GetNextHighestCapacityMagazine(this FVRObject currentMagazine, IEnumerable<FVRObject> possibleMagazines, IEnumerable<string>? blacklistedMagazines = null)
		{
			if (possibleMagazines is null || !possibleMagazines.Any()) return null;

			//We make this a list so that when several next largest mags have the same capacity, we can return a random magazine from that selection
			List<FVRObject> nextLargestMagazines = new List<FVRObject>();

			foreach (FVRObject magazine in possibleMagazines)
			{
				if (blacklistedMagazines is not null && blacklistedMagazines.Contains(magazine.ItemID)) continue;

				else if (nextLargestMagazines.Count == 0) nextLargestMagazines.Add(magazine);

				//If our next largest mag is the same size as the original, then we take the new larger mag
				if (magazine.MagazineCapacity > currentMagazine.MagazineCapacity && currentMagazine.MagazineCapacity == nextLargestMagazines[0].MagazineCapacity)
				{
					nextLargestMagazines.Clear();
					nextLargestMagazines.Add(magazine);
				}

				//We want the next largest mag size, so the minimum mag size that's also greater than the current mag size
				else if (magazine.MagazineCapacity > currentMagazine.MagazineCapacity && magazine.MagazineCapacity < nextLargestMagazines[0].MagazineCapacity)
				{
					nextLargestMagazines.Clear();
					nextLargestMagazines.Add(magazine);
				}

				//If this magazine has the same capacity as the next largest magazines, add it to the list of options
				else if (magazine.MagazineCapacity == nextLargestMagazines[0].MagazineCapacity)
				{
					nextLargestMagazines.Add(magazine);
				}
			}

			//If the capacity has not increased compared to the original, we should return null
			return nextLargestMagazines[0].MagazineCapacity == currentMagazine.MagazineCapacity ? null : nextLargestMagazines.GetRandom();
		}
				
	    /// <summary>
		/// Returns a list of FVRPhysicalObjects for items that are either in the players hand, or in one of the players quickbelt slots. This also includes any items in a players backpack if they are wearing one
		/// </summary>
		/// <returns>A list of FVRPhysicalObjects equipped on the player</returns>
		public static IEnumerable<FVRPhysicalObject> GetEquippedItems()
		{
			List<FVRPhysicalObject> heldItems = new List<FVRPhysicalObject>();

			FVRInteractiveObject rightHandObject = GM.CurrentMovementManager.Hands[0].CurrentInteractable;
			FVRInteractiveObject leftHandObject = GM.CurrentMovementManager.Hands[1].CurrentInteractable;

			//Get any items in the players hands
			if (rightHandObject is FVRPhysicalObject rightHandObj)
			{
				heldItems.Add(rightHandObj);
			}

			if (leftHandObject is FVRPhysicalObject leftHandObj)
			{
				heldItems.Add(leftHandObj);
			}

			//Get any items on the players body
			foreach (FVRQuickBeltSlot slot in GM.CurrentPlayerBody.QuickbeltSlots)
			{
				if (slot.CurObject is not null && slot.CurObject.ObjectWrapper is not null)
				{
					heldItems.Add(slot.CurObject);
				}

				//If the player has a backpack on, we should search through that as well
				if (slot.CurObject is PlayerBackPack backpack && backpack.ObjectWrapper is not null)
				{
					heldItems.AddRange(from backpackSlot in GM.CurrentPlayerBody.QuickbeltSlots where backpackSlot.CurObject is not null select backpackSlot.CurObject);
				}
			}

			return heldItems;
		}
		
	    /// <summary>
	    /// Returns a list of FVRObjects for all of the items that are equipped on the player. Items without a valid FVRObject are excluded. There may also be duplicate entries if the player has identical items equipped
	    /// </summary>
	    /// <returns>A list of FVRObjects equipped on the player</returns>
	    public static IEnumerable<FVRObject> GetEquippedFVRObjects() => (from item in GetEquippedItems() where item.ObjectWrapper is not null select item.ObjectWrapper);
	    
	    /// <summary>
	    /// Returns a random magazine, clip, or speedloader that is compatible with one of the players equipped items
	    /// </summary>
	    /// <param name="minCapacity">The minimum capacity for desired containers</param>
	    /// <param name="maxCapacity">The maximum capacity for desired containers</param>
	    /// <param name="blacklistedContainers">A list of ItemIDs for magazines that will be excluded</param>
	    /// <returns>An FVRObject for an ammo container. Can be null if no container is found</returns>
	    public static FVRObject? GetAmmoContainerForEquipped(int minCapacity = 0, int maxCapacity = 9999, IEnumerable<string>? blacklistedContainers = null)
	    {
		    List<FVRObject> heldItems = GetEquippedFVRObjects().ToList();

		    //Iterpret -1 as having no max capacity
		    if (maxCapacity == -1) maxCapacity = 9999;

		    //Go through and remove any items that have no ammo containers
		    for (int i = heldItems.Count - 1; i >= 0; i--)
		    {
			    if (!FVRObjectHasAmmoContainer(heldItems[i]))
			    {
				    heldItems.RemoveAt(i);
			    }
		    }

		    //Now go through all items that do have ammo containers, and try to get an ammo container for one of them
		    heldItems.Shuffle();
		    return (from item in heldItems select GetCompatibleAmmoContainers(item, minCapacity, maxCapacity, false, blacklistedContainers) into containers where containers.ToList().Count > 0 select containers.ToList().GetRandom()).FirstOrDefault();
	    }
	    
    }
    
}