using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Destructable scene object. Can have HP.
    /// </summary>
    public class Destructable : Entity
    {
        #region Properties
        /// <summary>
        /// Object ignores damage.
        /// </summary>
        [SerializeField] private bool m_Indestructable;
        public bool IsIndestructable => m_Indestructable;

        /// <summary>
        /// Start HP value.
        /// </summary>
        [SerializeField] private int m_Hitpoints;

        /// <summary>
        /// Current HP value.
        /// </summary>
        private int m_CurrentHitPoints;
        public int Hitpoints => m_CurrentHitPoints;

        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_Hitpoints;
        }

        #endregion

        #region Public API
        /// <summary>
        /// Apply damage to object.
        /// </summary>
        /// <param name="damage">Damage applied</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructable) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();

        }

        #endregion

        /// <summary>
        /// Ovveridable destroy object event, when HP <= 0
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructable> m_AllDestructables;

        public static IReadOnlyCollection<Destructable> AllDestructibles => m_AllDestructables;

        protected virtual void OnEnable()
        {
            if (m_AllDestructables == null)
                m_AllDestructables = new HashSet<Destructable>();

            m_AllDestructables.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructables.Remove(this);
        }

        #region Teams
        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;
        #endregion

        #region Score

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        #endregion
    }
}
