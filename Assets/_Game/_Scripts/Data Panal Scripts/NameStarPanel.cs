using UnityEngine;
using TMPro;

namespace DataPanel
{
    public class NameStarPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_AlphabetName;
        [SerializeField] TextMeshProUGUI m_Stars;

        public void SetUp(string _name, int _stars)
        {
            m_AlphabetName.text = _name;
            m_Stars.text = _stars.ToString();
        }
    }
}
    