using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{

    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Life }
        public UpdateSource source = UpdateSource.Gold;

        private Text m_text;


        void Start() //Temporary change to Start instead of awake bc Instance is created in Awake as well as subscription in which case null reference error occurs
        {
            m_text = GetComponent<Text>();
            switch (source)
            {
                case UpdateSource.Gold: 
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.LifeUpdateSubscribe(UpdateText);
                    break;
            }

        }


        private void UpdateText(int life)
        {
            m_text.text = life.ToString();
        }
    }
}