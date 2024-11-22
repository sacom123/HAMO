using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    GearObstacle,
    FallingPlatform,
    PlatFormLeft,
    PlatFormUp,
    Spike,
    MoveingObstacle,

}

[CreateAssetMenu(fileName ="BuildableItem", menuName ="BuildingObject/Create ItemObject")]
public class ItemObjectBase : ScriptableObject
{
    public int InstanceItemID;
    public ItemCategory itemCategory;
    public GameObject itemPrefab;
    public GameObject ItemImage;

    public int GetInstanceItemID
    {
        get
        {
            return InstanceItemID;
        }
    }

    public GameObject GetItemBase
    {
        get
        {
            return itemPrefab;
        }
    }

    public ItemCategory GetItemCategory
    {
        get
        {
            return itemCategory;
        }
    }
}
