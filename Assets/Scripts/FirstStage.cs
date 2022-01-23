using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStage : MonoBehaviour
{
    public GameObject firstStage;

    void Start()
    {
        Liftoff.OnSecondStage += Detach;
    }

    void Detach(){
        firstStage.GetComponent<Transform>().SetParent(null);
        StartCoroutine(WaitToAddRigidbody());
    }

    IEnumerator WaitToAddRigidbody()
    {
        yield return new WaitForSeconds(1f);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        //little push downwards
        rb.AddForce(Vector3.down);
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
