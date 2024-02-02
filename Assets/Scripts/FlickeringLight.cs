using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.07f;

    private float random;

    void Start()
    {
        if (flickerLight == null)
        {
            flickerLight = GetComponent<Light>();
        }
        random = Random.Range(0.0f, 65535.0f);
    }

    void Update()
    {
        // Random noise to intensity and time to simulate flickering
        float noise = Mathf.PerlinNoise(random, Time.time * flickerSpeed);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        flickerLight.intensity = intensity;

        // Optional: Slightly change the light color over time
        // flickerLight.color = new Color(1f, noise, 0.5f + noise * 0.5f);
    }
}
