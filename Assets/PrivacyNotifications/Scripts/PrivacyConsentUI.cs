using UnityEngine;
using UnityEngine.UI;

public class PrivacyConsentUI : MonoBehaviour
{
    public Toggle checkBox;
    public Button privacyPolicyButton;
    public Button proceedButton;
    public GameObject privacyPolicyScrollView;
    public GameObject standardUIView;
    private bool isPrivacyPolicyVisible = false;

    private void Start()
    {
        proceedButton.interactable = false; 
        privacyPolicyScrollView.SetActive(false); 

        privacyPolicyButton.onClick.AddListener(TogglePrivacyPolicy);
        checkBox.onValueChanged.AddListener(OnCheckBoxChanged);
        proceedButton.onClick.AddListener(OnProceed);
    }

    private void TogglePrivacyPolicy()
    {
        isPrivacyPolicyVisible = !isPrivacyPolicyVisible;
        privacyPolicyScrollView.SetActive(isPrivacyPolicyVisible);
    }

    private void OnCheckBoxChanged(bool isChecked)
    {
        proceedButton.interactable = isChecked;
    }

    // Hide standard UI once Proceed is clicked
    private void OnProceed()
    {
        standardUIView.SetActive(false);
        Debug.Log("Privacy Consent Accepted");
    }
}
