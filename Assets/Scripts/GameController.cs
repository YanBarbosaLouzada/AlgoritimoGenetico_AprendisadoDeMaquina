using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] enemyPrefabs;
    public List<BaseEnemy> spawnedEnemies;


    public void SpawnEnemy(int index) {
        // spawnedEnemies.Add(Instantiate(enemyPrefabs[index]).GetComponent<BaseEnemy>());
        spawnedEnemies.Add(ObjectPooler.instance.InstantiateFromPool(
            index,Vector3.zero,Quaternion.identity).GetComponent<BaseEnemy>());

    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void DestroyAllEnemies() {
        foreach (BaseEnemy enemy in spawnedEnemies) {
            enemy.DestroyEnemy();

        }
        spawnedEnemies.Clear();
    }

}
