using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Audio;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;
using System.Globalization;
using TMPro;

public class Liftoff : MonoBehaviour
{
    public Rigidbody rigidBody = null;
    public GameObject firstStage =  null;
    public GameObject target = null;
    // public Rigidbody secondStage =  null;
    public GameObject propulsionFire = null;
    public GameObject propulsionSmoke = null;
    public AudioSource propulsionSound;
    public AudioSource propulsionBlastSound;
    public TextMeshProUGUI initiateLaunchText;
    public TextMeshProUGUI countdownTimerText;


    public float propulsion;
    public float acceleration;
    public int propulsionTimeTotal = 5;
    private float propulsionTime;
    public float detachCountdown = 10;
    public bool launch = false;
    public float rotationSpeed;
    private bool detached = false;
    private bool openParachute = false;
    private float countdownTimer = 5f;
    private bool initiateCountdown = false;

    public delegate void SecondStage();
    public static event SecondStage OnSecondStage;

    private void Start() {
        propulsionTime = propulsionTimeTotal;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        //so that the detach counts its time after the launch countdown
        detachCountdown += propulsionTimeTotal;
    }

    void Update()
    {
        //input to launch the rocket
        if(Input.GetKeyDown(KeyCode.Space) && !launch){
            initiateCountdown = true;
            initiateLaunchText.gameObject.SetActive(false);
            StartCoroutine(Countdown());
            SetAnimations();
        }

        //update the timer UI
        if(initiateCountdown){
            countdownTimer -= Time.deltaTime;
            countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);
            countdownTimerText.text = Mathf.Round(countdownTimer).ToString();
            countdownTimerText.text = string.Format("{0:00.00}", countdownTimer);
        }

        if(countdownTimer <= 0)
            countdownTimerText.gameObject.SetActive(false);

        //option for manually detach the first stage
        if(Input.GetKeyDown(KeyCode.Q)){
            OnSecondStage();
        }

        //adding force to the rocket for 5 seconds
        if(launch && propulsionTime > 0){
            rigidBody.AddForce(Vector3.up * propulsion--);
            rigidBody.AddForce(Vector3.left * acceleration);

            
            //transform.LookAt(target.transform.position - transform.position);

            // Vector3 movementDir = new Vector3(target.transform.position - transform.position, 1, 1);
            // //movementDir.Normalize();
            
            propulsionTime -= Time.deltaTime;
        }
        else if(propulsionTime <= 0){
            propulsionTime = 0;
        }

        if(launch && !detached){
            Quaternion toRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }           

        //countdown for detaching
        if(detachCountdown <= 0 && !detached && launch){
            Debug.Log("Segundo Estágio");
            OnSecondStage();
            propulsionSound.Stop();
            propulsionFire.SetActive(false);
            firstStage.transform.parent = null;
            detached = true;
        }
        else if(detachCountdown > 0 && launch) detachCountdown -= Time.deltaTime;
    }

    IEnumerator Countdown()
    {
        propulsionSound.Play(0);
        yield return new WaitForSeconds(5f);
        propulsionBlastSound.Play(0);
        rigidBody.AddForce(Vector3.up * (propulsion+5));
        launch = true;
        //propulsionFire.SetActive(true);
    }

    //starting the animations
    void SetAnimations(){
        propulsionFire.SetActive(true);
        propulsionSmoke.SetActive(true);
    }

    void SetSounds(){
        propulsionSound.Play(0);
    }
}
