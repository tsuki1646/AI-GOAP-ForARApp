using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : GAction
{
    public override bool PrePerform()
    {
        // Get a free kitchen
        target = GWorld.Instance.GetQueue("kitchens").RemoveResource();
        // Check that we did indeed get an kitchen
        if (target == null)
            // No kitchen so return false
            return false;

        // Add it to the inventory
        inventory.AddItem(target);
        // Make the kitchen unavailable to other doctors
        GWorld.Instance.GetWorld().ModifyState("FreeKitchen", -1);
        //Debug.Log("Cooking Started");
        // All good
        return true;
    }

    public override bool PostPerform()
    {        
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        // Add the kitchen back to the pool
        GWorld.Instance.GetQueue("kitchens").AddResource(target);
        // Remove the kitchen from the list
        inventory.RemoveItem(target);
        // Give the kitchen back to the world
        GWorld.Instance.GetWorld().ModifyState("FreeKitchen", 1);
        //Debug.Log("Cooking Finished");
        // All good

        return true;
    }
}
