using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchBulletScript : MonoBehaviour
{
    Vector3 startPos = Vector3.zero;
    private int damage;
    float time4Destory;
    // Start is called before the first frame update]
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        time4Destory = 0;
        //birth position
        startPos = gameObject.transform.position;
        damage = 40;
        //avoid collision with player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        time4Destory += Time.deltaTime;
        if (Vector3.Distance(startPos,gameObject.transform.position)>1||time4Destory>0.6)     
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(),GetComponent<Collider>());
        }
        */

        if (collision.gameObject.tag == "Padlock")
        {
            collision.gameObject.GetComponent<PadlockScript>().TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Dummy")
        {
            collision.gameObject.GetComponent<DummyController>().TakeDamage(damage);
        }
        else if (collision.gameObject.tag == "Spider")
        {
            collision.gameObject.GetComponent<SpiderScript>().TakeDamage(damage);
        }
        else if (collision.gameObject.tag == "Lizard")
        {
            collision.gameObject.GetComponent<LizardScript>().TakeDamage(damage);
        }


    }
}
