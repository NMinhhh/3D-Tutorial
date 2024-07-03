using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public string itemName;

    public string req1;

    public int req1Amount;

    public string req2;

    public int req2Amount;

    public int numOfRequirement;

    public Blueprint(string itemName, int numOfRequirement, string req1, int req1Amount, string req2, int req2Amount)
    {
        this.itemName = itemName;
        this.numOfRequirement = numOfRequirement;
        this.req1 = req1;
        this.req1Amount = req1Amount;
        this.req2 = req2;
        this.req2Amount = req2Amount;
    }
}
