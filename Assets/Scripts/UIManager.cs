using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static TMP_Text task;
    public static string task_sample = "Collect ";
    
    public enum Fruits
    {
        Apple,
        Lemon,
        Banana
    }
    // Start is called before the first frame update
    void Awake()
    {
        task = GetComponent<TMP_Text>();
        //task.text += task_sample+IKControl.currentTaskNumber+" " + IKControl.currentTaskTag+'s';   
    }

    public static void UpdateTask(int currentTaskNumber,string currentTaskTag)
    {
        task.text = task_sample + currentTaskNumber + " " + currentTaskTag + 's';
    }
}
