using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheels : MonoBehaviour
{
    public Rigidbody CarBody;
    public Transform[] Wheels;
    public float WheelDiameter = 0.12f;
    void Start()
    {
        foreach (Transform wheel in Wheels)
        {
            float random_z = Random.Range(0.0f,360.0f);
            wheel.Rotate(0, 0, random_z);
        }
    }
    void Update()
    {
        float circumference = 3.14159f * WheelDiameter;
        float rate = CarBody.velocity.magnitude / circumference;
        foreach (Transform wheel in Wheels)
        {
            wheel.Rotate(0, 0, -rate);
        }
    }
}
