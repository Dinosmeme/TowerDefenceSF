using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{

    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("Visual")]
        public Color color = Color.white;
        public Vector2 spriteScale = new Vector2(3, 3);
        public RuntimeAnimatorController animations;
        public Sprite Sprite;

        [Header("Enemy Paremeters")]
        public float moveSpeed = 1;
        public int hp = 1;
        public int score = 1;
        public float radius = 0.19f;
    }
}