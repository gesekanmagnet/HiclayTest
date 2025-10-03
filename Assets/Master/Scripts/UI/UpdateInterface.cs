using TMPro;
using UnityEngine;

public class UpdateInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text updateText;
    [SerializeField] private GameObject updateButton;

    private void OnEnable()
    {
        EventCallback.OnDemandUpdate += Active;
        EventCallback.OnUpdateProgress += ShowProgress;
    }

    private void Active(bool enable)
    {
        updateText.SetText("Check update");
        updateText.gameObject.SetActive(enable);
        updateButton.SetActive(enable);
    }

    private void ShowProgress(long size, float progress)
    {
        updateText.text = $"Downloading {size / (1024f * 1024f):F2}MB : {progress * 100f:F1}%";
    }
}