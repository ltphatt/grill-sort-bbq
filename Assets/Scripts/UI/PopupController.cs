using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject lockScreen;
    public GameObject popupSettings;

    void Start()
    {
        lockScreen.SetActive(false);
        popupSettings.SetActive(false);
    }
}
