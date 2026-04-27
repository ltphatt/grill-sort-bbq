using System;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    public PopupType Type;
    [SerializeField] protected Button closeButton;
    [SerializeField] protected Button overlay;
    private Action onPopupClosed;

    protected virtual void Awake()
    {
        if (closeButton != null) closeButton.onClick.AddListener(Hide);
        if (overlay != null) overlay.onClick.AddListener(Hide);
    }

    protected virtual void Start()
    {
        Hide();
    }

    public virtual void Show(Action onClose = null)
    {
        onPopupClosed = onClose;
        gameObject.SetActive(true);

        OnShow();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        onPopupClosed?.Invoke();

        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }
}
