using ScrollView;
using UnityEngine;
using NetworkDataManager;
using TMPro;

namespace DataPanel
{
    public class StudentDataSetup : MonoBehaviour
    {
        [SerializeField] GameObject GameNamePanel;
        [SerializeField] GameObject NameStarPanel;
        [SerializeField] GameObject StarPanel;
        [SerializeField] GameObject RightWrongStarPanel;
        [SerializeField] GameObject RightWrongMissingPanel;
        [SerializeField] Transform ContentTrans;
        [SerializeField] GameObject ReturnPanel;

        ScrollManager scrollManager;

        private void Awake()
        {
            scrollManager = GetComponentInChildren<ScrollManager>();
        }

        public void OnClickBackButton()
        {
            /*if (AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play("Back");
            }*/

            foreach (Transform content in ContentTrans)
            {
                Destroy(content.gameObject);
            }
            ReturnPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void SetupData(Data data)
        {
            /////////////////////// ENGLISH ///////////////////////
            EnglishDataSetup(data);

            /////////////////////// MATHS ///////////////////////
            MathsDataSetup(data);

            /////////////////////// URDU ///////////////////////
            UrduDataSetup(data);
        }

        void EnglishDataSetup(Data data)
        {
            GameObject obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "English";

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Tracking";

            foreach (TypeNameData d in data.English.Tracking)
            {
                obj = scrollManager.AddToFront(NameStarPanel);
                NameStarPanel p = obj.GetComponent<NameStarPanel>();
                p.SetUp(d.Name, d.Stars);
            }

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Match Letters";

            obj = scrollManager.AddToFront(StarPanel);
            StarPanel starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.English.MatchLetters.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Guess Letters";

            obj = scrollManager.AddToFront(StarPanel);
            starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.English.GuessLetters.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Pop The Letters";

            obj = scrollManager.AddToFront(RightWrongMissingPanel);
            RightWrongMissingPanel rightWrongMissingPanel = obj.GetComponent<RightWrongMissingPanel>();
            rightWrongMissingPanel.Setup(data.English.PopTheLetters.Right,
                data.English.PopTheLetters.Wrong, data.English.PopTheLetters.Missing);
        }

        void MathsDataSetup(Data data)
        {
            GameObject obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Maths";

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Counting";

            obj = scrollManager.AddToFront(StarPanel);
            StarPanel starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.Maths.Counting.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Sorting";

            obj = scrollManager.AddToFront(StarPanel);
            starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.Maths.Sorting.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Matching";

            obj = scrollManager.AddToFront(RightWrongStarPanel);
            RightWrongPanel rightWrongMissingPanel = obj.GetComponent<RightWrongPanel>();
            rightWrongMissingPanel.Setup(data.Maths.Matching.Right,
                data.Maths.Matching.Wrong, data.Maths.Matching.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Addition Subtraction";

            obj = scrollManager.AddToFront(StarPanel);
            starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.Maths.AdditionSubtraction.Stars);
        }

        void UrduDataSetup(Data data)
        {
            GameObject obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Urdu";

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Haroof-e-Tahaji";

            obj = scrollManager.AddToFront(StarPanel);
            StarPanel starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.Urdu.HaroofETahaji.Stars);

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Haroof Likhiye";

            foreach (TypeNameData d in data.Urdu.HaroofLikhiye)
            {
                obj = scrollManager.AddToFront(NameStarPanel);
                NameStarPanel p = obj.GetComponent<NameStarPanel>();
                p.SetUp(d.Name, d.Stars);
            }

            obj = scrollManager.AddToFront(GameNamePanel);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "Haroof Milaye";

            obj = scrollManager.AddToFront(StarPanel);
            starPanel = obj.GetComponent<StarPanel>();
            starPanel.Setup(data.Urdu.HaroofMilaye.Stars);
        }
    }
}
    