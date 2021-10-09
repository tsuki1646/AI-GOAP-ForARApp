using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGuidedToTable : GAction
{
    protected override void Awake()
    {
        base.Awake();
        //isCurrentAction = true;
        //anim.SetBool("Idle", true);
    }

    public override bool PrePerform()
    {
        // Get a free cubicle
        target = inventory.FindItemWithTag("Table");

        //isCurrentAction = true;

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
        // Add a new state "Treated"
        GWorld.Instance.GetWorld().ModifyState("Guided", 1);

        // Add isCured to agents beliefs
        //beliefs.ModifyState("atRestaurant", 1);
        beliefs.ModifyState("isFull", 1);
        // Remove the cubicle from the list
        inventory.RemoveItem(target);

        return true;
    }
}
