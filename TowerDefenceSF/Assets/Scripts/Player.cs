using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{

    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private GameObject m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;

        protected override void Awake()
        {
            base.Awake();

            if(m_Ship != null)
                Destroy(m_Ship.gameObject);
        }

        private void Start()
        {
            if (m_Ship) 
            { 
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
                Respawn();
            }

        }

        private void OnShipDeath()
        {
            m_NumLives--;
            if (m_NumLives > 0)
                Respawn();
            else
                LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void Respawn()
        {
            if(LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);


                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                //m_CameraController.SetTarget(m_Ship.transform);
                //m_MovementController.SetTargetShip(m_Ship);

                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
            
        }

        protected void TakeDamage(int m_damage)
        {
            m_NumLives -= m_damage;
            if (m_NumLives <= 0)
            {
                //LevelSequenceController.Instance.FinishCurrentLevel(false);
                SceneManager.LoadScene(0);
            }
        }

        #region Score

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }



        #endregion
    }
}