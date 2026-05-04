using TMPro;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public GameObject checkpointBox;

    private void Start()
    {
        SaveManager.Instance.FileLoaded += OnLoad;
    }

    public void OnLoad(SaveSetup setup)
    {
        if (setup.lastCheckpoint > 0)
        {
            checkpointBox.SetActive(true);
            uiText.text = "CHECKPOINT " + setup.lastCheckpoint.ToString();
        }
        
    }

    private void OnDestroy()
    {
        SaveManager.Instance.FileLoaded -= OnLoad;
    }
}
