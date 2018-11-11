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

[Serializable]
public class DirectionalTileRectTransform : DirectionalTile<RectTransform> { }

public class DirectionalTile<T>
{
    public T Item;
    public int Rotation;
}

public abstract class Tileset<T>
{
    public T Intersection;
    public T TJunction;
    public T Straight;
    public T Endcap;

    public DirectionalTile<T> Get(DirectionalTileType t)
    {
        if (t == DirectionalTileType.Intersection)
            return new DirectionalTile<T>() { Item = Intersection };
        else
        {
            switch (t)
            {
                case DirectionalTileType.StraightHorizontal:
                    return new DirectionalTile<T>() { Item = Straight };
                case DirectionalTileType.StraightVertical:
                    return new DirectionalTile<T>() { Item = Straight, Rotation = 90 };
                case DirectionalTileType.TJunctionTop:
                    return new DirectionalTile<T>() { Item = TJunction, Rotation = -90 };
                case DirectionalTileType.TJunctionRight:
                    return new DirectionalTile<T>() { Item = TJunction };
                case DirectionalTileType.TJunctionBottom:
                    return new DirectionalTile<T>() { Item = TJunction, Rotation = 90 };
                case DirectionalTileType.TJunctionLeft:
                    return new DirectionalTile<T>() { Item = TJunction, Rotation = 180 };
                case DirectionalTileType.EndcapTop:
                    return new DirectionalTile<T>() { Item = Endcap, Rotation = 90 };
                case DirectionalTileType.EndcapRight:
                    return new DirectionalTile<T>() { Item = Endcap, Rotation = 180 };
                case DirectionalTileType.EndcapBottom:
                    return new DirectionalTile<T>() { Item = Endcap, Rotation = -90 };
                case DirectionalTileType.EndcapLeft:
                    return new DirectionalTile<T>() { Item = Endcap };
            }

            Debug.Log("Can't find a tile for "+ t);
            return null;
        } 
    }
}

public enum TileType
{
    Intersection = 0,
    TJunction = 10,
    Straight = 20,
    Endcap = 30,
}
public enum DirectionalTileType
{
    Unknown = 0,
    Intersection = 1,
    TJunctionTop, // = 10,
    TJunctionRight,
    TJunctionBottom,
    TJunctionLeft,
    StraightVertical, // = 20,
    StraightHorizontal,
    EndcapTop, // = 30,
    EndcapRight,
    EndcapBottom,
    EndcapLeft,
}
public enum Faces
{
    Top = 0, Right = 1, Bottom = 2, Left = 3
}
public struct NeighborhoodMap
{
    public bool? Top;
    public bool? Left;
    public bool? Bottom;
    public bool? Right;
}

public static class TileTypeExtensions
{
    private static DirectionalTileType[] AllDirectionalTiles = Enum.GetValues(typeof(DirectionalTileType)) as DirectionalTileType[];
    private static Dictionary<DirectionalTileType, Faces[]> facings = new Dictionary<DirectionalTileType, Faces[]>()
    {
        { DirectionalTileType.Intersection,       new Faces[4] {Faces.Top, Faces.Right, Faces.Bottom, Faces.Left} },
        { DirectionalTileType.StraightVertical,   new Faces[2] {Faces.Top, Faces.Bottom} },
        { DirectionalTileType.StraightHorizontal, new Faces[2] {Faces.Right, Faces.Left} },
        { DirectionalTileType.TJunctionTop,       new Faces[3] {Faces.Right, Faces.Top, Faces.Left} },
        { DirectionalTileType.TJunctionRight,     new Faces[3] {Faces.Right, Faces.Bottom, Faces.Top} },
        { DirectionalTileType.TJunctionBottom,    new Faces[3] {Faces.Right, Faces.Bottom, Faces.Left} },
        { DirectionalTileType.TJunctionLeft,      new Faces[3] {Faces.Top, Faces.Bottom, Faces.Left} },
        { DirectionalTileType.EndcapTop,          new Faces[1] {Faces.Top} },
        { DirectionalTileType.EndcapRight,        new Faces[1] {Faces.Right} },
        { DirectionalTileType.EndcapBottom,       new Faces[1] {Faces.Bottom} },
        { DirectionalTileType.EndcapLeft,         new Faces[1] {Faces.Left} },
    };
    public static Faces[] GetFaces(this DirectionalTileType t)
    {
        return facings[t];
    }
    public static List<DirectionalTileType> GetValidDirectionalTiles(this NeighborhoodMap neighborhood)
    {
        var valids = AllDirectionalTiles.ToList();
        valids.Remove(DirectionalTileType.Unknown);

        if (neighborhood.Top.HasValue && neighborhood.Top.Value)
        {
            valids.Remove(DirectionalTileType.EndcapLeft);
            valids.Remove(DirectionalTileType.EndcapRight);
            valids.Remove(DirectionalTileType.EndcapBottom);
            valids.Remove(DirectionalTileType.StraightHorizontal);
            valids.Remove(DirectionalTileType.TJunctionBottom);
        }
        if (neighborhood.Right.HasValue && neighborhood.Right.Value)
        {
            valids.Remove(DirectionalTileType.EndcapTop);
            valids.Remove(DirectionalTileType.EndcapBottom);
            valids.Remove(DirectionalTileType.EndcapLeft);
            valids.Remove(DirectionalTileType.StraightVertical);
            valids.Remove(DirectionalTileType.TJunctionLeft);
        }
        if (neighborhood.Bottom.HasValue && neighborhood.Bottom.Value)
        {
            valids.Remove(DirectionalTileType.EndcapLeft);
            valids.Remove(DirectionalTileType.EndcapRight);
            valids.Remove(DirectionalTileType.EndcapTop);
            valids.Remove(DirectionalTileType.StraightHorizontal);
            valids.Remove(DirectionalTileType.TJunctionTop);
        }
        if (neighborhood.Left.HasValue && neighborhood.Left.Value)
        {
            valids.Remove(DirectionalTileType.EndcapTop);
            valids.Remove(DirectionalTileType.EndcapBottom);
            valids.Remove(DirectionalTileType.EndcapRight);
            valids.Remove(DirectionalTileType.StraightVertical);
            valids.Remove(DirectionalTileType.TJunctionRight);
        }
        if (neighborhood.Top.HasValue && !neighborhood.Top.Value)
        {
            valids.Remove(DirectionalTileType.Intersection);
            valids.Remove(DirectionalTileType.EndcapTop);
            valids.Remove(DirectionalTileType.StraightVertical);
            valids.Remove(DirectionalTileType.TJunctionTop);
            valids.Remove(DirectionalTileType.TJunctionLeft);
            valids.Remove(DirectionalTileType.TJunctionRight);
        }
        if (neighborhood.Left.HasValue && !neighborhood.Left.Value)
        {
            valids.Remove(DirectionalTileType.Intersection);
            valids.Remove(DirectionalTileType.EndcapLeft);
            valids.Remove(DirectionalTileType.StraightHorizontal);
            valids.Remove(DirectionalTileType.TJunctionLeft);
            valids.Remove(DirectionalTileType.TJunctionTop);
            valids.Remove(DirectionalTileType.TJunctionBottom);
        }
        if (neighborhood.Right.HasValue && !neighborhood.Right.Value)
        {
            valids.Remove(DirectionalTileType.Intersection);
            valids.Remove(DirectionalTileType.EndcapRight);
            valids.Remove(DirectionalTileType.StraightHorizontal);
            valids.Remove(DirectionalTileType.TJunctionRight);
            valids.Remove(DirectionalTileType.TJunctionTop);
            valids.Remove(DirectionalTileType.TJunctionBottom);
        }
        if (neighborhood.Bottom.HasValue && !neighborhood.Bottom.Value)
        {
            valids.Remove(DirectionalTileType.Intersection);
            valids.Remove(DirectionalTileType.EndcapBottom);
            valids.Remove(DirectionalTileType.StraightVertical);
            valids.Remove(DirectionalTileType.TJunctionBottom);
            valids.Remove(DirectionalTileType.TJunctionLeft);
            valids.Remove(DirectionalTileType.TJunctionRight);
        }
        return valids;
    }
    public static bool? HasFace(this DirectionalTileType t, Faces matchFace)
    {
        if (t == DirectionalTileType.Unknown)
            return null;
        else
            return facings[t].Contains(matchFace);
    }
}

public class Map
{
    public int MaxRadius { get; private set; }
    private DirectionalTileType[,] map;
    public Map(int maxRadius)
    {
        this.MaxRadius = maxRadius;
        map = new DirectionalTileType[maxRadius * 2 + 1, maxRadius * 2 + 1];
        this.SetType(0, 0, DirectionalTileType.Intersection);
    }
    public DirectionalTileType GetType(int x, int y)
    {
        return map[x + MaxRadius, y + MaxRadius];
    }
    public void SetType(int x, int y, DirectionalTileType t)
    {
        map[x + MaxRadius, y + MaxRadius] = t;
    }
    public bool IsValid(int x, int y, DirectionalTileType? t = null)
    {
        var type = t ?? GetType(x, y);
        //map[x + MaxRadius, y + MaxRadius] = t;
        return true;
    }
}

public class ProceduralMapper : MonoBehaviour {
    public Map Map = new Map(5);
    public RectTransform CityStreetPanel;
    public Transform StreetsParent;
    public TilesetRectTransform TilesetIcons;
    public TilesetTransform TilesetPrefabs;

	// Use this for initialization
	void Start () {
        StartCoroutine(BuildMap());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))
        {
            CityStreetPanel.gameObject.SetActive(!CityStreetPanel.gameObject.activeSelf);
        }
	}

    public IEnumerator BuildMap()
    {
        foreach(Vector2Int coords in GenerateOutTo(3))
        {
            DirectionalTileType newType;
            if (coords.x == 0 && coords.y == 0)
            {
                newType = DirectionalTileType.Intersection;
            }
            else
            {
                newType = GetValidTile(coords);
            }
            Map.SetType(coords.x, coords.y, newType);
            SetImage(coords, newType);
            CreateStreet(coords, newType);
            yield return new WaitForSeconds(.01f);
        }
        CityStreetPanel.rotation = Quaternion.Euler(0, 0, 45f);
    }

    private void CreateStreet(Vector2Int coords, DirectionalTileType type)
    {
        var streetBlockInfo = this.TilesetPrefabs.Get(type);
        Transform streetBlock = GameObject.Instantiate<Transform>(streetBlockInfo.Item, StreetsParent);
        streetBlock.rotation = Quaternion.Euler(0f, streetBlockInfo.Rotation, 0f);
        streetBlock.position = new Vector3(coords.x * 69.8f, 0, coords.y * 69.8f);
        streetBlock.name = coords.ToString() + type.ToString();
    }

    private DirectionalTileType GetValidTile(Vector2Int coords)
    {
        var neighborMap = new NeighborhoodMap();
        neighborMap.Left = Map.GetType(coords.x - 1, coords.y).HasFace(Faces.Right);
        neighborMap.Right = Map.GetType(coords.x + 1, coords.y).HasFace(Faces.Left);
        neighborMap.Bottom = Map.GetType(coords.x, coords.y - 1).HasFace(Faces.Top);
        neighborMap.Top = Map.GetType(coords.x, coords.y + 1).HasFace(Faces.Bottom);
        var validTiles = neighborMap.GetValidDirectionalTiles();
        
        return validTiles[UnityEngine.Random.Range(0, validTiles.Count)];
    }

    private DirectionalTileType GetRandom()
    {
        return (DirectionalTileType)UnityEngine.Random.Range(1, 11);
    }

    private void SetImage(Vector2Int coords, DirectionalTileType type)
    {
        var t = CityStreetPanel.Find(coords.ToString());
        if (t != null)
        {
            GameObject.Destroy(t);
        }
        var tile = this.TilesetIcons.Get(type);
        RectTransform r = GameObject.Instantiate<RectTransform>(tile.Item, CityStreetPanel);
        if (Math.Abs(tile.Rotation) == 90)
        {
            tile.Rotation = -tile.Rotation;
        }
        r.rotation = Quaternion.Euler(0, 0, tile.Rotation);
        r.anchoredPosition = new Vector3(coords.x * 100, coords.y * 100);
        r.name = coords.ToString() + type.ToString();
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
