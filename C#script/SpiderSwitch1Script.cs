using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSwitch1Script : MonoBehaviour
{
    public GameObject spider1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door"||other.tag=="Player")
        {
            spider1.GetComponent<SpiderScript>().TurnOn();
            Debug.Log("ssssss");
        }
        Debug.Log("pppp");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Door")
        {
            spider1.GetComponent<SpiderScript>().TurnOn();
        }
    }
}
