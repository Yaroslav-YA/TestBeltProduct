using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static TMP_Text task;
    public static string task_sample = "Collect ";
    public static string win_sample = "Level Passed";
    public Button reset_button;
    public static UIManager instance;
    
    public enum Fruits
    {
        Apple,
        Lemon,
        Banana
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        task = GetComponent<TMP_Text>();
        //Debug.Log(task);
        //task.text += task_sample+IKControl.currentTaskNumber+" " + IKControl.currentTaskTag+'s';   
    }

    public static void UpdateTask(int currentTaskNumber,string currentTaskTag)
    {
        task.text = task_sample + currentTaskNumber + " " + currentTaskTag + 's';
    }

    public void WinText()
    {
        task.text = win_sample;
        reset_button.gameObject.SetActive(true);
    }
}
