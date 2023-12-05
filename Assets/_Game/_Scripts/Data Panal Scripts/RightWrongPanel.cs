using UnityEngine;
using TMPro;

namespace DataPanel
{
    public class RightWrongPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_Right;
        [SerializeField] TextMeshProUGUI m_Wrong;
        [SerializeField] TextMeshProUGUI m_Stars;

        public void Setup(int _right, int _wrong, int _stars)
        {
            m_Right.text = _right.ToString();
            m_Wrong.text = _wrong.ToString();
            m_Stars.text = _stars.ToString();
        }
    }
}
    