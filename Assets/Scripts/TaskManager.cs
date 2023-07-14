using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    string[] fruits;
    int currentTaskNumber;
    string currentTaskTag;
    
    int min = 0;
    int max = 5;

    // Start is called before the first frame update
    void Start()
    {
        fruits = System.Enum.GetNames(typeof(Enums.Fruits));
        currentTaskNumber = Random.Range(min, max+1);
        currentTaskTag = fruits[Random.Range(0, fruits.Length - 1)];
        UIManager.UpdateTask(currentTaskNumber, currentTaskTag);
    }

}
