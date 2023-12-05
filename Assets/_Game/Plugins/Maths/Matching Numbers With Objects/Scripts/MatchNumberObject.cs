using UnityEngine;

namespace Maths.MatchingNumber
{
    public class MatchNumberObject : MonoBehaviour
    {
        public string number;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == number)
            {
                MatchNumberManager.Instance.CheckFriutsCounter(this);
            }
            else
            {
                MatchNumberManager.Instance.MissMatchObject(this);
            }
        }
    }
}
    