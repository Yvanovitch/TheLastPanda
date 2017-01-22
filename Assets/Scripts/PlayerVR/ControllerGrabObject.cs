using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    public float collidingAlpha = 0.1f;
    public float inHandAlpha = 0.15f;

    private SteamVR_TrackedObject trackedObj;

    // Stores the GameObject that the trigger is currently colliding with
    private GameObject collidingObject;
    // Serves as a reference to the GameObject that the player is currently grabbing
    private GameObject objectInHand;

    private Quaternion initialRingRotation;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;

        if(collidingObject.tag == "Ring")
        {
            Material material = collidingObject.GetComponent<Renderer>().material;
            Color color = material.color;
            color.a = collidingAlpha;
            material.color = color;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        if (collidingObject.tag == "Ring" && collidingObject != objectInHand)
        {
            Material material = collidingObject.GetComponent<Renderer>().material;
            Color color = material.color;
            color.a = 0f;
            material.color = color;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

        if (objectInHand.tag == "Ring")
        {
            GameObject parent = objectInHand.transform.parent.gameObject;

            Vector3 initialRingRotationTemp = parent.transform.rotation.eulerAngles;
            initialRingRotationTemp.x = initialRingRotationTemp.z = 0;
            initialRingRotation = Quaternion.Euler(initialRingRotationTemp);
            Vector3 initialLookAtRotationTemp = Quaternion.LookRotation(this.transform.position - parent.transform.position).eulerAngles;
            initialLookAtRotationTemp.x = initialLookAtRotationTemp.z = 0;
            Quaternion initialLookAtRotation = Quaternion.Euler(initialLookAtRotationTemp);
            initialRingRotation = Quaternion.Inverse(initialLookAtRotation) * initialRingRotation;

            Material material = objectInHand.GetComponent<Renderer>().material;
            Color color = material.color;
            color.a = inHandAlpha;
            material.color = color;
        }
            /*Renderer rend = objectInHand.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = newMaterialRef;
            }
            Debug.Log("objectInHand: " + objectInHand.name);*/
        }


    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }

        if (objectInHand.tag == "Ring")
        {
            Material material = objectInHand.GetComponent<Renderer>().material;
            Color color = material.color;
            if(objectInHand == collidingObject)
                color.a = collidingAlpha;
            else
                color.a = 0;
            material.color = color;
        }

         objectInHand = null;
    }

    // Update is called once per frame
    void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        if(objectInHand != null)
        {
            if (objectInHand.tag == "Ring")
            {
                GameObject parent = objectInHand.transform.parent.gameObject;
                Vector3 newRotation = Quaternion.LookRotation(this.transform.position - parent.transform.position).eulerAngles;
                newRotation.x = newRotation.z = 0;
                Quaternion newRotationQuat = Quaternion.Euler(newRotation);
                newRotationQuat *= initialRingRotation;
                
                parent.transform.rotation = newRotationQuat;
            }
        }
        

        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }
}
