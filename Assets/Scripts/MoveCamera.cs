using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float mouseSensitivity=100f;
    float yRotation = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

       float mouseX = Input.GetAxis("Mouse X") *mouseSensitivity*Time.deltaTime;
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation,+135,+225);
        transform.localRotation = Quaternion.Euler(0,yRotation,0);
    }
}
