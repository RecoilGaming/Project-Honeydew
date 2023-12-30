using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public PlayerUpgrade upgrade;
    private new TextMeshProUGUI name;
    private TextMeshProUGUI desc;

    // runs before scene loads
    private void Awake()
	{
        name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        desc = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetUpgrade(PlayerUpgrade upgrade)
    {
        this.upgrade = upgrade;
        name.text = upgrade.name;
        desc.text = upgrade.description;
    }
}
