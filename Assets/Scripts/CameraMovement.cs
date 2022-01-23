using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target = null;
    public GameObject player = null;        


    private Vector3 offset;            

    // Use this for initialization
    void Start () 
    {
        if(player != null)
            offset = transform.position - player.transform.position;
    }

    private void Update() {
        
    }

    void LateUpdate () 
    {
        if(player != null)
            transform.position = player.transform.position + offset;

        if(target != null){
            transform.LookAt(target.transform, Vector3.up);
            if(target.GetComponent<Rigidbody>().velocity.y <= 0)
                transform.position += Vector3.up/2;
            if(target.GetComponent<Rigidbody>().velocity.y <= 0 && transform.position.y > 5)
                transform.position -= Vector3.up/2;
        }
    }

}
