using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;

    public int playerHp;
    
    public SerializableDictionary<string, EnemyStateMachine.Data> enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
    
    public GameData()
    {
        Vector3 playerPosition = Vector3.zero;
        int playerHp = 250;
        enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
    }
    
    public EnemyStateMachine.Data GetEnemyPosition(string guid)
    {
        if (enemyPositionByGuid.TryGetValue(guid, out var data))
            return data;
        return null;
    }
}
