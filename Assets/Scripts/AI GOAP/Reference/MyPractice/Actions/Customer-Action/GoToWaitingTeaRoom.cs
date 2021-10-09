using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingTeaRoom : GAction
{    
    protected override void Awake()
    {
        base.Awake();
        //isCurrentAction = true;
        //anim.SetBool("Idle", true);
    }

    public override bool PrePerform()
    {
        //isCurrentAction = true;

        return true;
    }

    public override bool PostPerform()
    {        
        return true;
    }

    
    public override bool PostPerformCleanUp()
    {
        // Inject waiting state to world states
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);

        // Patient adds himself to the queue
        GWorld.Instance.GetQueue("customers").AddResource(this.gameObject);
        // Inject a state into the agents beliefs
        beliefs.ModifyState("atRestaurant", 1);

        return true;
    }
}
