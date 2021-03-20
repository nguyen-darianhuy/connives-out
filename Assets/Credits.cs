using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(120f);
        SteamVR_Fade.View(Color.black, 2);
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
