using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    List<Rigidbody> RigidbodiesInWindList = new List<Rigidbody>();
    Vector3 windDirection = Vector3.forward;
    public float windStrength = 0.01f;

    private void OnTriggerEnter(Collider col) {
        Rigidbody objRigid = col.gameObject.GetComponent<Rigidbody>();
        if(objRigid != null){
            RigidbodiesInWindList.Add(objRigid);
            Debug.Log("Vento");
        }
    }

    private void OnTriggerExit(Collider col) {
        Rigidbody objRigid = col.gameObject.GetComponent<Rigidbody>();
        if(objRigid != null)
            RigidbodiesInWindList.Remove(objRigid);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(RigidbodiesInWindList.Count > 0){
            foreach(Rigidbody rb in RigidbodiesInWindList){
                rb.AddForce(windDirection * windStrength);
            }
        }
    }
}
