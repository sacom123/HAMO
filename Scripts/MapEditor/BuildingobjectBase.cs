using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Category
{
    Hook,
    Floor
}

[CreateAssetMenu(fileName ="Buildable", menuName ="BuildingObject/Create BuildableObject")]
public class BuildingobjectBase : ScriptableObject
{
    [SerializeField] int InstanceID2;
    [SerializeField] Category category;
    [SerializeField] TileBase tileBase;

    public int InstanceID
    {
        get
        {
            return InstanceID2;
        }
    }

    public TileBase TileBase
    {
        get
        {
            return tileBase;
        }
    }
    public Category categoryType
    {
        get
        {
            return category;
        }
    }
    
}
