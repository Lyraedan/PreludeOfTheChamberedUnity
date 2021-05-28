using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool hostile = false;
    public float actionTimeout = 10f;

    public Vector3 moveTo = Vector3.zero;

    public bool reached = true;
    public bool pathIsValid = false;
    public bool undergoingAction = false;

    private void Start()
    {
        PerformAction();
    }

    private void Update()
    {
        // Calculate the distance. If not assume they reached their destination
        reached = agent.hasPath ? agent.remainingDistance <= 1f : true;
    }

    public abstract void PerformAction();

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnDrawGizmos()
    {
        if (pathIsValid && Application.isPlaying)
        {
            // Draw at destination
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(moveTo, 0.25f);

            // Draw line from entity to destination
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawLine(transform.position, moveTo);

            //Draw path
            var line = GetComponent<LineRenderer>();
            if (line == null)
            {
                line = this.gameObject.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
                line.startWidth = 0.25f;
                line.endWidth = 0.25f;
                line.startColor = Color.yellow;
                line.endColor = Color.yellow;
            }

            var path = agent.path;

            line.positionCount = path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                line.SetPosition(i, path.corners[i]);
            }

        }
    }
}
