using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(50, 500)]
    public float sens;

    public Transform body;

    private float xRot = 0f;
    private float Vrecoil;
    private float Hrecoil;


    private void Update()
    {
        float rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime + Hrecoil;
        float rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime + Vrecoil;

        xRot -= rotY;
        xRot = Mathf.Clamp(xRot, -17, 17);
        
       

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);


        body.Rotate(Vector3.up * rotX);
    }

    public void AddRecoil(float h, float v)
    {

        Hrecoil = h;
        Vrecoil = v;


    }

}
