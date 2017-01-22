using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    public float collidingAlpha = 0.08f;
    public float inHandAlpha = 0.15f;

    public GameObject sephigh;
    public GameObject seplow;
    private bool topmovable;
    private bool midmovable;
    private bool botmovable;
    private int ringlvl;

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

        if(collidingObject.tag == "Ring" && collidingObject != objectInHand)
        {
			Vector3 playerPos = GameObject.FindWithTag ("Player").transform.position;

            topmovable = true;
            midmovable = true;
            botmovable = true;

            if (playerPos.y >= sephigh.transform.position.y)
                topmovable = false;
            else if ((playerPos.y < sephigh.transform.position.y) && (playerPos.y < seplow.transform.position.y))
                midmovable = false;
            else if (playerPos.y <= seplow.transform.position.y)
                botmovable = false;

            if (col.transform.position.y >= sephigh.transform.position.y)
                ringlvl = 2;
            else if (col.transform.position.y >= sephigh.transform.position.y)
                ringlvl = 1;
            else if (col.transform.position.y >= sephigh.transform.position.y)
                ringlvl = 0;

            if (topmovable == false && ringlvl == 2)
                return;
            if (midmovable == false && ringlvl == 1)
                return;
            if (botmovable == false && ringlvl == 0)
                return;





            //		Bounds bounds = collidingObject.GetComponent<MeshFilter> ().mesh.bounds;
            //		if(bounds.Contains(playerPos)){//if (bounds.min.y > playerPos.y && bounds.max.y < playerPos.y) {
            //			collidingObject = null;
            //            Debug.Log("Inside" + playerPos.y + "min " + bounds.min.y);
            //			return;
        }
   //         Debug.Log("Outside" + playerPos.y + "min " + bounds.min.y);
            collidingObject.GetComponent<RingInteraction> ().SetTargetAlpha (collidingAlpha);
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
			collidingObject.GetComponent<RingInteraction> ().SetTargetAlpha (0f);
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


			objectInHand.GetComponent<RingInteraction> ().SetTargetAlpha (inHandAlpha);
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
            if(objectInHand == collidingObject)
				objectInHand.GetComponent<RingInteraction> ().SetTargetAlpha (collidingAlpha);
            else
				objectInHand.GetComponent<RingInteraction> ().SetTargetAlpha (0f);

			//objectInHand.transform.parent.gameObject.GetComponent<RingInertia> ().SetInertia (previousAngle);
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
