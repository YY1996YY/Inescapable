using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardScript : MonoBehaviour
{

    // Start is called before the first frame update
    GameObject Player;
    public GameObject LizardAttckSphere;
    Animator animator;
    public enum Status
    {
        idle,

        attack,
        walk,
        run,
        dead,
    }
    public Status status;

    int hp;

    float standingTimeDelta;
    float standingTime;
    float attackTimeDelta;

    bool isAttacked;

    AudioSource audioSource;
    public AudioClip audioClip;
    void Start()
    {
        status = Status.idle;
        hp = 500;
        Player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        standingTimeDelta = 0.0f;
        standingTime = 1.0f;
        attackTimeDelta = 0.0f;
        isAttacked = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (status == Status.dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetBool("dead", true);
            audioSource.Pause();
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) < 2 && status == Status.idle)
        {
            audioSource.Play();
            animator.SetBool("wake", true);

        }
        
        else if (standingTimeDelta >= standingTime && status != Status.dead && (status != Status.attack))
        {
            status = Status.walk;
            animator.SetBool("walk", true);
            animator.SetBool("attack", false);
            
        }

        if ((status == Status.walk||status ==Status.run ) && Vector3.Distance(Player.transform.position, transform.position) < 1.2)
        {

            status = Status.attack;
            audioSource.Play();

        }

        if (status == Status.attack)
        {
            animator.SetBool("attack", true);



            attackTimeDelta += Time.deltaTime;
            if (attackTimeDelta >= 2 && !isAttacked)
            {
                Vector3 pos = new Vector3(0, 1.5f, 0);
                GameObject lizardAtkBall = Instantiate(LizardAttckSphere, gameObject.transform.position + pos, Quaternion.identity);
                LizardAttckSphere.GetComponent<LizardBulletScript>().Shoot(gameObject.transform.forward * 50);
                isAttacked = true;
            }

            if (attackTimeDelta >= 4)
            {
                status = Status.walk;
                isAttacked = false;
                attackTimeDelta = 0;
            }

        }

    }

    public bool IsWakeUp()
    {

        return status == Status.walk;
        audioSource.Play() ;


    }

    public bool IsDead()
    {
        return status == Status.dead;
    }



    public void SwitchON()
    {
        if (status != Status.dead)
        {
            status = Status.walk;
            animator.SetBool("walk", true);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (status != Status.dead && status != Status.idle)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                status = Status.dead;
                animator.SetBool("dead", true);
            }
        }

    }

    public bool IsAttacking()
    {
        return status == Status.attack;
    }
}

