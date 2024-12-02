using UnityEngine;
using UnityEngine.UI;

public class PrivacyConsentUI : MonoBehaviour
{
    // UI Elements
    public Toggle checkBox;
    public Button privacyPolicyButton;
    public Button proceedButton;
    public GameObject privacyPolicyScrollView;

    public GameObject standardUIView;

    private bool isPrivacyPolicyVisible = false;

    private void Start()
    {
        // Initial state
        proceedButton.interactable = false; // Disable Proceed button
        privacyPolicyScrollView.SetActive(false); // Hide privacy policy text

        // Assign listeners
        privacyPolicyButton.onClick.AddListener(TogglePrivacyPolicy);
        checkBox.onValueChanged.AddListener(OnCheckBoxChanged);
        proceedButton.onClick.AddListener(OnProceed);
    }

    private void TogglePrivacyPolicy()
    {
        isPrivacyPolicyVisible = !isPrivacyPolicyVisible;
        privacyPolicyScrollView.SetActive(isPrivacyPolicyVisible); // Toggle visibility
    }

    private void OnCheckBoxChanged(bool isChecked)
    {
        proceedButton.interactable = isChecked; // Enable or disable Proceed button
    }

    private void OnProceed()
    {
        // Hide everything once Proceed is clicked
        standardUIView.SetActive(false);

        Debug.Log("Privacy Consent Accepted. Proceeding...");
    }
}
