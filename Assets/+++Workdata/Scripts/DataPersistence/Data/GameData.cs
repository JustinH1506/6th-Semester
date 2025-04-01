using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    public int playerHp;
    
    public SerializableDictionary<string, Vector3> enemyPositionByGuid = new SerializableDictionary<string, Vector3>();
    
    public GameData()
    {
        Vector3 playerPosition = new Vector3();
        int playerHp = 250;
        enemyPositionByGuid = new SerializableDictionary<string, Vector3>();
    }
    
    public Vector3 GetEnemyPosition(string guid)
    {
        if (enemyPositionByGuid.TryGetValue(guid, out var position))
            return position;
        return Vector3.zero;
    }
}
