using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Player;
    public GameObject DumAttckSphere;
    Animator animator;
    public AudioSource audioSource;

    public AudioClip wakeSound;
    public AudioClip walkSound;
    public enum Status
    {
        idle,
        standing,
        wakeUp,
        attacking,
        dead,
    }
    public Status status;

    int hp;

    float standingTimeDelta;
    float standingTime;
    float attackTimeDelta;

    bool isAttacked;
    void Start()
    {
        status = Status.idle;
        hp = 100;
        Player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        standingTimeDelta = 0.0f;
        standingTime = 1.0f;
        attackTimeDelta = 0.0f;
        isAttacked = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (status==Status.dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetBool("dead", true);
        }else if (Vector3.Distance(Player.transform.position, transform.position) < 2&&status==Status.idle)
        {
            status = Status.standing;
            animator.SetBool("wake",true);

            
        }else if (status == Status.standing&& standingTimeDelta < standingTime)
        {
            standingTimeDelta += Time.deltaTime;
            audioSource.clip = wakeSound;
            audioSource.Play();
        }
        else if (standingTimeDelta >= standingTime&&status!=Status.dead&&status!=Status.attacking)
        {
            status = Status.wakeUp;
            //audioSource.clip = walkSound;
            //audioSource.Play();
            animator.SetBool("startWalking",true);
            animator.SetBool("attack",false);
        }

        if (status == Status.wakeUp && Vector3.Distance(Player.transform.position, transform.position) < 1.2)
        {
            
            status = Status.attacking;

        }
        
        if (status == Status.attacking)
        {
            animator.SetBool("attack", true);



            attackTimeDelta += Time.deltaTime;
            if (attackTimeDelta >= 1.5 && !isAttacked)
            {
                Vector3 pos = new Vector3(0,1.5f,0);
                GameObject dumAtkBall = Instantiate(DumAttckSphere, gameObject.transform.position+pos, Quaternion.identity);
                DumAttckSphere.GetComponent<DumAtkScript>().Shoot(gameObject.transform.forward * 100);
                isAttacked = true;
            }

            if (attackTimeDelta >= 2)
            {
                status = Status.wakeUp;
                isAttacked = false;
                attackTimeDelta = 0;
            }

        }

    }

    public bool IsWakeUp()
    {
        
            return status == Status.wakeUp;
        
       
    }

    public bool IsDead()
    {
        return status == Status.dead;
    }

    public bool IsStandingUp()
    {
        return status == Status.standing;
    }

    public void SwitchON()
    {
        if (status != Status.dead)
        {
            status = Status.standing;
            animator.SetBool("wake", true);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (status != Status.dead &&status !=Status.idle)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                status = Status.dead;
            }
        }
        
    }
}
