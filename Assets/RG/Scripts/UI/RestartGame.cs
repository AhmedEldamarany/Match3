using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
    }
}
