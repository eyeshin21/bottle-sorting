using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalAssetStringID",menuName = "AssetTable/LocalAssetTableStringID",order = 1)]
public class LocalAssetTable : LocalAssetTable<string>
{
    public override void LoadGameObject(string id,Action<GameObject> resultCallback)
    {
        LoadGameObjectID(id,resultCallback);
    }
}