using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractTextScript : MonoBehaviour
{
    public Text text;

    bool waterDrained;
    // Start is called before the first frame update
    void Start()
    {
        waterDrained = false;
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = "";
    }

    public void Text4PickupWrench()
    {
        text.text = "Press F to pick up the wrench";

    }
    public void EndText4PickupWrench()
    {
        text.text = "";

    }

    public void Text4PickupRevolver()
    {
        text.text = "Press F to pick up the Revolver";
    }

    public void EndText4PickupRevolver()
    {
        text.text = "";
    }


    public void Text4OpenTheDoor()
    {
        text.text = "Press F to open the door";

    }
    public void EndText4OpenTheDoor()
    {
        text.text = "";

    }

    public void Text4DrainWater()
    {

            text.text = "Press F to drain the water";

    }

    public void EndText4DrainWater()
    {
        text.text = "";

    }


}
