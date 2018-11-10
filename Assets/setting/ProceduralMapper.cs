using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TilesetRectTransform : Tileset<RectTransform> { }

[Serializable]
public class TilesetTransform : Tileset<Transform> { }

public class Tileset<T>
{
    public T Intersection;
    public T TJunction;
    public T Straight;
    public T Endcap;

    public T Get(TileType t)
    {
        switch (t)
        {
            case TileType.TJunction:
                return TJunction;
            case TileType.Straight:
                return Straight;
            case TileType.Endcap:
                return Endcap;
            default:
                return Intersection;
        }
    }
}

public enum TileType
{
    Intersection,
    TJunction,
    Straight,
    Endcap
}

public class Map
{
    public int MaxRadius { get; private set; }
    private TileType[,] map;
    public Map(int maxRadius)
    {
        this.MaxRadius = maxRadius;
        map = new TileType[maxRadius * 2 + 1, maxRadius * 2 + 1];
        this.SetType(0, 0, TileType.Intersection);
    }
    public TileType GetType(int x, int y)
    {
        return map[x + MaxRadius, y + MaxRadius];
    }
    public void SetType(int x, int y, TileType t)
    {
        map[x + MaxRadius, y + MaxRadius] = t;
    }
}

public class ProceduralMapper : MonoBehaviour {
    public Map Map = new Map(5);
    public RectTransform CityStreetPanel;
    public TilesetRectTransform TilesetIcons;
    public TilesetTransform TilesetPrefabs;

	// Use this for initialization
	void Start () {
        StartCoroutine(BuildMap());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator BuildMap ()
    {
        foreach(Vector2Int coords in GenerateOutTo(3))
        {
            TileType newType = GetRandom();
            if (coords.x == 0 && coords.y == 0)
            {
                newType = TileType.Intersection;
            }
            Map.SetType(coords.x, coords.y, newType);
            SetImage(coords, newType);
            yield return new WaitForSeconds(.15f);
        }
    }

    private TileType GetRandom()
    {
        return (TileType)UnityEngine.Random.Range(0, 5);
    }

    private void SetImage(Vector2Int coords, TileType type)
    {
        var t = CityStreetPanel.Find(coords.ToString());
        if (t != null)
        {
            GameObject.Destroy(t);
        }
        RectTransform r = GameObject.Instantiate<RectTransform>(this.TilesetIcons.Get(type), CityStreetPanel);
        r.anchoredPosition = new Vector3(coords.x * 100, coords.y * 100);
        r.name = coords.ToString();
    }

    public static IEnumerable<Vector2Int> GenerateOutTo(int radius)
    {
        //TODO trap negative radius.  0 is ok.

        foreach (int r in Enumerable.Range(0, radius + 1))
        {
            foreach (Vector2Int coord in GenerateRing(r))
            {
                yield return coord;
            }
        }
    }
    public static IEnumerable<Vector2Int> GenerateRing(int radius)
    {
        //TODO trap negative radius.  0 is ok.

        Vector2Int currentPoint = new Vector2Int(radius, 0);
        yield return new Vector2Int(currentPoint.x, currentPoint.y);

        //move up while we can
        while (currentPoint.y < radius)
        {
            currentPoint.y += 1;
            yield return new Vector2Int(currentPoint.x, currentPoint.y);
        }
        //move left while we can
        while (-radius < currentPoint.x)
        {
            currentPoint.x -= 1;
            yield return new Vector2Int(currentPoint.x, currentPoint.y);
        }
        //move down while we can
        while (-radius < currentPoint.y)
        {
            currentPoint.y -= 1;
            yield return new Vector2Int(currentPoint.x, currentPoint.y);
        }
        //move right while we can
        while (currentPoint.x < radius)
        {
            currentPoint.x += 1;
            yield return new Vector2Int(currentPoint.x, currentPoint.y);
        }
        //move up while we can
        while (currentPoint.y < -1)
        {
            currentPoint.y += 1;
            yield return new Vector2Int(currentPoint.x, currentPoint.y);
        }
    }
}
