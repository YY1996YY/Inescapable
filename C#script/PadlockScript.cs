using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockScript : MonoBehaviour
{
    private int hp = 0;
    Vector3 startPos = Vector3.zero;
    Quaternion startRot;
    float gravityTime = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        
        hp = 1;
        
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp == 0)
        {

            Debug.Log("padlock hitted");
            
            
            //Destroy(gameObject);
        }
        else
        {
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = startRot;
        }
    }

    public void TakeDamage(int dmg)
    {
        hp = 0;
        Unlock();
    }

    private void Unlock()
    {
        //GameObject.Find("Upper_part_LOD0").transform.Rotate(new Vector3(0, 180, 0));
        transform.Find("Upper_part_LOD0").transform.Rotate(new Vector3(0, 180, 0));
        gameObject.transform.parent.transform.GetComponentInChildren<LockedDoorScript>().DoorUnlock();

    }
}
