using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TerrainManager : MonoBehaviour
{
    public GameObject sceneManager;
    public List<TileBase> availableTilesStandard;
    private Dictionary<int, Tile> tileset;
    private Dictionary<int, Tile> tilesetGrass;
    public List<TileBase> damageTiles;
    public List<TileBase> resourceTiles;
    public List<TileBase> walls;
    public List<TileBase> corners;
    public List<TileBase> tilesToConvert;
    public Tilemap tilemap;
    public Tilemap grassTerrain;
    public Tilemap unWalkableTerrain;
    public List<GameObject> TowerArray = new List<GameObject>();
    public List<GameObject> TowerEdgeArray;
    public int width;
    public int height;
    public float magnification = 12.0f;
    private bool obstructionAllowed = true;
    private int obstructionWait = 300;
    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<Tile>> TileGrid = new List<List<Tile>>();

    public BoundsInt test;

    void Start() {
        //numberCactusAllowed = Mathf.sqrt()
        createMap();
        LevelManager managerScript = sceneManager.GetComponent<LevelManager>();
        managerScript.SceneSetup();
    }

    void hasWon() {

        bool isComplete = true;

        for (int y = 0; y < height; y++) {

            for (int x = 0; x < width; x++) {
                
                if (!tilesToConvert.Contains(TileGrid[x][y].getTexture()) && !walls.Contains(TileGrid[x][y].getTexture()) && !corners.Contains(TileGrid[x][y].getTexture())) {
                    isComplete = false;
                    break;
                }

            }

            if (!isComplete) {
                break;
            }
        }

        if (isComplete) {
            //victory condition satisfied
            sceneManager.GetComponent<LevelManager>().VictoryConditionMet();
        }
    }

    void createMap() {
        tileset = new Dictionary<int, Tile>();
        tilesetGrass = new Dictionary<int, Tile>();
        int index = 0;

        foreach (TileBase tile in availableTilesStandard) {
            if (tile.ToString().Equals("desert_33 (UnityEngine.Tilemaps.Tile)")) {
                tileset.Add(index, new Tile(index, false, false, false, tile, false, false));
            } else {
                tileset.Add(index, new Tile(index, false, false, false, tile, true, true));
            }
            index++;
        }

        index = 0;

        foreach (TileBase tile in tilesToConvert) {
            tilesetGrass.Add(index, new Tile(index, false, false, false, tile, true, false));
            index++;
        }

        int counter = 0;

        for (int y = 0; y < height; y++) {

            TileGrid.Add(new List<Tile>());
            noiseGrid.Add(new List<int>());

            for (int x = 0; x < width; x++) {

                if (counter == obstructionWait) {
                    obstructionAllowed = true;
                    counter = 0;
                }
                
                if (!obstructionAllowed) {
                    counter++;
                }

                int tile_id = GetPerlinId(x,y, tileset.Count);
                
                if (!tileset[tile_id].getIsWalkable() && !obstructionAllowed && x > 2 && x < width-2 && y < height-2 && y > 2) {
                    while (!tileset[tile_id].getIsWalkable()) {
                        tile_id = GetPerlinId(x,y, tileset.Count);
                    }
                }
                
                if (!tileset[tile_id].getIsWalkable()) { 
                    obstructionAllowed = false;
                }

                noiseGrid[y].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }

        addWalls();
    }

    void addWalls() {
        
        List<int> rows = new List<int>{height, -1};
        List<int> columns = new List<int>{width, -1};

        //TileGrid[height-1][0] = new Tile(height*width, false, false, false, corners[3], false, false);
        unWalkableTerrain.SetTile(new Vector3Int(height, -1, 0), corners[3]);
        
        //TileGrid[height-1][width-1] = new Tile(height*width, false, false, false, corners[1], false, false);
        unWalkableTerrain.SetTile(new Vector3Int(height, width, 0), corners[1]);

        //TileGrid[0][0] = new Tile(height*width, false, false, false, corners[2], false, false);
        unWalkableTerrain.SetTile(new Vector3Int(-1, -1, 0), corners[2]);

        //TileGrid[0][width-1] = new Tile(height*width, false, false, false, corners[0], false, false);
        unWalkableTerrain.SetTile(new Vector3Int(-1, width, 0), corners[0]);
       
        foreach ( int row in rows) 
        {
            
            for (int col = 0; col < width; col++) {
                Tile wall = new Tile(height*width, false, false, false, walls[0], false, false);;

                if (row == -1) {
                    wall = new Tile(height*width, false, false, false, walls[1], false, false);
                }

                //TileGrid[row][col] = wall;
                unWalkableTerrain.SetTile(new Vector3Int(col, row, 0), wall.getTexture());
            }    
        }

        foreach ( int col in columns) 
        {
            for (int row = 0; row < width; row++) {
                
                Tile wall = new Tile(height*width, false, false, false, walls[3], false, false);

                if (col ==  -1) {
                    wall = new Tile(height*width, false, false, false, walls[2], false, false);
                }
                
                //TileGrid[row][col] = wall;
                unWalkableTerrain.SetTile(new Vector3Int(col, row, 0), wall.getTexture());
            }    
        }
    }

    public void AddTower(GameObject Tower) {
        Tower.transform.position = new Vector3((int)Tower.transform.position.x, (int)(Tower.transform.position.y), (int)(Tower.transform.position.z));
        

        if (Tower.transform.position.x >= width) {
            Tower.transform.position = new Vector3(width - 1, Tower.transform.position.y, Tower.transform.position.z);
        }

        if (Tower.transform.position.x <=  0) {
            Tower.transform.position = new Vector3(0.5f, Tower.transform.position.y, Tower.transform.position.z);
        }

        if (Tower.transform.position.y >= height) {
            Tower.transform.position = new Vector3(Tower.transform.position.x, height-1, Tower.transform.position.z);
        }

        if (Tower.transform.position.y <= 0 ) {
            Tower.transform.position = new Vector3(Tower.transform.position.x, 0.1f, Tower.transform.position.z);
        }

        bool allowed = true;
        
        foreach (GameObject entry in TowerArray) {
            if (Vector3.Distance(Tower.transform.position, entry.transform.position) <= 5) {
                allowed = false;
                Tower.SetActive(false);
                Tower.transform.position = new Vector3(width*height, width*height, Tower.transform.position.z);
                break;
            } 
        }

        if (allowed) {
            TowerArray.Add(Tower);
            updateTerrain();
        }
    }

    public void RemoveTower(GameObject tower) {
        TowerArray.Remove(tower);
        tower.transform.position = new Vector3(width*height, width*height, tower.transform.position.z);
        tower.SetActive(false);
        updateTerrain();
    }

    public List<GameObject> getTowers() {
        return TowerArray;
    }

    private void updateTerrain() {
        grassTerrain.ClearAllTiles();

        if (TowerArray.Count < 3) {
            return;
        }

        GameObject start = getRightMostLowestTower();
        Vector3 startPos = start.transform.position;

        TowerEdgeArray = new List<GameObject>();
        TowerEdgeArray.Add(start);

        Dictionary<GameObject, float> sortedPolarAngles = new Dictionary<GameObject, float>();
        
        foreach (GameObject tower in TowerArray) {
            Vector3 towerPos = tower.transform.position;
            if (tower != start) {
                sortedPolarAngles.Add(tower, Mathf.Atan2(towerPos.y - startPos.y, towerPos.x - startPos.x));
            }
        }

        sortedPolarAngles = sortedPolarAngles.OrderBy(key => key.Value).ToDictionary(pair => pair.Key, pair => pair.Value);;
        
        TowerEdgeArray.Add(sortedPolarAngles.ElementAt(0).Key);
        TowerEdgeArray.Add(sortedPolarAngles.ElementAt(1).Key);
        
        if (sortedPolarAngles.Count > 2) {
            //GameObject top = TowerEdgeArray[1];
            for (int i = 2; i < sortedPolarAngles.Count; i++) {
                
                GameObject beforePrev = TowerEdgeArray.ElementAt(TowerEdgeArray.Count - 2);
                GameObject prevPoint = TowerEdgeArray.ElementAt(TowerEdgeArray.Count - 1);
                KeyValuePair<GameObject, float> current = sortedPolarAngles.ElementAt(i);
                
                //check orientation: 0 = sameLine, 1 = clockwise, 2 = counterclockwise
                int result = ((int) prevPoint.transform.position.y - (int) beforePrev.transform.position.y) 
                            * ((int) current.Key.transform.position.x - (int) prevPoint.transform.position.x) 
                            - ((int) prevPoint.transform.position.x - (int) beforePrev.transform.position.x)
                             * ((int) current.Key.transform.position.y - (int) prevPoint.transform.position.y);

                if (result < 0) {
                    TowerEdgeArray.Add(current.Key);
                } else {
                    
                    while (result > 0) {
                        
                        TowerEdgeArray.RemoveAt(TowerEdgeArray.Count - 1)   ;
                        prevPoint = TowerEdgeArray.ElementAt(TowerEdgeArray.Count - 1);
                        beforePrev = TowerEdgeArray.ElementAt(TowerEdgeArray.Count - 2);
                        
                        result = ((int) prevPoint.transform.position.y - (int) beforePrev.transform.position.y) 
                            * ((int) current.Key.transform.position.x - (int) prevPoint.transform.position.x) 
                            - ((int) prevPoint.transform.position.x - (int) beforePrev.transform.position.x)
                             * ((int) current.Key.transform.position.y - (int) prevPoint.transform.position.y);
                    }

                    TowerEdgeArray.Add(current.Key);

                }

                /*if (sortedPolarAngles[TowerEdgeArray[TowerEdgeArray.Count - 1]] > current.Value) {
                    TowerEdgeArray[TowerEdgeArray.Count - 1] = current.Key;
                } else {
                    TowerEdgeArray.Add(current.Key);
                }*/
            }
        }

        convertTiles();
        hasWon();
    }

    private List<GameObject> getMinMaxY() {
        GameObject bottom = null;
        GameObject top = null;
        
        foreach (GameObject t in TowerEdgeArray) {
            if (bottom == null) {
                bottom = t; 
                continue;
            } 

            if (t.transform.position.y < bottom.transform.position.y ) {
                bottom = t;
            }
        }

        foreach (GameObject t in TowerEdgeArray) {
            if (top == null) {
                top = t; 
                continue;
            } 

            if (t.transform.position.y > top.transform.position.y) {
                top = t;
            }
        }

        return new List<GameObject>{top, bottom};
    }


    private List<GameObject> getMinMaxX() {
        GameObject left = null;
        GameObject right = null;
        
        foreach (GameObject t in TowerEdgeArray) {
            if (left == null) {
                left = t; 
                continue;
            } 

            if (t.transform.position.x < left.transform.position.x) {
                left = t;
            }
        }

        foreach (GameObject t in TowerEdgeArray) {
            if (right == null) {
                right = t; 
                continue;
            } 

            if (t.transform.position.x > right.transform.position.x ) {
                right = t;
            }
        }

        return new List<GameObject>{left, right};
    }

    private bool isOnLine(int x, int y) {
        GameObject prevTower = null;

        Vector3 point = new Vector3( x, y, TowerEdgeArray[0].transform.position.z);

        foreach (GameObject tower in TowerEdgeArray) {
            if (prevTower == null) {
                prevTower = tower;

                continue;
            }

            /*if ((y == prevTower.transform.position.y && x != tower.transform.position.x) || (y == tower.transform.position.y && x != prevTower.transform.position.x)) {
                if (prevTower.transform.position.x > tower.transform.position.x && (x > prevTower.transform.position.x || x < tower.transform.position.x)) {
                    return false;
                }

                if (prevTower.transform.position.x < tower.transform.position.x && (x > tower.transform.position.x || x < prevTower.transform.position.x)) {
                    return false;
                }
            }*/

            /*if (x == prevTower.transform.position.x || x == tower.transform.position.x) {
                if (prevTower.transform.position.x > tower.transform.position.x && (x > prevTower.transform.position.x || x < tower.transform.position.x)) {
                    return false;
                }

                if (prevTower.transform.position.x < tower.transform.position.x && (x > tower.transform.position.x || x < prevTower.transform.position.x)) {
                    return false;
                }
            }*/
        
            int dist1 = Mathf.RoundToInt(Vector3.Distance(point, prevTower.transform.position));
            int dist2 = Mathf.RoundToInt(Vector3.Distance(point, tower.transform.position));
            int dist3 = Mathf.RoundToInt(Vector3.Distance(prevTower.transform.position, tower.transform.position));
            
            if (dist1 + dist2 == dist3) {
                return true;
            }

            //Vector3 seg1 = prevTower.transform.position - tower.transform.position;
            //Vector3 seg2 = tower.transform.position - point;
            
           // if (Mathf.RoundToInt((seg2 - seg1 * (Vector3.Dot(seg2, seg1) / Vector3.Dot(seg1, seg1))).magnitude) == 0 /*&& (seg2 - seg1 * (Vector3.Dot(seg2, seg1) / Vector3.Dot(seg1, seg1))).magnitude >= 0*/) {
           //     return true;
            //}

            /*if (Vector3.Dot((tower.transform.position - prevTower.transform.position).normalized ,(new Vector3(x, y, tower.transform.position.z) - tower.transform.position).normalized ) < 0f && Vector3.Dot( (prevTower.transform.position - tower.transform.position).normalized , ((new Vector3(x, y, tower.transform.position.z) - prevTower.transform.position)).normalized ) < 0f) {
                return true;
            }*/

            prevTower = tower;
        }

        Vector3 segFirst = TowerEdgeArray[0].transform.position - TowerEdgeArray[TowerEdgeArray.Count - 1].transform.position;
        Vector3 segLast = TowerEdgeArray[TowerEdgeArray.Count - 1].transform.position - point;
            
        if (Mathf.RoundToInt((segLast - segFirst * (Vector3.Dot(segLast, segFirst) / Vector3.Dot(segFirst, segFirst))).magnitude) == 0) {
            return true;
        }

        return false;
    }

    private bool isInShape(int x, int y, GameObject top, GameObject down, GameObject left, GameObject right) {
        int intersection = 0;

        if (y > (int) top.transform.position.y || y < (int) down.transform.position.y || x < (int) left.transform.position.x || x > (int) right.transform.position.x) {
            return false;
        }
        
        for (int i = x; i <= (int) right.transform.position.x; i++) {
            //check for intersection
            if (isOnLine(i,y)) {
                intersection++;
                break;
            }
        }

        if (intersection == 0 ) {
            return false;
        }

        intersection = 0;

        for (int i = x; i >= (int) left.transform.position.x ; i--) {
            //check for intersection
            if (isOnLine(i,y)) {
                intersection++;
                break;
            }
        }

        if (intersection == 0) {
            return false;
        }

        return true;
    }

    private void convertTiles() {
        List<GameObject> startAndEnd = getMinMaxY();
        
        GameObject top = startAndEnd[0];
        GameObject bottom = startAndEnd[1];

        List<GameObject> leftAndRight = getMinMaxX();

        GameObject left = leftAndRight[0];
        GameObject right = leftAndRight[1];

        //Tile startingTile = TileGrid[(int) start.transform.position.y][(int)start.transform.position.x];
        //Tile endingTile = TileGrid[(int) end.transform.position.y][(int) end.transform.position.x];

        for (int y = (int) bottom.transform.position.y; y <= (int) top.transform.position.y; y++) {

            for (int x = (int) left.transform.position.x; x <= (int) right.transform.position.x; x++) {
                
                //check if one the line
               
                
                bool inShape = false;
                bool onLine = isOnLine(x, y);

                if (onLine == false) {
                   inShape = isInShape(x, y, top, bottom, left, right);
                }

                if (inShape == true || onLine == true) {
                    int tile_id = GetPerlinId(x,y,tilesetGrass.Count);

                    if (!tilesetGrass[tile_id].getIsWalkable() && !obstructionAllowed) {
                        while (!tilesetGrass[tile_id].getIsWalkable()) {
                            tile_id = GetPerlinId(x,y, tilesetGrass.Count);
                        }
                    }
                    
                    if (!tilesetGrass[tile_id].getIsWalkable()) { 
                        obstructionAllowed = false;
                    }

                    noiseGrid[y][x] = tile_id;

                    Tile chosenTile = tilesetGrass[tile_id];
                    TileGrid[y][x] = chosenTile;
                    grassTerrain.SetTile(new Vector3Int(x, y, 1), chosenTile.getTexture());
                }
            }
        }
    }

    private GameObject getRightMostLowestTower() {

        GameObject rightMostLowestTower = null;

        foreach (GameObject tower in TowerArray) {

            Vector3 towerPos = tower.transform.position;
            Vector3 rightMostLowestTowerPos = new Vector3(Mathf.Infinity,Mathf.Infinity,1);

            if (rightMostLowestTower != null) {
                rightMostLowestTowerPos = rightMostLowestTower.transform.position;
            }

            if (rightMostLowestTower == null) {
                rightMostLowestTower = tower;
                continue;
            }
            
            if (rightMostLowestTowerPos.y > towerPos.y || (rightMostLowestTowerPos.y == towerPos.y && rightMostLowestTowerPos.x > towerPos.x )) {
                rightMostLowestTower = tower;
            }
        }
        
        return rightMostLowestTower;
    }

    private int GetPerlinId(int x, int y, int arrayCount) {
        float raw_perlin = Mathf.PerlinNoise((x * Random.value), (y * Random.value));

        float clamp = Mathf.Clamp01(raw_perlin);
        float scale = clamp * arrayCount;
        if (scale == arrayCount) {
            scale = arrayCount - 1;
        }

        return Mathf.FloorToInt(scale);
    }

    private void CreateTile(int tileId, int x, int y) {
        Tile chosenTile = tileset[tileId];
        TileGrid[y].Add(chosenTile);

        if (chosenTile.getIsWalkable()) {
            tilemap.SetTile(new Vector3Int(x, y, 0), chosenTile.getTexture());
        } else {
            unWalkableTerrain.SetTile(new Vector3Int(x, y, 0), chosenTile.getTexture());
        }
    }

    public List<Vector3> getTilesAroundPosition(Vector3 position, int distance) {
        List<Vector3> tilesInRadius = new List<Vector3>();

        for (int y = 0; y < height; y++) {
            
            for (int x = 0; x < width; x++) {

                Tile current = TileGrid[y][x];

                if (current.getCanSpawn() && Vector3.Distance(new Vector3(x,y,0), position) <= distance) {
                    tilesInRadius.Add(new Vector3(x, y, 2));
                } 
            }

        }

        return tilesInRadius;

    }
}
