using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Reflection;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;
using PlayFab.ClientModels;
using PlayFab;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timer = 0.0f;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private List<string> instructions = new();
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private GameObject instructionBackground;

    [SerializeField] private List<ObjectInteraction> objectInteractionsList = new();
    private Queue<ObjectInteraction> objectInteractionsQueue;
    public ObjectInteraction currentObjectInteraction;

    public AudioClip[] audioClipList;

    public static GameManager gm;

    void Start()
    {
        gm = this;

        Initialize();
    }

    private void Initialize()
    {
        objectInteractionsQueue = new Queue<ObjectInteraction>(objectInteractionsList.OrderBy(x => x.id));
        SetCurrentObjectInteraction();

        instructionText.text = instructions[0];
        instructionBackground.SetActive(true);
    }

    public void SetCurrentObjectInteraction()
    {
        if(objectInteractionsQueue.Count > 0)
        {
            currentObjectInteraction = objectInteractionsQueue.Peek();
            currentObjectInteraction.enabled = true;
            currentObjectInteraction.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Debug.Log("Finished");
        }
        
    }

    public void InteractionCompleted(int id)
    {
        if(id == currentObjectInteraction.id)
        {
            Debug.Log("Step completed " + id);

            // Enable filled water texture
            if (id == 2)
            {
                print("Water Filled");
                GameObject.Find("Cup Coffee Large 02 Empty").transform.GetChild(2).gameObject.SetActive(true);
            }

            // Enable filled coffee texture
            if (id == 4)
            {
                print("Coffee Filled");
                GameObject.Find("Cup Coffee Large 02 Empty").transform.GetChild(2).gameObject.SetActive(false);
                GameObject.Find("Cup Coffee Large 02 Empty").transform.GetChild(1).gameObject.SetActive(true);
            }

            //// Enable Mouse Cursor and disable Crosshair
            //if (id == 0)
            //{
            //    print("JNJCJB");
            //    Cursor.visible = true;
            //    Cursor.lockState = CursorLockMode.None;

            //}

            instructionText.text = instructions[id + 1];
            currentObjectInteraction.GetComponent<Outline>().enabled = false;
            currentObjectInteraction.enabled = false;
            if (objectInteractionsQueue.Count > 0)
            {
                objectInteractionsQueue.Dequeue();
                SetCurrentObjectInteraction();
            }
        }
       
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //SendTimer(timer.ToString());
        timer += Time.deltaTime;
        DisplayTime();        
    }

    void DisplayTime()
    {
        int mintues = Mathf.FloorToInt(timer/60.0f);
        int seconds = Mathf.FloorToInt(timer - mintues * 60);
        timerText.text = string.Format("{0:00}:{1:00}", mintues,seconds);        
    }

    public void SendTimer(string time)
    {        
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Time", timerText.text}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Sucessful user data send! : ");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Failed!!");
    }

    private void OnDestroy()
    {
        SendTimer(timer.ToString());
    }
}
