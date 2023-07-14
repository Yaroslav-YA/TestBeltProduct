using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public delegate void TaskUpdate(int score, string tag);
    public static event TaskUpdate onTaskUpdate;

    public delegate void TaskCompleted();
    public static event TaskCompleted onTaskComplete;

    int currentNumber;
    int currentTaskNumber;
    
    string currentTaskTag;

    [SerializeField] int min = 0;
    [SerializeField] int max = 5;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTask();
        onTaskUpdate?.Invoke(currentTaskNumber, currentTaskTag);
        EventManager.onDropInBasket += AddScore;
    }

    void GenerateTask()
    {
        string[] fruits = System.Enum.GetNames(typeof(Enums.Fruits));
        currentTaskNumber = Random.Range(min, max + 1);
        currentTaskTag = fruits[Random.Range(0, fruits.Length - 1)];
    }
    void AddScore()
    {
        currentNumber++;
        if (currentNumber >= currentTaskNumber)
        {
            FinishGame();
        }
        onTaskUpdate?.Invoke(currentTaskNumber - currentNumber, currentTaskTag);
    }

    void FinishGame()
    {
        onTaskComplete?.Invoke();       
    }
}
