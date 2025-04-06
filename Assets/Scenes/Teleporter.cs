using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
  int index = 1;
  int b = 0;
  int u = 0;
  int m = 0;
  public GameObject b1;
  public GameObject b2;
  public GameObject b3;
  public AudioSource bs;
  public AudioClip win;
  public AudioClip pop;
  public GameObject c1;
  public GameObject c2;
  public GameObject c3;
  public GameObject c4;
  public GameObject m1;
  public GameObject m2;
  public GameObject m3;
  public GameObject m4;
  public GameObject m5;
  public GameObject m6;
  public GameObject m7;
  public GameObject m8;
  public GameObject m9;
  public GameObject m10;
  public AudioClip line1;
  public AudioClip line2;

  private PermissionManager permissionManager;
  private Vector3[] pos;

  public float roomRange = 5f; // Range to determine if the user is in a room


  void Start()
  {
    this.transform.position = new Vector3(-7.698f, -3.5f, -17.471f);  // room 1 - hello

    // Get the PermissionManager instance
    permissionManager = FindObjectOfType<PermissionManager>();
    if (permissionManager == null)
    {
      Debug.LogError("PermissionManager not found in the scene!");
    }
  }

  void Update()
  {
    pos = new Vector3[] {
        new Vector3(-7.698f, -3.5f, -17.471f),  // room 1 - hello
        //new Vector3(-7.698f, -3.5f, -12.471f),  // room 2 - face
        //new Vector3(-7.698f, -3.5f, -2.044f),   // room 3 - captcha - velvet
        new Vector3(-10.701f, 0f, -4.24f),      // room 4 - find letters - church <---------(book-ui)
        //new Vector3(10.391f, 0f, -4.24f),       // room 5 - color vision - daisy <---------(standard-ui)
        //new Vector3(25.165f, 0f, -9.365f),      // room 6 - proximity - red
        //new Vector3(15.658f, 0f, -15.61f),      // room 7 - MOCA short - recluse
        //new Vector3(-25.475f, 0f, -7.88f),      // room 8 - wingspan - cave <---------(standard-ui)
        new Vector3(-15.587f, 0f, -17.425f),    // room 9 - fitness - motivation <---------(book-ui)
        //new Vector3(-21.032f, 0f, -16.16f),     // room 10 - puzzle - deafening
        //new Vector3(21.01f, 0f, -15.04f),       // room 11 - reaction time - flash
        //new Vector3(-24.65f, 3.5f, -2.198f),    // room 12 - ceiling - finally <---------(standard-ui)
        new Vector3(-22.149f, 3.5f, -17.242f),  // room 13 - language - apple <---------(book-ui)
        //new Vector3(-25.293f, 3.5f, -8.354f),   // room 14 - pattern - i can
        //new Vector3(-14.907f, 3.5f, -16.331f),  // room 15 - frame rate - conception
        //new Vector3(-10.84f, 3.5f, -3.78f),     // room 16 - MOCA animals - lion rhino camel <---------(standard-ui)
        new Vector3(12.85f, 3.5f, -15.76f),     // room 17 - MOCA serial 7 - 65 <---------(book-ui) 
        //new Vector3(15.658f, 0f, -15.61f),      // room 18 - MOCA long - recluse
        //new Vector3(25.16f, 3.5f, -10.541f),    // room 19 - MOCA abstraction - vehicle, measurement <---------(standard-ui)
        new Vector3(24.61f, 3.5f, -1.98f),      // room 20 - MOCA language <---------(book-ui)
        //new Vector3(24.61f, 0f, -0.98f),        // room 21 - einstein - einstein
        //new Vector3(12.553f, 3.5f, -4.481f),    // room 22 - orientation
        new Vector3(23.03f, 3.5f, -15.95f),     // room 23 - close vision - twelve <---------(book-ui)
        //new Vector3(20.794f, 3.5f, -11.245f),   // room 24 - distance vision - digital playground <---------(standard-ui)
        new Vector3(1f, 20f, -8.75f)            // room 25 - victory
      };

    if (Input.GetKeyDown("space"))
    {
      MoveToNextRoom();
    }
    // this.transform.position = pos[index++];
    // bs.PlayOneShot(win, 1f);

    if (Input.GetKeyDown("b"))
    {
      this.transform.position = pos[--index];
    }

    if (Input.GetKeyDown("p"))
    {
      b++;
      if (b == 1) b1.SetActive(false);
      if (b == 2) b2.SetActive(false);
      if (b == 3) b3.SetActive(false);
      bs.PlayOneShot(pop, 1f);
    }

    if (Input.GetKeyDown("c"))
    {
      bs.PlayOneShot(line1, 1f);
    }

    if (Input.GetKeyDown("v"))
    {
      bs.PlayOneShot(line2, 1f);
    }

    if (Input.GetKeyDown("r"))
    {
      b = 0;
      u = 0;
      m = 0;
      b1.SetActive(true);
      b2.SetActive(true);
      b3.SetActive(true);
      c1.SetActive(true);
      c2.SetActive(true);
      c3.SetActive(true);
      c4.SetActive(true);
      m1.SetActive(true);
      m2.SetActive(true);
      m3.SetActive(true);
      m4.SetActive(true);
      m5.SetActive(true);
      m6.SetActive(true);
      m7.SetActive(true);
      m8.SetActive(true);
      m9.SetActive(true);
      m10.SetActive(true);
    }

    if (Input.GetKeyDown("u"))
    {
      u++;
      if (u == 1) c1.SetActive(false);
      if (u == 2) c2.SetActive(false);
      if (u == 3) c3.SetActive(false);
      if (u == 4) c4.SetActive(false);
    }

    if (Input.GetKeyDown("m"))
    {
      m++;
      if (m == 1) m1.SetActive(false);
      if (m == 2) m2.SetActive(false);
      if (m == 3) m3.SetActive(false);
      if (m == 4) m4.SetActive(false);
      if (m == 5) m5.SetActive(false);
      if (m == 6) m6.SetActive(false);
      if (m == 7) m7.SetActive(false);
      if (m == 8) m8.SetActive(false);
      if (m == 9) m9.SetActive(false);
      if (m == 10) m10.SetActive(false);
    }
  }


  public void MoveToNextRoom()
  {
    if (index < pos.Length)
    {
      Vector3 nextPosition = pos[index];
      this.transform.position = nextPosition;
      bs.PlayOneShot(win, 1f);
      index++;

      // Check for permission for the new room
      if (permissionManager != null)
      {
        permissionManager.CheckRoomPermission(this.transform.position);
      }
    }
    else
    {
      Debug.Log("No more rooms to teleport to.");
    }
  }
  public bool CanSkipToNextRoom()
  {
    return index < pos.Length;
  }

  public Vector3 GetNextPosition()
  {
    if (index < pos.Length)
    {
      return pos[index];
    }
    return pos[pos.Length - 1]; // Return last position if no more rooms
  }

  public Vector3 GetCurrentRoomPosition()
  {
    foreach (var roomPosition in pos)
    {
      if (Vector3.Distance(this.transform.position, roomPosition) <= roomRange)
      {
        Debug.Log($"Current room detected at position: {roomPosition}");
        return roomPosition;
      }
    }

    Debug.LogWarning("Player is not in any room range.");
    return Vector3.zero; // Default value if no room matches
  }
}
