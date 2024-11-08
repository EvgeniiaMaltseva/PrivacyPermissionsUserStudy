using System.Collections;
using UnityEngine;

public class BookPageFlipper : MonoBehaviour
{
    public Transform page;           // The page to rotate
    public Transform spine;          // The spine (pivot point) of the book
    public float flipSpeed = 2f;     // Speed of the page flip
    public float maxFlipAngle = 100f;  // Maximum flip angle based on how open the book is (adjust this)
    private bool isFlipping = false;

    void Start()
    {
        // Ensure the spine is assigned (otherwise, it uses the page's position)
        if (spine == null)
        {
            Debug.LogError("Spine (pivot point) not assigned!");
        }
    }

    public void FlipPage()
    {
        if (!isFlipping)
        {
            StartCoroutine(FlipPageRoutine());
        }
    }

    IEnumerator FlipPageRoutine()
    {
        isFlipping = true;

        float angle = 0;
        float targetAngle = Mathf.Min(180f, maxFlipAngle);  // Limit to the maxFlipAngle

        // Rotate around the local Y-axis of the spine, respecting the book's current orientation
        Vector3 localYAxis = spine.up;  // Using the spine's local Y-axis

        while (angle < targetAngle)
        {
            float rotationStep = flipSpeed * Time.deltaTime * targetAngle;
            page.RotateAround(spine.position, localYAxis, rotationStep); // Rotate around spine's local Y-axis

            angle += rotationStep;

            // If the page goes beyond the limit, stop flipping
            if (angle >= targetAngle)
            {
                angle = targetAngle;
                break;
            }

            yield return null;
        }

        isFlipping = false;
    }
}
