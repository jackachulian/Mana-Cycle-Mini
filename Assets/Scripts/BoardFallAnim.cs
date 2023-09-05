using System.Collections;
using UnityEngine;

public class BoardFallAnim : MonoBehaviour
{

    private float initialVelocity = 1f;

    private float fallAccel = -5f;

    private float fallAngularVel = 15f;


    private float angle = 0;

    private float velocity = 0;

    private bool falling = false;

    // Use this for initialization
    public void Fall()
    {
        falling = true;
        velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling) return;

        velocity += fallAccel * Time.deltaTime;
        angle += fallAngularVel * Time.deltaTime;
        transform.position = transform.position + Vector3.up * velocity * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}