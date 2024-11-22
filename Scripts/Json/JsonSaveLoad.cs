using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Tilemaps;
using System;


public class JsonSaveLoad : MonoBehaviour
{
    string TileMapfilePath;
    string ItemfilePath;

    public  int i = 0;

    [SerializeField]TextAsset[] MapText;
    [SerializeField] TextAsset[] MapText2;

    [SerializeField]TextAsset[] ItemText;
    [SerializeField] TextAsset[] ItemText2;
    
    [SerializeField] LevelData data;

    public DirectoryInfo Di = new DirectoryInfo(Application.dataPath + "/Resources");
    public DirectoryInfo itemDi = new DirectoryInfo(Application.dataPath + "/Resources");
    
    // Save LevelData to Json File
    public void TileMapLevelJsonSave(LevelData Tilemap, ItemlevelData itemlevel)
    {
        TileMapfilePath = Path.Combine(Application.dataPath, "Resources/", "Tile" + i + ".json");
        string tilemapjson = JsonUtility.ToJson(Tilemap,true);
        File.WriteAllText(TileMapfilePath, tilemapjson);
        
        ItemfilePath = Path.Combine(Application.dataPath, "Resources/", "Item" + i + ".json");
        string Itemjson = JsonUtility.ToJson(itemlevel, true);
        File.WriteAllText(ItemfilePath, Itemjson);

        i++;
    }
    

    public LevelData TileLoadToJson(string Type, int index, int mapeditorIndex)
    {
        LevelData data = new LevelData();
        data = LoadJsonData<LevelData>(Type, index, mapeditorIndex);
        return data;
    }
    public ItemlevelData ItemLoadToJson(string Type, int index, int mapeditorIndex)
    {
        ItemlevelData data = new ItemlevelData();
        data = LoadJsonData<ItemlevelData>(Type, index, mapeditorIndex);
        return data;
    }

    public int CheckJsonFile(int mapeditorIndex)
    {
        int filecount = 0;
        for(int i = 0; i < 3; i++)
        {
            string mapTextPath = Path.Combine("Tile" + mapeditorIndex + "-" + i + ".json");
            string ItemTextPath = Path.Combine("Item" + mapeditorIndex + "-" + i + ".json");
            MapText = new TextAsset[3];
            ItemText = new TextAsset[3];
            foreach(FileInfo File in Di.GetFiles("*json"))
            {
                Debug.Log(File.Name);
                if(File.Name == mapTextPath)
                {
                    filecount++;
                }
            }
        }
        

        return filecount;
    }

    // Delete Json File
    public void ChangeJson(int index, int mapeditorIndex, LevelData TileMap, ItemlevelData itemlevel)
    {
        TileMapfilePath = Path.Combine(Application.dataPath, "Resources/", "Tile" + mapeditorIndex + "-" + index + ".json");
        ItemfilePath = Path.Combine(Application.dataPath, "Resources/", "Item" + mapeditorIndex + "-"+ index + ".json");

        string tilemapjson = JsonUtility.ToJson(TileMap, true);
        string itemjson = JsonUtility.ToJson(itemlevel, true);

        File.WriteAllText(TileMapfilePath, tilemapjson);
        File.WriteAllText(ItemfilePath, itemjson);

        i = index;
    }


    private string GetJsonSavePath(string typeName, int index, int mapEditorIndex)
    {
        string path = Application.dataPath + "/Resources/" + typeName + mapEditorIndex + "-" + index + ".json";
        return path;
    }

    private T LoadJsonData<T>(string typeName, int index, int mapEditorIndex)
    {
        string path = GetJsonSavePath(typeName,index, mapEditorIndex);
        var file = new System.IO.FileInfo(path);

        if (!file.Exists)
        {
            Debug.LogError("파일 없음: " + path);
            return default(T);
        }

        string jsonData = System.IO.File.ReadAllText(file.FullName);
        return JsonUtility.FromJson<T>(jsonData);
    }

}
