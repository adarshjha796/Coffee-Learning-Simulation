using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] private GameObject holdObject;
    private Rigidbody holdRigidbody;
    private GameObject tempHoldObject;

    public int counter;


    [Header("Physics Parameters")]

    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    [SerializeField] private RectTransform crosshair;
    private enum CrosshairHandle
    {
        INCREASE,
        DECREASE
    }

    private CrosshairHandle crosshairStatus;

    public int id;



    public bool isSelected;

    private void Start()
    {
        
    }


    private void Update()
    {
        GetObjectToPick();
    }

    private void GetObjectToPick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (holdObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    ObjectInteraction _interactionObject = hit.transform.gameObject.GetComponent<ObjectInteraction>();
                    
                    if(_interactionObject && _interactionObject.interactionType == ObjectInteraction.Interaction_Type.HOLD)
                        PickupObject(hit.transform.gameObject);
                    else if(_interactionObject && _interactionObject.interactionType == ObjectInteraction.Interaction_Type.ANIMATION)
                        _interactionObject.InteractAnimationObject();
                    
                }
            }
            else
            {
                DropObject();
            }
        }
        if (holdObject != null)
        {
            MoveObject();
        }

        RaycastHit hit1;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit1, pickupRange))
        {
            ObjectInteraction _interactionObject = hit1.transform.gameObject.GetComponent<ObjectInteraction>();

            if(_interactionObject)
            {
                HandleCrosshair(CrosshairHandle.INCREASE);
            }
            else
            {
                if(holdArea != null)
                    HandleCrosshair(CrosshairHandle.DECREASE);
            }
        }
    }

    private void PickupObject(GameObject objectToPick)
    {
        tempHoldObject = objectToPick;
        holdRigidbody = objectToPick.GetComponent<Rigidbody>();
        holdRigidbody.useGravity = false;
        holdRigidbody.isKinematic = true;
        holdRigidbody.drag = 10;
        //holdObject.layer = 6;        
        tempHoldObject.layer = LayerMask.NameToLayer("PickedUpObject");
        //holdRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        holdRigidbody.transform.parent = holdArea;
        holdObject = objectToPick;
        isSelected = true;
        counter = 1;
        objectToPick.GetComponent<ObjectInteraction>().UpdateStatus();
    }

    private void DropObject()
    {
        isSelected = false;
        holdRigidbody.useGravity = true;
        holdRigidbody.isKinematic = false;

        holdRigidbody.drag = 1;
        holdObject.transform.parent = null;
        holdObject = null;
        counter = 0;
        tempHoldObject.layer = LayerMask.NameToLayer("Default");
    }

    void MoveObject()
    {
        if (Vector3.Distance(holdObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 move_position = (holdArea.position - holdObject.transform.position);
            holdRigidbody.AddForce(move_position * pickupForce);
        }
    }

    private void HandleCrosshair(CrosshairHandle status)
    {
        if(status == CrosshairHandle.INCREASE)
        {
            if(crosshair.localScale.x < 2)
                crosshair.localScale += new Vector3(0.1f , 0.1f , 0.1f);
        }
        if(status == CrosshairHandle.DECREASE)
        {
            if(crosshair.localScale.x > 1)
                crosshair.localScale -= new Vector3(0.2f , 0.2f , 0.2f);
        }
    }
}
