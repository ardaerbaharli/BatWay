using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    public bool isOn;

    [SerializeField] private Image offImage;

    public delegate void ValueChanged(bool value);

    public event ValueChanged valueChanged;

    public void Toggle(bool value)
    {
        isOn = value;
        ToggleImage(isOn);

        if (valueChanged != null)
            valueChanged(isOn);
    }

    private void ToggleImage(bool value)
    {
        offImage.gameObject.SetActive(!value);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn);
    }
}