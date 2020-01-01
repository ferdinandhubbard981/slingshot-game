using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveLevel : MonoBehaviour
{
    public GameObject levelLoader;
    levelLoad script;
    void Start()
    {
        script = levelLoader.GetComponent<levelLoad>();
    }

    void OnMouseDown()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); << levelNum resets (problem)
        script.levelNum += 1;
        script.LoadLevel();
    }
}
