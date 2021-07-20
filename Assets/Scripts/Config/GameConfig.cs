using System.Collections.Generic;
using UnityEngine;

namespace TestInvaders.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Player")]
        public Vector3 PlayerSpawnPosition = new Vector3(0, -9, 0);
        
        [Header("Fleet")]
        public Vector3 FleetSpawnPosition = Vector3.zero;
        public Vector3 FleetStartPosition = new Vector3(-12.5f, 8f, 0f);
        public Vector3 FleetSpacing = new Vector3(2.5f, -2f, 0f);
        public int FleetRows = 5;
        public int FleetColumns = 11;
        public float FleetYPositionMin = -7f;
        public float FleetStep = 1f;
        public float FleetShootCooldown = 0.5f;
        public ObjectType[] RowTypes;
        public List<ObjectScore> Scores;
        
        [Header("General")]
        public float XPositionMin = -16.5f;
        public float XPositionMax = 16.5f;
    }
}