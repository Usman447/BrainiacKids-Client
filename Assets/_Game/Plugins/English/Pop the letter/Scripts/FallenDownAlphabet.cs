using UnityEngine;

namespace English.PopTheLetter
{
    public class FallenDownAlphabet : MonoBehaviour
    {
        [SerializeField] float m_FallingSpeed = 10;
        [SerializeField] float m_RotationSpeed = 20;

        Rigidbody2D rb;
        CircleCollider2D circleCollider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            m_FallingSpeed = Random.Range(5, 10);
            m_RotationSpeed = Random.Range(120, 160);

            int rotationSide = (Random.Range(0, 2) == 0 ? -1 : 1);
            m_RotationSpeed *= rotationSide;
        }

        private void Update()
        {
            // Falling down
            rb.MovePosition(rb.position + new Vector2(0, -1) * m_FallingSpeed * Time.deltaTime);

            // Rotate on z-axis
            transform.Rotate(new Vector3(0, 0, m_RotationSpeed) * Time.deltaTime);

            if (Input.GetMouseButtonDown(0) &&
                !PTLManager.instance.GameEnd)
            {
                // World Touch
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (!hit)
                    return;

                if (hit.transform.tag.Equals("Alphabet") &&
                    hit.collider == this.circleCollider)
                {
                    string letter = gameObject.name[0].ToString();
                    print($"{hit.transform.name} -> {letter}");

                    if(PTLManager.instance.IsSameLetterClicked(letter))
                    {
                        PTLManager.instance.CorrectAlphabetsCount++;
                        DestoryObject();
                    }
                    else
                    {
                        PTLManager.instance.WrongAlphabetsCount++;
                        print("Wrong Letter");
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag.Equals("Dead Zone"))
            {
                if(gameObject.name[0].ToString() == PTLManager.instance.letter &&
                    !PTLManager.instance.GameEnd)
                {
                    PTLManager.instance.MissingAlphabetsCount++;
                }

                DestoryObject();
            }
        }

        public void DestoryObject()
        {
            Destroy(this.gameObject);
        }
    }
}
    