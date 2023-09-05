using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private Vector3 shakeNormal = Vector3.right;
    [SerializeField] private float frequency = 10f;
    [SerializeField] private float shakeDuration = 1.25f;

    private float shakeIntensity;
    private Vector3 shakeCenter;

    // Update is called once per frame
    void Update()
    {
        if (shakeIntensity > 0)
        {
            transform.localPosition = shakeCenter + shakeNormal * Mathf.Sin(Time.time * frequency * Mathf.PI) * shakeIntensity;
            shakeIntensity -= Time.deltaTime / shakeDuration;
            if (shakeIntensity <= 0)
            {
                transform.localPosition = shakeCenter;
            }
        }
    }

    public void StartShake()
    {
        if (shakeIntensity <= 0) shakeCenter = transform.localPosition;
        shakeIntensity = 1;
    }
}