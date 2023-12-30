using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // serialized variables
    [SerializeField] private PlayerController player;
    [SerializeField] private float baseWaveValue;
    [SerializeField] private float waveValueGrowth;
    [SerializeField] private float waveDuration;
    [SerializeField] private float waveWaitTime;
    [SerializeField] private WaveDisplay waveDisplay;
    [SerializeField] private List<Spawnable> enemies = new();

    // wave variables
    private bool waveIncreasing = false;
    private int waveNumber = 1;
    private int waveValue;
    private int waveEnemies;
    private float waveTimer;

    // spawning variables
    private float spawnTimer;
    private float spawnInterval;
    private List<GameObject> newEnemies = new();

    // runs when script loads
    private void Start()
    {
        GenerateWave();
    }

    // runs every frame
    private void Update()
    {
        if (spawnTimer <= 0) {
            if (newEnemies.Count > 0) {
                float randomAngle = Random.Range(0,2*Mathf.PI);
                Vector3 position = new(2*player.cameraSize*Mathf.Cos(randomAngle),2*player.cameraSize*Mathf.Sin(randomAngle),0);
                Instantiate(newEnemies[0], position, Quaternion.identity);
                newEnemies.RemoveAt(0);
                spawnTimer = spawnInterval;
            } else {
                waveTimer = 0;
            }
        }
        
        spawnTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;

        if (!waveIncreasing && waveTimer <= 0) {
            StartCoroutine(nameof(IncreaseWave));
        }
    }

    private void GenerateWave()
    {
        waveValue = (int) (baseWaveValue * Mathf.Pow(waveValueGrowth, 2*(waveNumber-1)));
        waveEnemies = Mathf.Min(enemies.Count, 1 + (waveNumber / 3));

        newEnemies.Clear();
        while (waveValue > 0)
        {
            int newEnemy = Random.Range(0, waveEnemies);
            if (waveValue - enemies[newEnemy].value >= 0) {
                newEnemies.Add(enemies[newEnemy].enemy);
                waveValue -= enemies[newEnemy].value;
            }
        }

        spawnInterval = waveDuration / newEnemies.Count;
        waveTimer = waveDuration;
    }

    private IEnumerator IncreaseWave()
    {
        waveIncreasing = true;
        yield return new WaitForSeconds(waveWaitTime);
        
        waveIncreasing = false;
        waveNumber += 1;
        waveDisplay.SetWave(waveNumber);
        GenerateWave();
    }
}

[System.Serializable]
public class Spawnable
{
    public GameObject enemy;
    public int value;
}