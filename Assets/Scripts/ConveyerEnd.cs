using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerEnd : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PoolManager.Delete(collision.gameObject);
    }
}
