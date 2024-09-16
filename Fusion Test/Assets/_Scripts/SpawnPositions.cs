using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    public Transform[] spawnPositions;
    Dictionary<Transform, bool> _usedPositions = new Dictionary<Transform, bool>();

    private void Start()
    {
        foreach (var spawnPosition in spawnPositions)
        {
            _usedPositions.Add(spawnPosition, false);
        }
    }

    public Transform GetSpawnPosition()
    {
        foreach (var spawnPosition in spawnPositions)
        {
            if (_usedPositions[spawnPosition]) continue;

            _usedPositions[spawnPosition] = true;
            return spawnPosition;
        }

        return null;
    }
}