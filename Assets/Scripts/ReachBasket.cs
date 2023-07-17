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

    private void OnEnable()
    {
        IKControl.onDropInBasket += PopUp;
    }
    private void OnDisable()
    {
        IKControl.onDropInBasket -= PopUp;
    }
    private void Start()
    {
        textAnimator = GetComponent<Animator>();
        
    }
    public static void PopUp()
    {
        textAnimator.SetTrigger("PopUpText");
        //if()
    }
}
