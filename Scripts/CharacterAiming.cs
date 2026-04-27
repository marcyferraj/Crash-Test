using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yaw = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yaw, 0), turnSpeed * Time.deltaTime);
    }
}
