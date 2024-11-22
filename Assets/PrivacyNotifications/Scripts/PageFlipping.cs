using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFlipping : MonoBehaviour
{
    public List<Transform> rightPages;    // List of pages on the right side of the book
    public List<Transform> leftPages;     // List of pages on the left side of the book
    public Transform spine;               // The spine (pivot point) of the book
    public float flipSpeed = 2f;          // Speed of the page flip
    public float maxFlipAngle = 160f;     // Maximum flip angle

    private bool isFlipping = false;

    void Start()
    {
        if (spine == null)
        {
            Debug.LogError("Spine (pivot point) not assigned!");
        }
    }

    public void FlipRightToLeft()
    {
        if (!isFlipping && rightPages.Count > 0)
        {
            // Get the top page on the right side
            Transform topPage = rightPages[rightPages.Count - 1];
            rightPages.RemoveAt(rightPages.Count - 1);
            leftPages.Add(topPage);

            StartCoroutine(FlipPageRoutine(topPage, true));
        }
    }

    public void FlipLeftToRight()
    {
        if (!isFlipping && leftPages.Count > 0)
        {
            // Get the top page on the left side
            Transform topPage = leftPages[leftPages.Count - 1];
            leftPages.RemoveAt(leftPages.Count - 1);
            rightPages.Add(topPage);

            StartCoroutine(FlipPageRoutine(topPage, false));
        }
    }

    private IEnumerator FlipPageRoutine(Transform page, bool rightToLeft)
    {
        isFlipping = true;

        float angle = 0;
        float targetAngle = Mathf.Min(180f, maxFlipAngle);  // Limit to the maxFlipAngle
        Vector3 localYAxis = spine.up;  // Using the spine's local Y-axis

        if (!rightToLeft)
        {
            targetAngle = -targetAngle; // Flip direction for left-to-right
        }

        while (Mathf.Abs(angle) < Mathf.Abs(targetAngle))
        {
            float rotationStep = flipSpeed * Time.deltaTime * Mathf.Abs(targetAngle);
            page.RotateAround(spine.position, localYAxis, rotationStep * Mathf.Sign(targetAngle));
            angle += rotationStep;

            if (Mathf.Abs(angle) >= Mathf.Abs(targetAngle))
            {
                angle = targetAngle; // Ensure it ends exactly at the target angle
                break;
            }

            yield return null;
        }

        isFlipping = false;
    }
}
