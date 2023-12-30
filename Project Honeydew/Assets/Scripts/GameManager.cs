using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // serialized variables
    [Header("General")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject upgradePanel;
    private bool gamePaused = false;
    private bool upgradesOpen = false;

    // runs before scene loads
    private void Awake()
	{
        pausePanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    // runs every frame
    private void Update()
    {
        if (playerController.Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Pause()
    {
        if (!upgradesOpen) {
            if (gamePaused) {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
                gamePaused = false;
            } else {
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
                gamePaused = true;
            }
        }
    }

    public void Upgrades()
    {
        if (gamePaused) Pause();

        if (upgradesOpen) {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f;
            upgradesOpen = false;
        } else {
            Time.timeScale = 0f;
            upgradePanel.SetActive(true);
            upgradesOpen = true;
        }
    }
}
