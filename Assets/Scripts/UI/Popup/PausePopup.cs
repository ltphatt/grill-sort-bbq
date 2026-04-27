using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : BasePopup
{
    protected override void OnShow()
    {
        Time.timeScale = 0f;
    }

    protected override void OnHide()
    {
        Time.timeScale = 1f;
    }
}
