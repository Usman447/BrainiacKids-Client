using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene_Management
{
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition _singleton;
        public static SceneTransition Singleton
        {
            get => _singleton;
            set
            {
                if (_singleton == null)
                {
                    _singleton = value;
                }
                else if (_singleton != value)
                {
                    Destroy(value);
                    Debug.Log($"{nameof(SceneTransition)} instance already exists, destroying duplicate");
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }



        public void LoadScene(int _sceneIndex, string _soundName)
        {
            if (AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play(_soundName);
            }

            StartCoroutine(LoadAsyncScene(_sceneIndex));
        }

        public void LoadScene(string _sceneName, string _soundName)
        {
            if (AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play(_soundName);
            }

            StartCoroutine(LoadAsyncScene(_sceneName));
        }

        public void LoadScene(string _sceneName)
        {
            StartCoroutine(LoadAsyncScene(_sceneName));
        }

        IEnumerator LoadAsyncScene(int _sceneIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneIndex);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        IEnumerator LoadAsyncScene(string _sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
    