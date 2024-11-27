using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPermissionsUI : MonoBehaviour
{
    public Slider acceptAllSlider;
    public Slider bodyTrackingSlider;
    public Slider personalDataSlider;
    public Slider eyeTrackingSlider;
    public Slider locationSlider;
    public Slider voiceRecordingSlider;
    public Slider cognitivePerfomanceSlider;
    public Button confirmButton;
    public GameObject uiPanel;

    private List<Slider> allSliders;

    // Flag to prevent recursive updates
    private bool isUpdating = false;

    void Start()
    {
        allSliders = new List<Slider> { bodyTrackingSlider, personalDataSlider, eyeTrackingSlider, locationSlider, voiceRecordingSlider, cognitivePerfomanceSlider };

        acceptAllSlider.onValueChanged.AddListener(OnAcceptAllSlid);
        confirmButton.onClick.AddListener(OnConfirmClicked);

        // Add listeners for each individual slider to detect manual changes
        foreach (var slider in allSliders)
        {
            slider.onValueChanged.AddListener(value => SnapSlider(slider));
        }
    }

    private void SnapSlider(Slider slider)
    {
        if (isUpdating) return; 

        isUpdating = true;
        slider.value = slider.value >= 0.5f ? 1 : 0; // Snap to 1 (right) if >= 0.5, otherwise to 0 (left)
        isUpdating = false;

        UpdateAcceptAllSlider();
    }
    private void OnAcceptAllSlid(float value)
    {
        if (isUpdating) return;

        bool isOn = value >= 0.5f; // Treat slider values >= 0.5 as "on"

        // Set all individual sliders to match the "Accept All" state
        isUpdating = true; // Prevent recursive triggering
        foreach (var slider in allSliders)
        {
            slider.value = isOn ? 1 : 0; // Set each slider to max (1) or min (0) based on Accept All slider
        }
        isUpdating = false;
    }

    // Update "Accept All" slider based on the state of individual sliders
    private void UpdateAcceptAllSlider()
    {
        // Check if all sliders are at max (1)
        bool allAreOn = true;
        foreach (var slider in allSliders)
        {
            if (slider.value < 1)
            {
                allAreOn = false;
                break;
            }
        }

        // Update "Accept All" slider
        isUpdating = true; // Prevent recursive triggering
        acceptAllSlider.value = allAreOn ? 1 : 0;
        isUpdating = false;
    }
    private void OnConfirmClicked()
    {
        uiPanel.SetActive(false);
    }
}
