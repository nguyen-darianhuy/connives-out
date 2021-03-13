using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class Movement : MonoBehaviour
{
    
    public SteamVR_Action_Vector2 trackmove;
    public SteamVR_Input_Sources hand;//Set Hand To Get Input From
    public float speed = 3f;

    private Vector3 gravity = new Vector3(0, -9.81f, 0);

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector2 tracking = trackmove.GetAxis(hand);

        float x = tracking.x;
        float z = tracking.y;

        Vector3 move =
            Camera.main.transform.right * x +
            Camera.main.transform.forward * z;

        controller.Move(move * speed * Time.deltaTime + gravity * Time.deltaTime );
    }
}