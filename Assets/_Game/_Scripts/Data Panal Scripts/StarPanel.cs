using UnityEngine;
using TMPro;

namespace DataPanel
{
    public class StarPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_Stars;

        public void Setup(int _star)
        {
            m_Stars.text = _star.ToString();
        }
    }
}
    