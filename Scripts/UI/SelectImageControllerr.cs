using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageControllerr : MonoBehaviour
{
    [SerializeField]List<selectMapMouse> selectMapMouse;

    private void OnEnable()
    {
        for (int i = 0; i < selectMapMouse.Count; i++)
        {
            selectMapMouse[i].OnMouse = false;
            selectMapMouse[i].OnMouseAction = false;
            selectMapMouse[i].OutMouse = true;
            selectMapMouse[i].OutMouseAction = false;
            selectMapMouse[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
