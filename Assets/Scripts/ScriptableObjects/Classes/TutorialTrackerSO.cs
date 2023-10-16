using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TutorialTrackerSO", menuName = "Scriptable Objects/TutorialTracker", order = 1)]
public class TutorialTrackerSO : ScriptableObject
{
    [Header("Ints")]
    private int step = 0;

    [Header("Ints")]
    public string[] tutorialsDescription;

    [Header("Events")]
    [System.NonSerialized]
    private UnityEvent tutorialFinished;

    public int Step
    {
        get
        {
            return step;
        }
        set
        {
           step = value;
           tutorialFinished?.Invoke();
        }
    }
}
