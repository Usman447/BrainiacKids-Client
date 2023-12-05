using UnityEngine;
using Scene_Management;

public class StartingAnimScript : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (AudioManager.Singleton != null)
        {
            AudioManager.Singleton.Play("home screen");
        }
    }

    public void OnClickStartButton()
    {
        animator.SetTrigger("Ending");
    }

    public void MoveToTheNextScene()
    {
        if (AudioManager.Singleton != null)
        {
            AudioManager.Singleton.Stop("home screen");
        }

        SceneTransition.Singleton.LoadScene("Login Scene");
    }

    public void SetStartButtonBeating()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Start Loop"), 1);
    }
}
