using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text task;
    string task_sample="Collect ";
    int min = 1;
    int max = 6;
    string[] fruits;
    enum Fruits
    {
        Apple,
        Orange,
        Banana
    }
    // Start is called before the first frame update
    void Start()
    {
        fruits = System.Enum.GetNames(typeof(Fruits));
        //System.Random random = new System.Random();
        task.text += task_sample+Random.Range(min, max) +" " + fruits[Random.Range(0,fruits.Length-1)]+'s';
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
