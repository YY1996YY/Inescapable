using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Player;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] CharacterController controller;

    private Vector3 cameraDirection = Vector3.zero;
    private float jumpForce = 3f;
    private Vector3 moveDirection = Vector3.zero;
    //private Rigidbody rb;
    private Vector2 angle;
    private Vector2 input;
    private float maxSpeed;
    private float waterSpeed;
    private bool IsInWater;
    private bool wrenchPosRecorded;
    private bool wrenchCanAttack;
    private bool isFired;
    private bool recoilStart;
    
    float wrenchDeltaTime;
    float fireDeltaTime;
    float revolverRecoilTime;
    float reloadingDeltaTime;

    bool wrenchAct;
    bool isReloading;

    public GameObject wrenchBulletPrefab;
    public GameObject revolverBulletPrefab;
    public GameObject revolverHammer;
    Vector3 wrenchOrgPos;
    Quaternion wrenchOrgRot;
    Vector3 revolverOrgPos;

    public AudioClip gunshot;
    public AudioClip wrenchSound;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    public enum WeaponStatus
    {
        None,
        Wrench,
        Revovler,
    }

    public WeaponStatus weaponStatus;
    private void Start()
    {
        Application.targetFrameRate = 60;
        //controller = GetComponent<CharacterController>();
        controller = GetComponentInParent<CharacterController>();
        speed = 0.07f;
       
        
        waterSpeed = 1.0f;
        angle = input = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsInWater = false;
        ///
        //GameObject.Find("Wrench_P").SetActive(true);

        wrenchDeltaTime = 0;
        wrenchAct = false;
        wrenchPosRecorded = false;
        wrenchCanAttack = true;

        float hummerAngle;
        isFired = false;
        recoilStart = false;
        isReloading = false;
        GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.SetActive(true);
        revolverOrgPos = GameObject.Find("Revolver_P").transform.localPosition;
        GameObject.Find("Main Camera").transform.Find("Revolver_P").gameObject.SetActive(false);



        audioSource1 = gameObject.GetComponent<AudioSource>();
        audioSource2 = gameObject.GetComponent<AudioSource>();
        audioSource1.clip = gunshot;
        audioSource2.clip = wrenchSound;

    }

    // Update is called once per frame
    private void Update()
    {
        EndGame();

        IsWrenchAttacking();

        IsRevolverAttacking();




        /////////////////////////////////////////////////
        /////player move control    //DO NOT CHANGE!!!/////
        /////////////////////////////////////////////////
        if (controller.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        angle += new Vector2(Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"))*sensitivity;
        angle = new Vector2(Mathf.Clamp(angle.x,-86f,85f),angle.y);

        transform.eulerAngles = new Vector3(-angle.x,angle.y,0);

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection.y -= gravity*Time.deltaTime;

        cameraDirection = Vector3.Scale(transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        
        Vector3 Forward = cameraDirection * input.y + transform.right * input.x;

        Vector3 moveVelocity = new Vector3(0f, moveDirection.y*Time.deltaTime, 0f);
        moveVelocity += Forward * speed * waterSpeed; //water
        controller.Move(moveVelocity);
        moveVelocity = Vector3.zero;
        if (controller.isGrounded) { moveDirection.y = 0; };
        /////////////////////////////////
        /////player move control END//////
        ///////////////////////////////
        
        

        ////Attack///
        if (Input.GetMouseButtonDown(0))
        {
            //Wrench
            if (weaponStatus == WeaponStatus.Wrench)
            {
                
                if (GameObject.Find("Wrench_P").activeSelf)
                {
                    
                    wrenchAct = true;
                    
                    //WrenchAttack();

                }
            }
            if (weaponStatus == WeaponStatus.Revovler)
            {
                if (GameObject.Find("Revolver_P").activeSelf)
                {
                    //Debug.Log("shot!!");
                    //Vector3 cam = Camera.main.transform.position;
                    //GameObject revolverBullet = Instantiate(revolverBulletPrefab, cam, Quaternion.identity);
                    //revolverBullet.GetComponent<RevolverBulletScript>().Shoot(Camera.main.transform.forward * 100);
                }
            }


            //gun


        }
        if (Input.GetMouseButton(0)&&!isFired&&!isReloading)
        {
            if (GameObject.Find("Revolver_P").activeSelf)
            {
                fireDeltaTime += Time.deltaTime;

                revolverHammer = GameObject.Find("Revolver_P").transform.Find("Hammer").gameObject;
                revolverHammer.transform.Rotate(-2, 0, 0);
                if (fireDeltaTime >= 0.5f)
                {
                    Debug.Log("shot!!");
                    
                    
                    
                    GameObject.Find("Revolver_P").transform.localPosition = revolverOrgPos;

                    if (GameObject.Find("Player").GetComponent<PlayerControl>().RevolverLoadedAmmoCheck())
                    {
                        recoilStart = true;
                        Vector3 cam = Camera.main.transform.position;
                        GameObject revolverBullet = Instantiate(revolverBulletPrefab, cam, Quaternion.identity);
                        revolverBullet.GetComponent<RevolverBulletScript>().Shoot(Camera.main.transform.forward * 100);
                        audioSource1.clip = gunshot;
                        audioSource1.Play();


                        GameObject.Find("Player").GetComponent<PlayerControl>().RevolverAmmoUsed();
                    }

                    fireDeltaTime = 0;
                    revolverHammer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    isFired = true;

                    


                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fireDeltaTime = 0;
            revolverHammer.transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFired = false;
        }

        //reload
        if (Input.GetKeyDown("r")&&GameObject.Find("Player").GetComponent<PlayerControl>().RevolverStorageAmmoCheck())
        {
            isReloading = true;
        }

        if (isReloading)
        {
            reloadingDeltaTime += Time.deltaTime;
            if (reloadingDeltaTime <= 1)
            {
                GameObject.Find("Revolver_P").transform.localPosition -= new Vector3(0, 0.2f * Time.deltaTime, 0);
            }
            else if (reloadingDeltaTime >1&&reloadingDeltaTime<2)
            {
                GameObject.Find("Revolver_P").transform.localPosition -= new Vector3(0, -0.2f * Time.deltaTime, 0);
            }
            else if(reloadingDeltaTime >= 2)
            {
                GameObject.Find("Player").GetComponent<PlayerControl>().RevolverReload();

                isReloading = false;
                reloadingDeltaTime = 0;
                GameObject.Find("Revolver_P").transform.localPosition = revolverOrgPos;
            }
        }
        


        if (recoilStart)
        {
            GameObject.Find("Revolver_P").transform.localPosition -= new Vector3(0, 0, 0.2f * Time.deltaTime);
            revolverRecoilTime += Time.deltaTime;
            if (revolverRecoilTime >= 0.1f)
            {
                revolverRecoilTime = 0;
                GameObject.Find("Revolver_P").transform.localPosition = revolverOrgPos;
                recoilStart =false;
            }
        }
        else if (!recoilStart&&!isReloading)
        {
            GameObject.Find("Revolver_P").transform.localPosition = revolverOrgPos;
        }



    }

    private void LateUpdate()
    {
        
    }
    ///for pick up item,This part of job is now handled by script attached on Player(parent of Main Camera)
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Wrench4Pickup")
        {
            Debug.Log("you can pick up the wrench");
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().Text4PickupWrench();
            if (Input.GetKeyDown("f"))
            {
                GameObject.Find("Main Camera").transform.Find("Wrench_P").gameObject.SetActive(true);
                Destroy(other.gameObject);
                GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupWrench();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wrench4Pickup")
        {
            GameObject.Find("InteractTextController").GetComponent<InteractTextScript>().EndText4PickupWrench();
            GameObject.Find("Wrench_P").SetActive(true);
        }
    }
    */
    private void WrenchAttack()
    {
        Vector3 cam = Camera.main.transform.position;
        GameObject wrenchBullet = Instantiate(wrenchBulletPrefab, cam, Quaternion.identity);
        wrenchBullet.GetComponent<WrenchBulletScript>().Shoot(Camera.main.transform.forward * 1000);
        audioSource2.clip = wrenchSound;
        audioSource2.Play();
    }

    private void IsWrenchAttacking()
    {
        if (wrenchAct)
        {
            if (wrenchPosRecorded == false)
            {

                wrenchOrgPos = GameObject.Find("Wrench_P").transform.localPosition;
                wrenchOrgRot = GameObject.Find("Wrench_P").transform.localRotation;

                wrenchPosRecorded = true;
            }
            wrenchDeltaTime += Time.deltaTime;
            if (wrenchDeltaTime < 0.2f)
            {
                if (wrenchCanAttack)
                {
                    WrenchAttack();
                    wrenchCanAttack = false;
                }
                GameObject.Find("Wrench_P").transform.Rotate(0, 0, 7);
                GameObject.Find("Wrench_P").transform.Translate(-0.04f, 0.03f, 0);

            }
            else if (wrenchDeltaTime >= 0.2f && wrenchDeltaTime < 0.4f)
            {

                GameObject.Find("Wrench_P").transform.Rotate(0, 0, -7);
                GameObject.Find("Wrench_P").transform.Translate(0.04f, -0.03f, 0);
            }
            else if (wrenchDeltaTime >= 0.4f)
            {
                wrenchDeltaTime = 0;
                wrenchAct = false;
                GameObject.Find("Wrench_P").transform.localPosition = wrenchOrgPos;
                GameObject.Find("Wrench_P").transform.localRotation = wrenchOrgRot;
                wrenchPosRecorded = false;
                wrenchCanAttack = true;

            }
        }
    }

    private void IsRevolverAttacking()
    {

    }


    public void InWater()
    {
        waterSpeed = 0.3f;
    }

    public void NotInWater()
    {
        waterSpeed = 1.0f;
    }

    private void EndGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
//#else
//    Application.Quit();//ゲームプレイ終了
//#endif
        }

    }




}
