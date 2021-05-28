using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBat : AI
{

    public float wanderRadius = 15f;

    public override void PerformAction()
    {
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        undergoingAction = true;
        Debug.Log("Wandering");
        moveTo = RandomNavmeshLocation(wanderRadius);
        agent.SetDestination(moveTo);
        yield return new WaitForEndOfFrame();
        bool path = agent.CalculatePath(moveTo, agent.path);
        if(path && agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError("Invalid path!");
        }
        float timeout = Time.time + actionTimeout;
        yield return new WaitUntil(() => ActionFinished || (Time.time > timeout));
        if(!ActionFinished)
        {
            Debug.Log("Action timed out");
            PerformAction();
        }
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
