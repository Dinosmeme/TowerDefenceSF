using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehavior
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehavior m_AIBehavior;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLenght;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructable m_SelectetTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;


        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehavior == AIBehavior.Null)
            {

            }

            if (m_AIBehavior == AIBehavior.Patrol)
            {
                UpdateBehaviorPatrol();
            }
        }

        private void UpdateBehaviorPatrol()
        {
            ActionFindNewMovePosition();

            ActionControllShip();

            ActionFindNewAttackTarget();

            ActionFire();

            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehavior == AIBehavior.Patrol)
            {
                if(m_SelectetTarget != null)
                {
                    m_MovePosition = m_SelectetTarget.transform.position;
                }
                else
                {
                    if(m_PatrolPoint != null)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {

                            if (m_RandomizeDirectionTimer.IsFinished == true)
                            {
                                Vector2 newPoint = Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                            }

                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                }
            }
        }

        private void ActionEvadeCollision()
        {
            if(Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLenght) == true)
            {
                m_MovePosition = transform.position + transform.right * 100f; // Can be changed to choose random evade direction (don't forget timer to actually turn ;) ).      !!!
            }
        }

        private void ActionControllShip()
        {
            m_SpaceShip.ThrustControll = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45f;

        /// <summary>
        /// Returns lormalized toque value to allign ship with target
        /// </summary>
        /// <param name="targetPosition">Position of target</param>
        /// <param name="ship">Position of ship</param>
        /// <returns>Normalized torque value</returns>
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition); //Position from world to local

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward); // Signed Angle between target and forward spaceship direction

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE; //Clamped torque value angle >= 45 => torque = 1.

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectetTarget = FindNearestDestructableTarget();

                m_FindNewTargetTimer.Start(m_ShootDelay); // Same as line 173                                                         !!!
            }
        }

        private void ActionFire()
        {
            if (m_SelectetTarget != null)
            {
                if(m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FindNewTargetTimer.Start(m_ShootDelay); // Add Restart function for Time class in future                        !!!
                }
            }
        }

        private Destructable FindNearestDestructableTarget()
        {
            float maxDist = float.MaxValue;

            Destructable potentialTarget = null;

            foreach(var v in Destructable.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructable.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

    private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

        }

        #endregion

        public void SetPatrolBehavior(AIPointPatrol point)
        {
            m_AIBehavior = AIBehavior.Patrol;
            m_PatrolPoint = point;
        }
    }
}