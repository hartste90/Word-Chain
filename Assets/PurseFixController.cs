using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurseFixController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputField;

    void Start()
    {
        _inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        _inputField.keyboardType = TouchScreenKeyboardType.PhonePad;

    }
    public void OnSubmitButtonPressed()
    {
        string inputAmt = _inputField.text;
        int amtSet;

        bool success = int.TryParse(inputAmt, out amtSet);
        if (success && amtSet >= 0)
        {
            MoneyController.SetMoney(amtSet);
            Close();
        }
        else
        {
            _inputField.text = "999";
        }
    }
    public void Open()
    {
        _inputField.text = string.Empty;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
