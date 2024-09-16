using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    void Start()
    {
        LoadMenu();
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(ConstVariables.MENU);
    }
}
