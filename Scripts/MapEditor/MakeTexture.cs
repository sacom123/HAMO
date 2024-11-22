using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MakeTexture : MonoBehaviour
{
    [Header("랜더링 할 맵")]
    public Tilemap[] TM;                                                            // 렌더링을 할 맵

    [Header("랜더링 완료한 택스처")]
    [SerializeField]Texture2D[] TileMapTexture = new Texture2D[3];                  // 렌더링 완료한 텍스쳐를 담을 텍스쳐 2d

    [Header("랜더링 할 카메라")]
    public Camera[] RenderCam;                                                      // 렌더링할 카메라

    [Header("랜더링 이미지")]
    [SerializeField] RawImage[] raw = new RawImage[3];                              // 렌더링한 이미지를 담을 RawImage

    [Header("맵 교체")]
    [SerializeField] RawImage[] SelectTextuer;                                      // 맵을 교체할 텍스쳐
    [SerializeField] GameObject[] SelectButtonUi;                                   // 맵을 교체할 버튼

    [Header("저장된 맵 불러오기")]
    [SerializeField] RawImage[] LoadSelectTextuer;                                  // 저장된 맵을 불러올 텍스쳐
    [SerializeField] GameObject[] LoadSelectButtonUi;                               // 저장된 맵을 불러올 버튼

    [Header("메인 맵")]
    [SerializeField] RawImage[] MainSelectTextuer;                                  // 메인 맵 텍스쳐
    [SerializeField] GameObject[] MainSelectButtonUi;                               // 메인 맵 버튼


    [Header("맵 데이터")]
    public LevelData[] LD;                                                          // 타일맵 데이터를 담을 배열

    [SerializeField] GameObject[] ButtonUI = new GameObject[3];                     // 버튼을 눌렀을때 렌더링할 UI


    #region 각 타입에 맞는 텍스처로 메인 텍스처 교체
    public void ChangeTextureSelect()
    {
        for (int i = 0; i < SelectButtonUi.Length; i++)
        {
            ButtonUI[i] = SelectButtonUi[i];
            raw[i] = SelectTextuer[i];
        }
        MakeTM();
    }

    public void LoadTextureSelect()
    {
        for (int i = 0; i < LoadSelectButtonUi.Length; i++)
        {
            ButtonUI[i] = LoadSelectButtonUi[i];
            raw[i] = LoadSelectTextuer[i];
        }
        MakeTM();
    }

    public void MainMapTextureSelect()
    {
        for (int i = 0; i < MainSelectButtonUi.Length; i++)
        {
            ButtonUI[i] = MainSelectButtonUi[i];
            raw[i] = MainSelectTextuer[i];
        }
        MakeTM();
    }
    #endregion

    #region 텍스처로 렌더링할 맵을 그리는 함수
    void MakeTM()
    { 
        if(LD.Length == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < LD.Length; i++)
            {
                if (LD[i] == null)
                {
                    return;
                }
                else
                {
                    for (int j = 0; j < LD[i].pos.Count; j++)
                    {
                        TM[i].SetTile(LD[i].pos[j], LD[i].tiles[j].TileBase);
                    }
                    RenderTextureAction(i);
                }
            }
        }
    }
    #endregion

    #region 그린 맵을 텍스처화 시키는 함수
    void RenderTextureAction(int i)
    {
        ButtonUI[i].SetActive(true);
       
        RenderCam[i].enabled = true;
       
        RenderTexture TileManpRenderTexture;    // 렌더링을 할 렌더텍스쳐
       
        TileManpRenderTexture = new RenderTexture(490, 853, 0);
       
        TileMapTexture[i] = new Texture2D(490, 853);
       
        RenderCam[i].targetTexture = TileManpRenderTexture;
       
        RenderCam[i].Render();
       
        RenderTexture.active = TileManpRenderTexture;
       
        TileMapTexture[i].ReadPixels(new Rect(0, 0, TileManpRenderTexture.width, TileManpRenderTexture.height), 0, 0);
       
        TileMapTexture[i].Apply();

        raw[i].texture = TileMapTexture[i];
       
        RenderTexture.active = null;
       
        RenderCam[i].targetTexture = null;
       
        RenderCam[i].enabled = false;

        TM[i].ClearAllTiles();
       
        Destroy(TileManpRenderTexture);
    }
    #endregion
}
