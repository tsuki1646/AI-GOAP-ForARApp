using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestForAWhile : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        //the agent will no longer believe they need a rest
        beliefs.RemoveState("exhausted");
        return true;
    }
}
