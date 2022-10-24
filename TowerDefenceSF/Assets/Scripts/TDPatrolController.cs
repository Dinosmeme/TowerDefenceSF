using UnityEngine;
using UnityEngine.Events;
using SpaceShooter;



namespace TowerDefence
{

    public class TDPatrolController : AIController
    {
        private Path m_path;
        private int pathIndex;
        [SerializeField] private UnityEvent OnEndPath;

        public void SetPath(Path newPath)
        {
            m_path = newPath;
            pathIndex = 0;
            SetPatrolBehavior(m_path[pathIndex]);
        }

        protected override void GetNewPoint()
        {
            if (m_path.Lenght > ++pathIndex)
            {
                SetPatrolBehavior(m_path[pathIndex]);
            }
            else
            {
                OnEndPath.Invoke();
                Destroy(gameObject);
            }
        }
    }
}