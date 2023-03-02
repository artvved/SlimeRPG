using Game.Mono;
using UnityEngine;

namespace ScritableData
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public float PlayerSpeed;
        public int PlayerMaxHp;
        public int PlayerDamage;
        public float DistanceBetweenFight;
        public float PlayerRange;
        public PlayerView PlayerPrefab;
        public EnemyView EnemyPrefab;
    }
}