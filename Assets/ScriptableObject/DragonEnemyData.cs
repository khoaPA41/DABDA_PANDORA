using UnityEngine;

[CreateAssetMenu(fileName = "DragonEnemyData", menuName = "Scriptable Objects/DragonEnemyData")]
public class DragonEnemyData : ScriptableObject
{
    public string name;
    public float health;
    public float speed;
    public bool isCanShoot;
}
