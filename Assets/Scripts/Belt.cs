using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    public float belt_speed = 0.000001f;
    // Start is called before the first frame update
    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.transform.Translate(belt_speed*Time.deltaTime, 0, 0);
    }
}
