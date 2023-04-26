using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour {

    public GameObject targetObject;
    private Rigidbody targetObjectRB;
    private bool hit;
    private bool grapping;
    string layerName;
    moveBox box;

    void Start () {
        targetObject = null;
        hit = false;
        grapping = false;
        layerName = null;
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnTriggerStay (Collider collider) {
        layerName = LayerMask.LayerToName (collider.gameObject.layer);
        if (layerName.Equals ("box") && grapping == true) {
            targetObject = collider.gameObject;

            targetObject.transform.parent = this.gameObject.transform;
            targetObjectRB = targetObject.GetComponent<Rigidbody> ();
            targetObjectRB.isKinematic = true;
            // box = targetObject.gameObject.GetComponent<moveBox> ();

            // box.grabbedBox (gameObject);
        }
    }

    public void OnTriggerExit (Collider collider) {

    }

    public void grap () {
        grapping = true;
    }

    public void release (Vector3 armAccel) {
        if (targetObject != null) {
            //box.releaseBox ();
            grapping = false;
            targetObject.transform.parent = null;
            targetObjectRB.isKinematic = false;
            targetObjectRB.AddForce (armAccel * 10, ForceMode.Impulse);
            targetObject = null;
        }
    }
}