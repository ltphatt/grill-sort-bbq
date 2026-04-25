using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class Ghost : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] float targetPositionX = 200f;
    [SerializeField] float duration = 2f;
    [SerializeField] LoopType loopType = LoopType.Yoyo;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        rectTransform.DOAnchorPosX(targetPositionX, duration)
            .SetLoops(-1, loopType)
            .SetEase(Ease.InOutSine)
            .OnStepComplete(() =>
            {
                this.FlipGhost();
            });
    }

    void OnDestroy()
    {
        rectTransform.DOKill();
    }

    void FlipGhost()
    {
        rectTransform.localScale = new Vector3(-rectTransform.localScale.x, rectTransform.localScale.y, rectTransform.localScale.z);
    }
}
