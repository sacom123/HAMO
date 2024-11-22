using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TileSelectPoPUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sequence TSPSequence;

    [SerializeField]bool isMoveUP = false;
    [SerializeField]bool isMoveupCall = false;

    [SerializeField]bool isMoveDown = false;
    [SerializeField]bool isMoveDownCall = false;

    bool IsMouseCheck = false;

    public void Start()
    {
        TSPSequence = DOTween.Sequence();
    }
    public void Update()
    {
        if (IsMouseCheck)
        {
            if (isMoveUP && !isMoveupCall)
            {
                PoPUpMoveUp();
                isMoveDownCall = false;
            }
            else if (isMoveDown && !isMoveDownCall)
            {
                PoPUpMoveDown();
                isMoveupCall = false;
            }
        }
    }

    void PoPUpMoveUp()
    {
        if (!TSPSequence.IsActive())
        {
            TSPSequence.Kill(); // Kill the sequence to reset it
            TSPSequence = DOTween.Sequence(); // Recreate the sequence
            TSPSequence.Append(transform.DOLocalMoveY(-310f, 0.5f))
                .OnComplete(() => isMoveupCall = true); // Set completion status
        }
    }
    void PoPUpMoveDown()
    {
        if(!TSPSequence.IsActive())
        {
            TSPSequence.Kill(); // Kill the sequence to reset it
            TSPSequence = DOTween.Sequence(); // Recreate the sequence
            TSPSequence.Append(transform.DOLocalMoveY(-680f, 0.5f))
                .OnComplete(() => isMoveDownCall = true); // Set completion status
        }
    }

    public void PoPUp()
    {
        TSPSequence = DOTween.Sequence()
            .Append(transform.DOScale(0.4f, 0.5f))
            .Append(transform.DOScale(2f, 0.5f))
            .Append(transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce))
            .OnComplete(() => IsMouseCheck = true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMoveUP = true;
        isMoveDown = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isMoveDown = true;
        isMoveUP = false;
    }
}
