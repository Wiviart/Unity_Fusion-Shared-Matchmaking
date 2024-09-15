using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    public Transform[] spawnPositions;
    Dictionary<int, bool> _usedPositions = new Dictionary<int, bool>();

    public Vector3 GetSpawnPosition(int index)
    {
        if (index < 0 || index >= spawnPositions.Length)
        {
            Debug.LogError("Invalid spawn position index: " + index);
            return Vector3.zero;
        }

        if (_usedPositions.ContainsKey(index) && _usedPositions[index])
        {
            Debug.LogError("Spawn position already used: " + index);
            return Vector3.zero;
        }

        Debug.Log("Spawn position used: " + index);
        _usedPositions[index] = true;
        return spawnPositions[index].position;
    }
}