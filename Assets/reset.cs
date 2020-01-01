using UnityEngine;
using UnityEngine.SceneManagement;

public class reset : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
