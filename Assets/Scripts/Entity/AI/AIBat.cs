using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBat : AI
{

    public float wanderRadius = 5f;

    public override void PerformAction()
    {
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        undergoingAction = true;
        moveTo = RandomNavmeshLocation(wanderRadius);
        agent.SetDestination(moveTo);
        yield return new WaitForEndOfFrame();
        bool path = agent.CalculatePath(moveTo, agent.path);
        if(path && agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            pathIsValid = false;
            PerformAction();
            undergoingAction = false;
            yield break;
        }
        pathIsValid = true;
        float timeout = Time.time + actionTimeout;
        yield return new WaitUntil(() => reached || (Time.time > timeout));
        PerformAction();
        undergoingAction = false;
    }

    IEnumerator Search()
    {
        yield return null;
    }

    IEnumerator Persue()
    {
        yield return null;
    }

    IEnumerator Attack()
    {
        yield return null;
    }

}
