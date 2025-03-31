using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 hotspot;

    public GameObject menu;
    bool menuactive = false;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && menuactive == false)
        {
            menu.SetActive(true);
            menuactive = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuactive == true)
        {
            menu.SetActive(false);
            menuactive = false;
            Time.timeScale = 1;
        }
    }

    public void backtomenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quittoDesktop()
    {
        Application.Quit();
    }

    public void resume()
    {
        menu.SetActive(false);
        menuactive = false;
        Time.timeScale = 1;
    }
}
