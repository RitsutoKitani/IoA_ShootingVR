using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public void GoStage(int i)
    {
        SceneManager.LoadScene(i);
    }
}
