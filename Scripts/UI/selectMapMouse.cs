using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class selectMapMouse : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public GameObject SelectObject;

    [SerializeField]
    [Range(0.1f,4f)]float ScaleValue;

     public bool OnMouse = false;
     public bool OnMouseAction = false;
     public bool OutMouse = false;
     public bool OutMouseAction = false;

    Sequence DG;

    private void Start()
    {
        DG = DOTween.Sequence();
    }
    private void OnEnable()
    { 
        OnMouse = false;
        OnMouseAction = false;
        OutMouse = false;
        OutMouseAction = false;
    }

    void Update()
    {
        if(OnMouse && !OnMouseAction)
        {
            if(SelectObject != null)
            {
                SelectObject.SetActive(true);
            }
            OnMouseAction = true;
            OutMouse = false;
            OutMouseAction = false;
            DG.Kill();
            DG = DOTween.Sequence();
            DG = DoTweenManager.instance.OnMouseScale(transform, ScaleValue);

        }
        else if(OutMouse && !OutMouseAction)
        {
            if (SelectObject != null)
            {
                SelectObject.SetActive(false);
            }
            OutMouseAction = true;
            OnMouse = false;
            OnMouseAction = false;
           
            DG.Kill();
            DG = DOTween.Sequence();
            DG = DoTweenManager.instance.OutMouseScale(transform);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouse = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OutMouse = true;
    }
}
