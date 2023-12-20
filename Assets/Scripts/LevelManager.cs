using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> mobs;
    public TerrainManager terrainManager;
    public PauseMenu uiManager;
    private bool setupDone = false;
    private float chanceToSpawn = 100;
    private float chanceToSpawnWizard = 40;
    public float spawnFrequency;
    public GameObject navMesh;
    int numberOfMobs = 0;
    private bool spawnAllowedTower = true;

    public void SceneSetup() {
        player.transform.position = new Vector3(terrainManager.width/2, terrainManager.height/2, 2);
        player.GetComponent<PlayerController>().respawnPoint = new Vector2(terrainManager.width/2, terrainManager.height/2);
        
        //NavMeshSurface navMeshComponent = navMesh.GetComponent<NavMeshPlus.Components.NavMeshSurface>();
        InvokeRepeating("SpawnAroundPlayer", 5.0f, spawnFrequency);

    }   

    public void VictoryConditionMet() {
        uiManager.finish();
    }

    public void mobKilled() {
        numberOfMobs--;
        chanceToSpawn = (9 * chanceToSpawn)/10;
    }


    private void SpawnAroundPlayer() {

        Vector3 playerPos = player.transform.position;
        
        List<Vector3> spawners = terrainManager.getTilesAroundPosition(playerPos, 10);

        if (spawners.Count != 0) {
            
            int spawnIndexChosen = UnityEngine.Random.Range(0, spawners.Count);
            
            float randomNumber =  UnityEngine.Random.Range(0, 100);

            if (randomNumber <= chanceToSpawn && mobs.Count > 0) {
                int mobIndex = 0;

                /*if ( UnityEngine.Random.Range(0, 100) <= chanceToSpawnWizard) {
                    mobIndex = 1;
                }*/
                
                Instantiate(mobs[mobIndex], spawners[spawnIndexChosen], Quaternion.identity);

                numberOfMobs++;
                chanceToSpawn = chanceToSpawn - (chanceToSpawn/10);
                
            }
            
        }
    }

     private IEnumerator waitForNextSpawn() {
        
        yield return new WaitForSeconds(4f);

        spawnAllowedTower = true;
    }
}
