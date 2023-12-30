using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // serialized variables
    [Header("General")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private List<UpgradeButton> upgradeButtons;

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
            for (int i = 0; i < upgradeButtons.Count; i++) upgradeButtons[i].SetUpgrade(upgradeManager.GetUpgrade());
            upgradesOpen = true;
        }
    }

    public void ApplyUpgrade(int id)
    {
        PlayerUpgrade upgrade = upgradeButtons[id].upgrade;
        playerController.AddStats(upgrade);
        if (upgrade.upgradeType != UpgradeType.STAT_UPGRADE) upgradeManager.RemoveUpgrade(upgrade);
        Upgrades();
    }
}
