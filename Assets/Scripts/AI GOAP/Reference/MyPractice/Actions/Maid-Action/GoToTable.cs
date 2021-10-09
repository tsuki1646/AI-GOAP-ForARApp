using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTable : GAction
{
    public override bool PrePerform()
    {
        // Get a free cubicle
        target = inventory.FindItemWithTag("Table");
        // Check that we did indeed get a cubicle
        if (target == null)
            // No cubicle so return false
            return false;
        // All good
        return true;
    }

    public override bool PostPerform()
    {        
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        // Add a new state "Guided->GuidingCustomer"
        GWorld.Instance.GetWorld().ModifyState("TreatingCustomer", 1);
        // Add isCured to agents beliefs
        //beliefs.ModifyState("atRestaurant", 1);
        // Remove the cubicle from the list
        //inventory.RemoveItem(target);

        // Give back the table
        GWorld.Instance.GetQueue("tables").AddResource(target);
        // Remove the table from the list
        inventory.RemoveItem(target);
        // Give the table back to the world
        GWorld.Instance.GetWorld().ModifyState("FreeTable", 1);

        return true;
    }
}
