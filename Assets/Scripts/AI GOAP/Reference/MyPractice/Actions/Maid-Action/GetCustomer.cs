using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCustomer : GAction
{
    // Resource in this case = table
    GameObject resource;

    public override bool PrePerform()
    {
        // Set our target customer and remove them from the Queue
        target = GWorld.Instance.GetQueue("customers").RemoveResource();
        // Check that we did indeed get a customer
        if (target == null)
            // No customer so return false
            return false;
        // Grab a free table and remove it from the list
        resource = GWorld.Instance.GetQueue("tables").RemoveResource();
        // Test did we get one?
        if (resource != null)
        {
            // Yes we have a table
            inventory.AddItem(resource);
        }
        else
        {
            // No free tables so release the customer
            GWorld.Instance.GetQueue("customers").AddResource(target);
            target = null;
            return false;
        }

        //take away one cubicle being available from the world state
        GWorld.Instance.GetWorld().ModifyState("FreeTable", -1);
        return true;
    }

    public override bool PostPerform()
    {        
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        // Remove a patient from the world
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        if (target)
        {
            target.GetComponent<GAgent>().inventory.AddItem(resource);
        }

        return true;
    }
}
