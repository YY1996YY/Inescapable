using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class SpiderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private NavMeshAgent agent;
    private bool isOn;
    private bool isJumped;
    private float jumpDeltaTime;
    Animator animator;
    Rigidbody rb;
    private int hp;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isOn = false;
        isJumped = false;
        hp = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp == 0)
        {
            animator.SetBool("dead", true);
        }
        else
        {


            if (isOn)
            {
                Vector3 playerDir = player.position;
                playerDir.y = gameObject.transform.position.y;
                gameObject.transform.LookAt(playerDir);
                agent.destination = player.position;
            }

            if (Vector3.Distance(player.transform.position, this.transform.position) < 2)
            {
                animator.SetBool("jump", true);
                //rb.velocity= new Vector3(0, 1000, 0);
                //agent.velocity = new Vector3(0, 10, 0);
                this.GetComponent<NavMeshAgent>().enabled = false;
                if (!isJumped)
                {
                    rb.velocity = transform.up * 6 + transform.forward * 2;
                    //rb.velocity = transform.forward * 2;
                    isJumped = true;
                }
            }

            if (isJumped)
            {
                jumpDeltaTime += Time.deltaTime;
                if (jumpDeltaTime >= 0.8)
                {
                    if (Vector3.Distance(player.transform.position, this.transform.position) <= 2)
                    {
                        GameObject.Find("Player").GetComponent<PlayerControl>().SpiderOn();
                        Destroy(gameObject);
                    }
                    else
                    {

                        animator.SetBool("jump", false);
                        this.GetComponent<NavMeshAgent>().enabled = true;
                        isJumped = false;
                        jumpDeltaTime = 0;
                    }





                }

            }
        }

        
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void TakeDamage(int dmg)
    {
        hp = hp - dmg;
        if (hp < 0)
        {
            hp = 0;
        }
    }
}
