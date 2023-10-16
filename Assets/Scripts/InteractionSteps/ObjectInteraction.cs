using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public int id;
    private AudioSource audioSource;

    public enum Interaction_Type
    {
        HOLD,
        TRIGGER,
        ANIMATION,
        //BUTTON
    }

    public enum Interaction_Status
    {
        NONE,
        STARTED,
        COMPLETED
    }

    public Interaction_Status status;


    public Interaction_Type interactionType;

    [SerializeField] private GameObject targetGameobject;

    public void Start()
    {
        //if(interactionType == Interaction_Type.ANIMATION)
        //  UpdateStatus();
        audioSource = GameManager.gm.GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    public void UpdateStatus()
    {
        status = Interaction_Status.COMPLETED;
        GameManager.gm.InteractionCompleted(id);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (interactionType == Interaction_Type.TRIGGER && collider == targetGameobject.GetComponent<Collider>())
        {
            if (collider.GetComponent<Rigidbody>().useGravity)
            {
                GetComponent<Collider>().enabled = false;
                targetGameobject.gameObject.transform.position = transform.position;
                targetGameobject.gameObject.transform.rotation = transform.rotation;
                UpdateStatus();
            }

            if(gameObject.CompareTag("Barista Machine Trigger"))
            {
                StartCoroutine(PlayStopTriggerAnimation());
            }

            if (gameObject.CompareTag("Sugar trigger"))
            {
                StartCoroutine(PlayStopTriggerAnimation());
            }
        }
    }

    public void InteractAnimationObject()
    {
        StartCoroutine(AnimationCheck());
    }

    private IEnumerator AnimationCheck()
    {
        //Handle animation from target object;

        if (GameManager.gm.currentObjectInteraction.id == 2)
        {
            audioSource.clip = GameManager.gm.audioClipList[0];
            audioSource.Play();
        }

        if (GameManager.gm.currentObjectInteraction.id == 4)
        {
            audioSource.clip = GameManager.gm.audioClipList[1];
            audioSource.Play();
        }

        targetGameobject.SetActive(true);
        yield return new WaitForSeconds(5);
        targetGameobject.SetActive(false);
        audioSource.Stop();
        UpdateStatus();
    }

    private IEnumerator PlayStopTriggerAnimation()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        transform.GetChild(0).gameObject.SetActive(false);
        UpdateStatus();
    }
}
