using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static Manager_Attack attack;
    public static Manager_Grid grid;

    private void Start()
    {
        attack = GetComponent<Manager_Attack>();
        grid = GetComponent<Manager_Grid>();

        grid.SetupValues();
        attack.SetupValues();
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }


}
