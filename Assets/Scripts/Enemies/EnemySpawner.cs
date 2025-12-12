using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header ("HUD")]
    [SerializeField] TextMeshProUGUI levelText;
    [Header ("Enemy Spawn")]
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Transform spawnLineTop;
    [SerializeField] Transform spawnLineBottom;

    int level = 0;
    int numberEnemies = 0;

    Vector3 lineTop, lineBottom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineTop = spawnLineTop.position;
        lineBottom = spawnLineBottom.position;
        levelText.text = "Nivel " + (level + 1);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        while (!PlayerSpaceShips.gameEnded)
        {
            numberEnemies = level * 3 + 5;
            for (int i = 0; i < numberEnemies; i++)
            {
                float t = Random.Range(0f, 1f);
                GameObject enemyToSummon = enemyPrefab[Mathf.Clamp(Random.Range(0, enemyPrefab.Length), 0, level)];
                
                Vector3 startPosition = Vector3.Lerp(lineTop, lineBottom, t);
                Instantiate(enemyToSummon, startPosition, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
            
            yield return new WaitForSeconds(5f);
            level++;
            levelText.text = "Nivel " + (level + 1);
        }
    }
}
