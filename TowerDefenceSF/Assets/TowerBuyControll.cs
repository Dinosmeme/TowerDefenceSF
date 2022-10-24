using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControll : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_ta;
        [SerializeField] private Text m_text;
        [SerializeField] private Button m_button;
        [SerializeField] private Transform buildSite;
        public void SetBuildSite(Transform value)
        {
            buildSite = value;
        }


        private void Start() // Temporary Start instead of Awake same as in TextUpdate
        {
            TDPlayer.GoldUpdateSubscribe(GoldStatusCheck);
            m_text.text = m_ta.goldCost.ToString();
            m_button.GetComponent<Image>().sprite = m_ta.GUISprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if (gold > m_ta.goldCost != m_button.interactable)
            {
                m_button.interactable = !m_button.interactable;
                m_text.color = m_button.interactable ? Color.white : Color.red; // TODO : Change images instead of color
            }
        }

        // TODO : If player have enough money
        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_ta, buildSite);
            TowerDefence.BuildSite.HideControls();
        }
    }
}