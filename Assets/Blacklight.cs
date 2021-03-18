using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Blacklight : MonoBehaviour
{
    public Material[] materials;

    public Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Material reveal in materials)
        {
            reveal.SetVector("_LightPosition", light.transform.position);
            reveal.SetVector("_LightDirection", -light.transform.forward);
            reveal.SetFloat("_LightAngle", light.spotAngle);
        }
    }
}
