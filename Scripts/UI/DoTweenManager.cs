using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    public static DoTweenManager instance;

    private void Start()
    {
        Init();
    }
    void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region popup
    public Sequence PoPUpSequnce()
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(transform.DOScale(0.4f, 3f))
            .Append(transform.DOScale(2f, 3f))
            .Append(transform.DOScale(1f, 3f));
    }
    #endregion

    public Sequence Fade(Image obj, float time)
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(obj.DOFade(0f, time));
    }

    #region 마우스 올릴때 Scale 커짐
    public Sequence OnMouseScale(Transform obj, float max)
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(obj.DOScale(max, 0.5f).SetEase(Ease.OutBounce));
    }
    #endregion

    #region 마우스 내릴때 Scale 작아짐
    public Sequence OutMouseScale(Transform obj2)
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(obj2.DOScale(1f, 0.5f));
    }
    #endregion

    #region outMouse
    public Sequence OutMouseMove(float OriginalPos, float target)
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(transform.DOLocalMoveX(OriginalPos, 0.5f))
            .Append(transform.DOLocalMoveX(target, 0.5f));
    }
    #endregion

    #region OnMouse
    public Sequence OnMouseMove(float OriginalPos, float target)
    {
        return DOTween.Sequence().SetAutoKill(false)
            .Append(transform.DOLocalMoveX(OriginalPos, 0.5f))
            .Append(transform.DOLocalMoveX(target, 0.5f));
    }
    #endregion
}
