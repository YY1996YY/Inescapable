using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DumSwitchON()
    {
        enemy.GetComponent<DummyController>().SwitchON();
    }
}
