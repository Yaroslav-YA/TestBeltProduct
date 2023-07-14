using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    

    public delegate void DropInBasket();
    public static event DropInBasket onDropInBasket;

    

    public static void Drop()
    {
        onDropInBasket?.Invoke();
    }
}
