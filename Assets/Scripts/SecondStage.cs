using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection.Emit;
using UnityEngine;
using TMPro;

public class SecondStage : MonoBehaviour
{
    public Rigidbody rigidBody = null;
    public GameObject parachute = null;
    public GameObject target = null;
    public GameObject propulsionFire = null;
    public AudioSource propulsionSound = null;
    public TextMeshProUGUI maxHeightText = null;

    public float propulsion = 15f;
    public float rotationSpeed = 10f;
    public int propulsionTotal = 5;
    private float propulsionTime;
    public float parachuteDrag;
    public float maxHeight;
    private bool launch = false;
    public bool openParachute = false;
    public bool landed = false;

    void Start()
    {
        Liftoff.OnSecondStage += Propel;
        propulsionTime = propulsionTotal;
    }

    
    void Update()
    {
        if(launch && propulsionTime > 0){
            rigidBody.AddForce(Vector3.up * propulsion);

            //rotate to face downwards
            // if (rigidBody.velocity.y <= 0)
            //     transform.LookAt(transform.position + rigidBody.velocity);            
            
            propulsionTime -= Time.deltaTime;
        }
        else if(propulsionTime <= 0){
            propulsionFire.SetActive(false);
            propulsionSound.Stop();
        }

        //cheking the moment when the rocket stop accelerating and opening the parachute 
        if(rigidBody.velocity.y < 0 && transform.position.y > 10 && !openParachute){
            OpenParachute();
            Debug.Log("Altura Máxima");
            maxHeight = transform.position.y;
            maxHeightText.text = "Altura Máxima: " + maxHeight;
        }

        if(openParachute && !landed){
            Quaternion toRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if(launch && transform.position.y <= 1 && !landed){
            Release();
            landed = true;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<TerrainCollider>()){
            Debug.Log("Pousou");
            Release();
        }
    }

    void Propel(){
        launch = true;
        propulsionFire.SetActive(true);
        propulsionSound.Play(0);
    }

    void OpenParachute(){
        //maxHeight = transform.position.y;
        Debug.Log("Abrindo Paraquedas");
        rigidBody.drag = parachuteDrag;
        //parachute.transform.SetParent()
        parachute.GetComponent<MeshRenderer>().enabled = true;
        openParachute = true;
    }

    void Release(){
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        transform.DetachChildren();
        rigidBody.transform.DetachChildren();
        parachute.gameObject.AddComponent<Rigidbody>();
        parachute.gameObject.AddComponent<SphereCollider>();
    }

}
