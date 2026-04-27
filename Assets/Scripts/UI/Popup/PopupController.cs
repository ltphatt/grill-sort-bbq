using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public static PopupController Ins { get; private set; }

    [SerializeField] private BasePopup[] popups;
    Dictionary<PopupType, BasePopup> popupDict = new();

    void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
            return;
        }
        Ins = this;

        popupDict = new();
        foreach (var popup in popups)
        {
            if (!popupDict.ContainsKey(popup.Type))
            {
                popupDict.Add(popup.Type, popup);
            }
        }
    }

    public void Show(PopupType type)
    {
        if (popupDict.TryGetValue(type, out var popup))
        {
            popup.transform.SetAsLastSibling();
            popup.Show();
        }
        else
        {
            Debug.LogError($"Popup of type {type} not found!");
        }
    }

    public void Hide(PopupType type)
    {
        if (popupDict.TryGetValue(type, out var popup))
        {
            popup.Hide();
        }
    }
}
