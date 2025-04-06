using UnityEngine;

public class PrinterController : MonoBehaviour
{
    public GameObject paper;   // Reference to the paper object
    public Transform paperEndPos;  // The position where the paper should end up after being printed

    public float printSpeed = 1.0f;  // Speed at which the paper moves

    private bool isPrinting = false;

void Update()
{
    if (isPrinting)
    {
        // Disable the printer's collider while printing

        // Move the paper towards the end position
        paper.transform.position = Vector3.MoveTowards(paper.transform.position, paperEndPos.position, printSpeed * Time.deltaTime);

        // Stop moving the paper once it reaches the end position
        if (Vector3.Distance(paper.transform.position, paperEndPos.position) < 0.001f)
        {
            isPrinting = false;  // Stop the printing process
            Debug.Log("Paper reached the end position.");

            // Re-enable the printer's collider after printing
        }
    }
}

public void PrintPaper()
{
    Debug.Log("PrintPaper() called");
    isPrinting = true; // Start moving the paper
}
}