using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isLock;
    public bool isUsed;
    void Start()
    {
        //isLock = true;
        //isUsed = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorUnlock()
    {
        isLock = false;
    }

    public void Open()
    {
        gameObject.transform.Rotate(0, 90, 0);
        isUsed = true;
    }
}
