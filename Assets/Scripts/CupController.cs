using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    public GameObject cup;
    public float coverDelay = 0.2f;
    public float fallSpeed = 1.0f;
    public float startHeight = 10.0f;

    public void MoveCupToDice(Vector3 dicePosition)
    {
        StartCoroutine(MoveCup(dicePosition));
    }

    private IEnumerator MoveCup(Vector3 dicePosition)
    {
        yield return new WaitForSeconds(coverDelay);

        Vector3 startPosition = new Vector3(dicePosition.x, dicePosition.y + startHeight, dicePosition.z);
        Vector3 endPosition = new Vector3(dicePosition.x, dicePosition.y + 0.1f, dicePosition.z);

        float journey = 0f;
        while (journey <= 1f)
        {
            journey += Time.deltaTime * fallSpeed / (startPosition.y - endPosition.y);
            cup.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }
    }
}
