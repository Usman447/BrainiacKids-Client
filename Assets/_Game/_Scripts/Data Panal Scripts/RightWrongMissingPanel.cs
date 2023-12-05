using UnityEngine;
using TMPro;

namespace DataPanel
{
    public class RightWrongMissingPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_Right;
        [SerializeField] TextMeshProUGUI m_Wrong;
        [SerializeField] TextMeshProUGUI m_Missing;

        public void Setup(int _right, int _wrong, int _missing)
        {
            m_Right.text = _right.ToString();
            m_Wrong.text = _wrong.ToString();
            m_Missing.text = _missing.ToString();
        }
    }
}
    