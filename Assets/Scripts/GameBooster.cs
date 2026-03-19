using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBooster : MonoBehaviour
{
    [SerializeField] int timeBoost = 30;

    public void OnUseMagnet()
    {
        Observer.Notify(EventMessage.ON_USE_BOOSTER_MAGNET);
    }

    public void OnUseShuffle()
    {
        Observer.Notify(EventMessage.ON_USE_BOOSTER_SHUFFLE);
    }

    public void OnUseTimeBooster()
    {
        Observer.Notify(EventMessage.ON_USE_BOOSTER_TIME, timeBoost);
    }
}
