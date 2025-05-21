using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LizardNavScript : MonoBehaviour
{ // Start is called before the first frame update
    public Transform player;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<LizardScript>().IsWakeUp())
        {
            Vector3 playerDir = player.position;
            playerDir.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(playerDir);
            agent.destination = player.position;
        }
    }
}
