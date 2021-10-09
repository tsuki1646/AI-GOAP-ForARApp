using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid : GAgent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatCustomer", 1, false);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 3);

        // Toilet goal
        //SubGoal s3 = new SubGoal("relief", 1, false);
        //goals.Add(s3, 5);

        // Call the GetTired() method for the first time
        Invoke("GetTired", Random.Range(10.0f, 20.0f));
    }

    void GetTired()
    {

        beliefs.ModifyState("exhausted", 0);
        //call the get tired method over and over at random times to make the nurse
        //get tired again
        Invoke("GetTired", Random.Range(10.0f, 20.0f));
        // Call the NeedRelief() methd for the first time
        //Invoke("NeedRelief", Random.Range(10.0f, 20.0f));
    }

    /*
    void NeedRelief()
    {

        beliefs.ModifyState("busting", 0);
        // Call the get NeedRelief method over and over at random times to make the agent
        // go to the loo again
        Invoke("NeedRelief", Random.Range(10.0f, 20.0f));
    }*/
}
