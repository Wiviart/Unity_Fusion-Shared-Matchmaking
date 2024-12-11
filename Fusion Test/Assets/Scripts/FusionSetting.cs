using UnityEngine;

[CreateAssetMenu(fileName = "FusionSetting", menuName = "Scriptable Objects/Setting")]
public class FusionSetting : ScriptableObject
{
    public GameObject playerPrefab;
    public int maxPlayers = 2;
}