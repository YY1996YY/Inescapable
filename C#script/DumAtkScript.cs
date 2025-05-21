using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumAtkScript : MonoBehaviour
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
        damage = 20;
        //avoid collision with dummy
        GameObject dummy = GameObject.FindGameObjectWithTag("Dummy");
        Physics.IgnoreCollision(dummy.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        time4Destory += Time.deltaTime;
        if (Vector3.Distance(startPos, gameObject.transform.position) > 2)
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

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControl>().PlayerTakeDamage(damage);
            Debug.Log("shotdum");
            Destroy(gameObject);
        }
        Destroy(gameObject);

    }
}
