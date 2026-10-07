using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIControl2 : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;

    float speedMult;
    float detectionRadius = 5;
    float fleeRadius = 10;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        int i = Random.Range(0, goalLocations.Length);

        agent.SetDestination(goalLocations[i].transform.position);

        anim = this.GetComponent<Animator>();

        anim.SetFloat("wOffse", Random.Range(0.0f, 1.0f));

        ResetAgent();
    }

    void ResetAgent()
    {
        speedMult = Random.Range(0.5f, 2);

        anim.SetFloat("speedMult", speedMult);

        agent.speed += speedMult;

        anim.SetTrigger("isWalking");

        agent.angularSpeed = 120;

        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection =
                (this.transform.position - position).normalized;

            Vector3 newgoal =
                this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();

            agent.CalculatePath(newgoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(
                    path.corners[path.corners.Length - 1]
                );

                anim.SetTrigger("isRunning");

                agent.speed = 10;

                agent.angularSpeed = 500;
            }
        }
    }

    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            int i = Random.Range(0, goalLocations.Length);

            agent.SetDestination(
                goalLocations[i].transform.position
            );
        }
    }
}
