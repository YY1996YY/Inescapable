using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageInWater : MonoBehaviour
{
    float moveTime;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        moveTime = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void LateUpdate()
    {
        moveTime += Time.deltaTime;
        if (moveTime <= 1)
        {
            transform.position += new Vector3(0, 0.001f, 0);
            //rb.position += new Vector3(0, 0.001f, 0);

        }
        if (moveTime >= 1)
        {
            transform.position -= new Vector3(0, 0.001f, 0);
            //rb.position -= new Vector3(0, 0.001f, 0);
            if (moveTime >= 2)
            {
                moveTime = 0;
            }
        }
    }
}
