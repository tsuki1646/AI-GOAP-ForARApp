using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToRestaurant_Test : GAction
{
    bool isWalking;
    bool isIdle;
    protected override void Awake()
    {
        base.Awake();
        //anim.SetTrigger("isWalking");
        isCurrentAction = false;
        //anim.SetBool("Idle", true);
    }

    public override bool PrePerform()
    {
        isCurrentAction = true;
        /*
        if (distance > 0)
        {
            agent.isStopped = false;
            anim.SetFloat("wOffset", Random.Range(0, 1f));
            anim.SetTrigger("isWalking");
            float sm = Random.Range(0.1f, 0.5f);
            anim.SetFloat("speedMult", sm);
            agent.speed *= sm;
        }
        else
        {
            agent.isStopped = true;
            anim.SetTrigger("isIdle");
        }*/
        agent.isStopped = false;
        anim.SetFloat("wOffset", Random.Range(0, 1f));
        //anim.SetTrigger("isWalking");
        anim.SetBool("Walking", true);
        float sm = Random.Range(0.1f, 0.5f);
        anim.SetFloat("speedMult", sm);
        agent.speed *= sm;

        return true;
    }

    public override bool PostPerform()
    {
        isCurrentAction = false;
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        return true;
    }
}
