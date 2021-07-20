using UnityEngine;

namespace TestInvaders.Config
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        public int MaxLives = 3;
        public int TeamId;
        public float InvincibleDuration;
        public float BlinkingPeriod;
        public float MovingSpeed = 2;
        public float ProjectileSpeed = 5;
        public float ReloadDuration = 1;
        public Vector3 ShootDirection = Vector3.up;
    }
}