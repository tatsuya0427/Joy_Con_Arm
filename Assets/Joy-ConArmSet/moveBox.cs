using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBox : MonoBehaviour {

    //Collision lastCollision;

    public GameObject targetObject = null;
    private bool hit;

    void Start () {
        this.hit = false;
    }

    void Update () {
        if (hit && targetObject != null) {
            Vector3 vec = targetObject.transform.position;
            transform.position = vec;
        }
    }

    public void grabbedBox (GameObject hand) {
        // hit = true;
        // Debug.Log ("putButton");

        targetObject = hand;
        transform.parent = hand.transform;
        hit = true;
    }

    public void releaseBox () {
        hit = false;
        targetObject = null;
        transform.parent = null;
    }
    //ontriger
    // public void OnTriggerStay (Collider t) {
    //     //Debug.Log ("Hit");
    //     //Debug.Log (t.gameObject.CompareTag ());
    //     //Debug.Log (hit);

    //     string layerName = LayerMask.LayerToName (t.gameObject.layer);
    //     Debug.Log (layerName);

    //     // hit = true;
    //     // this.lastCollision = collision;
    //     //transform.parent = collision.transform;
    //     if (layerName.Equals ("hand") && hit == true) {
    //         targetObject = t.gameObject;
    //     }
    // }

    // public void OnTriggerExit (Collider t) {
    //     Debug.Log ("release");

    // }
}