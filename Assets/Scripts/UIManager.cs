using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    TMP_Text task;
    string task_sample = "Collect ";
    string win_sample = "Level Passed";
    public Button reset_button;
    

    private void OnEnable()
    {
        ScoreManager.onTaskUpdate += UpdateTask;
        ScoreManager.onTaskComplete += WinText;
    }
    private void OnDisable()
    {
        ScoreManager.onTaskUpdate -= UpdateTask;
        ScoreManager.onTaskComplete -= WinText;
    }
    // Start is called before the first frame update
    void Awake()
    {
        task = GetComponent<TMP_Text>();
        reset_button.gameObject.SetActive(false); 
    }

    public void UpdateTask(int currentTaskNumber,string currentTaskTag)
    {
        task.text = task_sample + currentTaskNumber + " " + currentTaskTag;
        if (currentTaskNumber > 1)
        {
            task.text += 's';
        }
    }

    public void WinText()
    {
        task.text = win_sample;
        reset_button.gameObject.SetActive(true);
    }
}
