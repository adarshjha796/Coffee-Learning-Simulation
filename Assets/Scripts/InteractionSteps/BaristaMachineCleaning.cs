using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectInteraction;

public class BaristaMachineCleaning : MonoBehaviour
{
    public GameObject cleaningEffect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cloth"))
        {
            StartCoroutine(EnableDisableAnimation());
        }
    }

    IEnumerator EnableDisableAnimation()
    {
        cleaningEffect.SetActive(true);

        yield return new WaitForSeconds(5);

        cleaningEffect.SetActive(false);
        print("AAA : " + FindObjectOfType<ObjectInteraction>().id);
        if (FindObjectOfType<ObjectInteraction>().id == 9)
        {
            print("B");
            FindObjectOfType<ObjectInteraction>().status = Interaction_Status.COMPLETED;
            GameManager.gm.InteractionCompleted(FindObjectOfType<ObjectInteraction>().id);
            GameManager.gm.SetCurrentObjectInteraction();
        }
    }
}
