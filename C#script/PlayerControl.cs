using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    private int hp;
    private float spiderDeltaTime;
    public Text hpText;
    public Text revolverAmmoText;
    bool isSpiderOn;
    private bool haveWrench;
    private bool haveRevolver;


    private int revolverAmmoLoaded;
    private int revolverAmmoStorage;

    Vector3 spiderOrgPos;


    public AudioClip waterSound;
    private AudioSource waterAudioSource;

    void Start()
    {
        hp = 100;
        isSpiderOn = false;

        GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.SetActive(true);
        spiderOrgPos = GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.transform.localPosition;
        GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.SetActive(false);
        waterAudioSource = GetComponent<AudioSource>();
        waterAudioSource.clip = waterSound;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        hpText.text = "HP:" + hp;
        if (GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.activeSelf == true)
        {
            revolverAmmoText.text = revolverAmmoLoaded + " | " + revolverAmmoStorage;
        }
        else
        {
            revolverAmmoText.text = "";
        }


        SpiderCheck();


        //switch weapon
        if (Input.GetKeyDown("q"))
        {
            if (GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.activeSelf == true && haveRevolver)
            {
                GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.SetActive(true);
                GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.SetActive(false);
                gameObject.GetComponentInChildren<FPSController>().weaponStatus = FPSController.WeaponStatus.Revovler;
            }
            else if (GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.activeSelf == true && haveWrench)
            {
                GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.SetActive(false);
                GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.SetActive(true);
                gameObject.GetComponentInChildren<FPSController>().weaponStatus = FPSController.WeaponStatus.Wrench;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ClearManager")
        {
            SceneManager.LoadScene("EndScene");
        }
        if (other.tag == "Water")
        {
            waterAudioSource.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {






        //In water
        if (other.tag == "Water")
        {
            //gameObject.GetComponent<FPSController>().InWater();
            gameObject.GetComponentInChildren<FPSController>().InWater();
           // waterAudioSource.Play();
        }




        ////pick up wrench
        if (other.tag == "Wrench4Pickup")
        {
            Debug.Log("you can pick up the wrench");
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().Text4PickupWrench();
            if (Input.GetKeyDown("f"))
            {
                haveWrench = true;
                GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.SetActive(true);
                gameObject.GetComponentInChildren<FPSController>().weaponStatus = FPSController.WeaponStatus.Wrench;

                Destroy(other.gameObject);
                GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupWrench();
            }

        }


        ///pickup revolver
        if (other.tag == "Revolver4Pickup")
        {
            Debug.Log("you can pick up the revolver");
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().Text4PickupRevolver();
            if (Input.GetKeyDown("f"))
            {
                haveRevolver = true;
                GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.SetActive(true);
                GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.SetActive(false);
                gameObject.GetComponentInChildren<FPSController>().weaponStatus = FPSController.WeaponStatus.Revovler;
                revolverAmmoLoaded = 6;
                revolverAmmoStorage = 24;

                Destroy(other.gameObject);
                GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupRevolver();
            }

        }

        //open the door
        if (other.tag == "Door")
        {
            if (!other.GetComponent<LockedDoorScript>().isLock && !other.GetComponent<LockedDoorScript>().isUsed)
            {
                Debug.Log("you can open the door");
                GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().Text4OpenTheDoor();
                if (Input.GetKeyDown("f"))
                {
                    other.GetComponent<LockedDoorScript>().Open();

                    GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4OpenTheDoor();
                }
            }

        }


        //enemy switch
        if (other.tag == "DumSwitch")
        {
            other.GetComponent<DumSwitch>().DumSwitchON();

        }

        //Drain Water
        if (other.tag == "WaterValve")
        {

            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().Text4DrainWater();
            if (Input.GetKeyDown("f"))
            {
                GameObject.Find("WaterCube").GetComponent<WaterMoveScript>().DrainStart();
                GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4DrainWater();
            }
        }


        //switch weapon
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wrench4Pickup")
        {
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupWrench();
            GameObject.Find("Wrench_P").SetActive(true);

        }

        if (other.tag == "Revolver4Pickup")
        {
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupRevolver();
            GameObject.Find("Revolver_P").SetActive(true);

        }




        if (other.tag == "Door")
        {
            //if (other.GetComponent<LockedDoorScript>().isLock) {
            //    GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4OpenTheDoor();
            //}
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4OpenTheDoor();

        }

        if (other.tag == "Water")
        {
            gameObject.GetComponentInChildren<FPSController>().NotInWater();
            waterAudioSource.Pause();
        }

        if (other.tag == "WaterValve")
        {
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4DrainWater();
        }

    }

    public void PlayerTakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 0)
        {
            hp = 0;
        }
    }

    public void SpiderOn()
    {
        GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.SetActive(true);
        //spiderOrgPos = GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.transform.localPosition;
        GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.transform.localPosition = spiderOrgPos;
        isSpiderOn = true;

    }

    private void SpiderCheck()
    {
        if (isSpiderOn) {
            spiderDeltaTime += Time.deltaTime;
            

        }
        if (spiderDeltaTime >= 3.5f)
        {
            GameObject.Find("Main Camera").transform.Find("Spider_P").transform.position += new Vector3(0, 0.2f, 0) * Time.deltaTime;
        }
        if (spiderDeltaTime >= 5)
        {
            isSpiderOn = false;
            spiderDeltaTime = 0;
            GameObject.Find("Main Camera").transform.Find("Spider_P").gameObject.SetActive(false);

        }

    }


    public void RevolverAmmoUsed()
    {
        if (revolverAmmoLoaded > 0)
        {
            revolverAmmoLoaded--;
        }
    }

    public void RevolverReload()
    {
        if (revolverAmmoStorage + revolverAmmoLoaded >= 6)
        {
            revolverAmmoStorage -= (6 - revolverAmmoLoaded);
            revolverAmmoLoaded = 6;
        }
        else
        {
            revolverAmmoLoaded += revolverAmmoStorage;
            revolverAmmoStorage = 0;
        }
    }

    public bool RevolverLoadedAmmoCheck()
    {
      return revolverAmmoLoaded > 0;
    }

    public bool RevolverStorageAmmoCheck()
    {
        return revolverAmmoStorage > 0;
    }


}
