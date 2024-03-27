using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    public int SelectedValue;

    public GameObject[] Images;

    public Button Button;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
        Button.onClick.AddListener(OnButtonClick);
        dropdown.value = SelectedValue;
    }

    private void OnButtonClick()
    {
        if(dropdown.value < Images.Length - 1)
            dropdown.value++;
        else
            dropdown.value = 0;
    }

    private void OnValueChanged(int arg0)
    {
        HideImages();
        Images[arg0].SetActive(true);
    }

    private void HideImages()
    {
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].SetActive(false);
        }
    }
}
