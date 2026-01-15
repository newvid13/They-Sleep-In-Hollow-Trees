using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeSceneLoad : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene(1);
    }
}
