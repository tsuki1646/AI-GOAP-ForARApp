using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoMyHome : GAction
{
    protected override void Awake()
    {
        base.Awake();
        //isCurrentAction = true;

    }

    public override bool PrePerform()
    {
        beliefs.RemoveState("atRestaurant");

        //isCurrentAction = true;
        

        return true;
    }

    public override bool PostPerform()
    {
        //isCurrentAction = false;

        Destroy(this.gameObject, 1.0f);
        return true;
    }

    
    public override bool PostPerformCleanUp()
    {
        return true;
    }
}
