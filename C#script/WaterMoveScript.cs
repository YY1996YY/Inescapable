using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    float moveTime;
    private bool IsDrain;
    private bool drainFinished;
    private float drainDeltaTime;
    public GameObject spiderInWater1;
    public GameObject spiderInWater2;
    public GameObject spiderInWater3;
    public GameObject spiderInWater4;
    public GameObject spiderInWater5;
    public GameObject monster;

    private AudioSource audioSource;
    void Start()
    {
        moveTime = 0;
        startPos = transform.position;
        drainFinished = false;
        drainDeltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        


        MoveOnZ(0.01f);
        moveTime += Time.deltaTime;
        if (moveTime > 100)
        {
            startPos.y = transform.position.y;
            transform.position = startPos;
            moveTime = 0;
        }
        if (IsDrain)
        {
            DrainWater();
            drainDeltaTime += Time.deltaTime;
            if (drainDeltaTime>=6)
            {
                drainFinished = true;
            }
        }
        if (drainFinished)
        {
            spiderInWater1.GetComponent<SpiderScript>().TurnOn();
            spiderInWater2.GetComponent<SpiderScript>().TurnOn();
            spiderInWater3.GetComponent<SpiderScript>().TurnOn();
            spiderInWater4.GetComponent<SpiderScript>().TurnOn();
            spiderInWater5.GetComponent<SpiderScript>().TurnOn();
            monster.GetComponent<LizardScript>().SwitchON();
        }

    }

    private void MoveOnZ(float amount)
    {
        transform.position -= transform.forward * amount;
    }

    private void DrainWater()
    {

        this.transform.position -= new Vector3(0, 0.1f, 0) * Time.deltaTime;
    }

    public void DrainStart()
    {
        IsDrain = true;
    }

    

}
