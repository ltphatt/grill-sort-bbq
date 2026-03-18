using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonToggle : MonoBehaviour
{
    public bool IsOn = true;
    public Image background;
    public Image turnOnCheckmark;
    public Image turnOffCheckmark;

    public List<Sprite> backgroundSprites = new();
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnToggle);
    }

    void Start()
    {
        IsOn = true;
        UpdateButtonVisual();
    }

    void OnToggle()
    {
        IsOn = !IsOn;
        UpdateButtonVisual();
    }

    void UpdateButtonVisual()
    {
        if (background != null && backgroundSprites.Count > 0)
        {
            background.sprite = IsOn ? backgroundSprites[0] : backgroundSprites[1];
            background.SetNativeSize();
        }

        if (turnOffCheckmark == null || turnOnCheckmark == null)
        {
            return;
        }

        turnOnCheckmark.gameObject.SetActive(IsOn);
        turnOffCheckmark.gameObject.SetActive(!IsOn);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnToggle);
    }

}
