using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    public float belt_speed = 0.000001f;
    Vector2 offset;
    Material material;
    // Start is called before the first frame update
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = new Vector2(belt_speed, 0);
    }

    private void FixedUpdate()
    {
        material.mainTextureOffset = offset*Time.time;
    }
    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.transform.Translate(belt_speed*Time.fixedDeltaTime, 0, 0);
    }
}
