using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private TerrainManager terrainData;
    public int health = 10;
    private bool isInvincible = false;
    private bool destroyed = false;
    private float invincibleDuration = 2f;
    public GameObject player;
    private float chanceToSpawn = 100;
    private float chanceToSpawnWizard = 40;
    int numberOfMobs = 0;
    public List<GameObject> mobs;

    void Awake()
    {
        terrainData = GameObject.Find("Map").GetComponent<TerrainManager>();
        terrainData.AddTower(gameObject);
        player = GameObject.Find("SpaceGuy");

        InvokeRepeating("SpawnAroundTower", 0f, 5.0f);
    }

    public void takeDamage(int amount) {

        if (isInvincible) {
            return;
        }

        if (amount > health) {
            health = 0;
        } else {
            health -= amount;
        }

        if (health != 0) {
            StartCoroutine(temporaryInvincible());
        }

        updateHealth();
    }

    void updateHealth() {

        if (health <= 0) {
            destroyed = true;
            removeTower();
        }
    }

    private IEnumerator temporaryInvincible() {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }

    public void removeTower() {
        terrainData.RemoveTower(gameObject);
    }

    private void SpawnAroundTower() {

        Vector3 playerPos = gameObject.transform.position;
        
        List<Vector3> spawners = terrainData.getTilesAroundPosition(playerPos, 10);

        if (spawners.Count != 0 && player != null && Vector3.Distance(player.transform.position, playerPos) > 5) {
            
            int spawnIndexChosen = UnityEngine.Random.Range(0, spawners.Count);
            
            float randomNumber =  UnityEngine.Random.Range(0, 100);

            if (randomNumber <= chanceToSpawn && mobs.Count > 0) {
                int mobIndex = 0;

               /* if ( UnityEngine.Random.Range(0, 100) <= chanceToSpawnWizard) {
                    mobIndex = 1;
                }*/
                
                Instantiate(mobs[mobIndex], spawners[spawnIndexChosen], Quaternion.identity);

                numberOfMobs++;
                chanceToSpawn = chanceToSpawn - (chanceToSpawn/5);
                
            }
            
        }
    }

}
