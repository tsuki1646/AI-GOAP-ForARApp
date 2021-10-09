using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToRestaurant : GAction
{
    //public Customer customer;
    protected override void Awake()
    {
        base.Awake();
        //anim.SetTrigger("isWalking");
        //isCurrentAction = true;
    }

    public override bool PrePerform()
    {

        //anim.SetTrigger("IsWalking");
        //anim.SetTrigger("IsIdle");
        isCurrentAction = true;
        return true;
    }

    public override bool PostPerform()
    {
        //anim.SetTrigger("IsIdle");
        isCurrentAction = false;
        return true;
    }

    
    public override bool PostPerformCleanUp()
    {
        //anim.SetTrigger("IsIdle");
        isCurrentAction = false;
        return true;
    }
}
