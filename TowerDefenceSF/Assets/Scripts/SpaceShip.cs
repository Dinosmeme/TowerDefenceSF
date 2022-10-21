using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructable
    {
        /// <summary>
        /// Mass for automatic set on rigid.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Force pushing forward.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Turning force.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Max linear velocity.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity => m_MaxLinearVelocity;

        /// <summary>
        /// Max turning velocity. In deg/sec.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        /// <summary>
        /// Preview of Ship for Menu and UI
        /// </summary>
        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImgae => m_PreviewImage;

        /// <summary>
        /// Saved link to rigid.
        /// </summary>
        private Rigidbody2D m_Rigid;

        #region Public API

        /// <summary>
        /// Controll of linear thrust. From -1.0 to +1.0
        /// </summary>
        public float ThrustControll { get; set; }

        /// <summary>
        /// Controll of turning thrust. From -1.0 to +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

           // InitOffensive();
        }


        private void FixedUpdate()
        {
            UpdateRigidBody();

           // UpdateEnergyRegen();
        }

        #endregion

        /// <summary>
        /// Adjust forces applied to a ship.
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControll * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * ( m_Thrust / m_MaxLinearVelocity )* Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

        }

        /// <summary>
        /// TODO: Replace temporary function
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true if bullets we're drawn</returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: Replace temporary function
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true if energy we're drawn</returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: Replace temporary function
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public void Fire(TurretMode mode)
        {
            return;
        }

        /*
        #region Offensive

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for (int i =0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }
        
         Offensive

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int a)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + a, 0, m_MaxAmmo);
        }

        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0)
                return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }
        public bool DrawEnergy(int count)
        {
            if (count == 0)
                return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        #endregion

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
        
        */

    }
}