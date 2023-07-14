using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachBasket : MonoBehaviour
{
    /*private void OnCollisionEnter(Collision collision)
    {
        IKControl.Control.Drop(collision.transform);
    }*/
    static  Animator textAnimator;

    private void Start()
    {
        textAnimator = GetComponent<Animator>();
        EventManager.onDropInBasket += PopUp;
    }
    public static void PopUp()
    {
        textAnimator.SetTrigger("PopUpText");
        //if()
    }
}
