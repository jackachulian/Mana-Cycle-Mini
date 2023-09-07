using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton class used to store / serialize data that can be used at any time
[CreateAssetMenu(fileName = "Storage Asset", menuName = "ManaCycle/StorageAsset")]
public class StorageAsset : ScriptableObject
{
    public static StorageAsset Instance;

    // set instance so asset can be statically referenced
    void OnEnable()
    {
        Instance = this;
    }

    // variables to be stored
    public List<ManaSkins> manaSkins;

}
