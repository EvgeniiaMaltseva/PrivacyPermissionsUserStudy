using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpenBook : MonoBehaviour
{
    [SerializeField] Button openBtn;

    private Vector3 rotationVector;

    private bool isOpenClicked;
    
    private DateTime startTime;
    private DateTime endTime;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip openBook;

    // Start is called before the first frame update
    void Start()
    {
        if (openBtn != null)
            openBtn.onClick.AddListener(() => openBtn_Click());
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenClicked)
        {
            transform.Rotate(rotationVector * Time.deltaTime);
            endTime = DateTime.Now;

            if ((endTime - startTime).TotalSeconds >= 1)
            {
                isOpenClicked = false;
            }
        }
    }

    private void openBtn_Click()
    {
        isOpenClicked = true;
        startTime = DateTime.Now;
        rotationVector = new Vector3(0, 180, 0);
        playSound();
    }

    private void playSound()
    {
        if ((audioSource != null) && (openBook != null))
        {
            audioSource.PlayOneShot(openBook);
        }
    }
}
