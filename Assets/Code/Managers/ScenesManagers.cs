using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class ScenesManagers : MonoBehaviour
    {

        public void NewGameScene(string scenename)
        {
            SceneManager.LoadScene(scenename, LoadSceneMode.Single);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}