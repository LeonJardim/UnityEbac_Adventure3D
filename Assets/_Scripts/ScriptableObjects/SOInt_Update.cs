using TMPro;
using UnityEngine;

public class SOInt_Update : MonoBehaviour
{
    public SOInt soInt;
    public TextMeshProUGUI uiText;

    private void Start()
    {
        UpdateText(soInt.Value);
    }
    
    private void OnEnable()
    {
        soInt.OnValueChanged += UpdateText;
        UpdateText(soInt.Value);
    }
    private void OnDisable()
    {
        soInt.OnValueChanged -= UpdateText;
    }

    private void UpdateText(int newValue)
    {
        uiText.text = newValue.ToString();
    }

}
