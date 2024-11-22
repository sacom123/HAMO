using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    [SerializeField]LevelManager LM;
    [SerializeField]TileMapEditor TME;


    public void SaveButtonAction()
    {
        LM.SaveMapButton();
    }

    public void ResetButtonAction()
    {
        LM.MapReset();
    }
}
