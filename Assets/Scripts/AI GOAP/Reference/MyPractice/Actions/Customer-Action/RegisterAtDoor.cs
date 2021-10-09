using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterAtDoor : GAction
{    
    bool isWalking;
    protected override void Awake()
    {
        base.Awake();
    }

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
        return true;
    }
}
