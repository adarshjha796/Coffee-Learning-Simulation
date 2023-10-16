using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI tutorialDescription; 
    //[SerializeField] TutorialTrackerSO tutorialTrackerSO;

    [SerializeField] private Image images;

    private void OnMouseUpAsButton()
    {
        print("Hello");
        //images.rectTransform.sizeDelta = new Vector2(65, 60);
        images.color = new(1, 1, 1, 1);
        images.transform.GetChild(0).gameObject.SetActive(true);
    }
}
