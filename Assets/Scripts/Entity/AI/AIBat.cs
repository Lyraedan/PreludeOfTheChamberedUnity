using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBat : AI
{
    public override void OnAttack()
    {

    }

    public override void PerformAction()
    {
        StartCoroutine(Wander());
    }

}
