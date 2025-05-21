using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardBulletScript : MonoBehaviour
{

    Vector3 startPos = Vector3.zero;
    private int damage;
    float atkDeltaTime;
    bool attacked;
    // Start is called before the first frame update]
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }
    void Start()
    {
        Application.targetFrameRate = 60;
       
        //birth position
        startPos = gameObject.transform.position;
        damage = 35;
        //avoid collision with lizard
        GameObject lizard = GameObject.FindGameObjectWithTag("Lizard");
        //Physics.IgnoreCollision(lizard.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Lizard").GetComponent<LizardScript>().IsAttacking()&&!attacked)
        {
            damage = 35;

        }
        else
        {
            damage = 0;
        }

        if (attacked)
        {
            atkDeltaTime += Time.deltaTime;
            if (atkDeltaTime >= 0.8f)
            {
                attacked = false;
                atkDeltaTime = 0;
            }
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControl>().PlayerTakeDamage(damage);
            Debug.Log("shotLiz");
            attacked = true;
        }
        
    }
}

