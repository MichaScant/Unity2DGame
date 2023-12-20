using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile
{
    private int id;
    private bool doesHarm;
    private bool isResource;
    private bool isConverted;
    private TileBase texture;
    private bool isWalkable;
    private bool canSpawn;

    public Tile(int initId, bool initDoesHarm, bool initIsResource, bool initIsConverted, TileBase initTexture, bool initIsWalkable, bool initCanSpawn) {
        id = initId;
        doesHarm = initDoesHarm;
        isResource = initIsResource;
        isConverted = initIsConverted;
        texture = initTexture;
        isWalkable = initIsWalkable;
        canSpawn = initCanSpawn;
    }

    public int getId() {
        return id;
    }

    public void setId(int newId) {
        id = id;
    }

    public bool getDoesHarm() {
        return doesHarm;
    }

    public void setDoesHarm(bool newDoesHarm) {
        doesHarm = newDoesHarm;
    }

    public bool getIsResource() {
        return isResource;
    }

    public void setIsResource(bool newIsResource) {
        isResource = newIsResource;
    }

    public bool getIsConverted() {
        return isConverted;
    }

    public void setIsConverted(bool newIsConverted) {
        isConverted = newIsConverted;
    }

    public TileBase getTexture() {
        return texture;
    }

    public void setTexture(TileBase newTexture) {
        texture = newTexture;
    }

    public bool getIsWalkable() {
        return isWalkable;
    }

    public void setIsWalkable(bool newIsWalkable) {
        isWalkable = newIsWalkable;
    }

    public bool getCanSpawn() {
        return canSpawn;
    }

    public void setCanSpawn(bool newCanSpawn) {
        canSpawn = newCanSpawn;
    }

}