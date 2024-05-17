using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public CharacterCreator characterCreator;

    public void OnPlayButtonPressed()
    {
        characterCreator.SaveCharacter();
        SceneManager.LoadScene("Main Hub");
    }
}
