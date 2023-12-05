using UnityEngine;

namespace Urdu.Haroof_e_Tahaji
{
    public class HTObject : MonoBehaviour
    {
        public GameObject HaroofPrefab;
        public Sprite Image;
        public Sprite[] Haroofs;

        Vector2 width, height;

        private void Awake()
        {
            int w = (Screen.width / 2) - 175;
            width = new Vector2(w - 250, -w);

            int hTop = (Screen.height / 2) - 150;
            int hBottom = (Screen.height / 2) - 400;
            height = new Vector2(hTop, hBottom);
        }

        private void Start()
        {
            for(int i = 0; i < Haroofs.Length; i++)
            {
                SpawnHaroof(Haroofs[i]);
            }
            HTManager.Singleton.CreateHaroofsList(Haroofs);
        }

        void SpawnHaroof(Sprite _haroofImage)
        {
            GameObject newHaroof = Instantiate(HaroofPrefab, transform);

            Vector3 randPos = GetRandomPosition();
            newHaroof.GetComponent<RectTransform>().localPosition = randPos;

            HTHaroof newHTHaroof = newHaroof.GetComponent<HTHaroof>();
            newHTHaroof.UpdateHaroofImage(_haroofImage);
            newHTHaroof.InitialPosition = randPos;
        }

        Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(width.y, width.x),
                Random.Range(height.x, height.y), 0);
        }

    }
}
    