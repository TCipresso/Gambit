using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public Transform diceTransform;
    public Transform launchPoint;
    public float launchForce = 10.0f;
    public float spinSpeed = 1000.0f;
    public CupController cupController;

    private bool isDiceInAir = false;

    void Update()
    {
        if (isDiceInAir)
        {
            diceTransform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.World);
        }
    }

    public void RollDice()
    {
        diceTransform.position = launchPoint.position;
        diceTransform.rotation = Quaternion.identity;

        Rigidbody diceRigidbody = diceTransform.GetComponent<Rigidbody>();
        if (diceRigidbody != null)
        {
            diceRigidbody.isKinematic = false;
            diceRigidbody.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);
        }

        isDiceInAir = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        isDiceInAir = false;
        Debug.Log("Dice Position: " + diceTransform.position);

        if (cupController != null)
        {
            cupController.MoveCupToDice(diceTransform.position);
        }
    }
}
