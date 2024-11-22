using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    [SerializeField] LevelManager LM;


    public void LoadButtonAction()
    {
        LM.LoadMapButton();
    }
}
