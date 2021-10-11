using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] AudioClip negativeSound;
    [SerializeField] AudioClip positiveSound;


    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator NegativeSound()
    {
        AudioSource.PlayClipAtPoint(negativeSound, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(2);
    }
    IEnumerator PositiveSound()
    {
        AudioSource.PlayClipAtPoint(positiveSound, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(2);
    }
}
