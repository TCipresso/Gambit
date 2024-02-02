using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    public Camera cam;
    public Rigidbody diceRigidbody;
    public float throwForceMultiplier = 1.0f;
    public float torqueForce = 10.0f;
    public float holdHeight = 1.5f;
    public float spinSpeed = 200.0f;

    private bool isDragging = false;
    private Vector3 lastMousePos;
    private Vector3 screenPoint;
    private Vector3 offset;
    private float heightDifference;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == diceRigidbody.gameObject)
            {
                isDragging = true;
                Vector3 dicePosition = diceRigidbody.transform.position;
                screenPoint = cam.WorldToScreenPoint(dicePosition);
                offset = dicePosition - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                
                heightDifference = holdHeight - dicePosition.y;

                diceRigidbody.isKinematic = true;
                lastMousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            }
        }

        if (isDragging)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenPoint) + offset;

            
            currentPosition.y += heightDifference;
            diceRigidbody.transform.position = currentPosition;

            
            diceRigidbody.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                diceRigidbody.isKinematic = false;
                Vector3 currentMousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
                Vector3 forceDirection = currentMousePos - lastMousePos;
                diceRigidbody.AddForce(forceDirection * throwForceMultiplier, ForceMode.Impulse);
                diceRigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
            }
            else
            {
                lastMousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            }
        }
    }

    float RandomTorque()
    {
        return Random.Range(-torqueForce, torqueForce);
    }
}
