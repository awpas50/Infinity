using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 600f;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject barrel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        BodyMovement();
        BarrelMovement();
    }

    void BodyMovement()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(movement * Time.deltaTime * speed, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void BarrelMovement()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Virtual plane
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
            barrel.transform.LookAt(new Vector3(pointToLook.x, barrel.transform.position.y, pointToLook.z));
        }
        
        //Vector3 mousePos2D = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.z));
        //Debug.Log(mousePos2D);
        //Quaternion toRotation = Quaternion.LookRotation(mousePos2D - transform.position, Vector3.up);
        //barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        //barrel.transform.LookAt(barrelDir);
    }
}
