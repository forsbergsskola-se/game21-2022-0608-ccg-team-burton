using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Flags]
public enum LevelElements
{
    None = 0,
    Platform = 1,
    Ground = 2,
    Edge = 4,
    Jump = 8,
    TwoWayPass = 16,
    Gap = 32,
    TotalBlock = 64,
}

[Flags]
public enum TileOptions
{
    None = 0,
    OpenMinus = 1,
    OpenPlus = 2,
    JumpPlus = 4,
    JumpMinus = 8,
    WallPlus = 16,
    WallMinus = 32
}

[Serializable]
public class PointsOfInterest
{
    public Vector2 location;
    public LevelElements pointType;
}

[Serializable]
public class CubeFacts
{
    public Vector2 location;
    public Color color;
    public Vector2 min;
    public Vector2 max;
    public List<PointsOfInterest> pointsList = new();
    public float lowestGroundY = 9999;
    public TileOptions options;
}

[Serializable]
public class WalkableGround
{
    public Vector2 start;
    public Vector2 end;
    public float groundSize;
}

public class LevelGrid : MonoBehaviour
{
    [Header("Input values")]
    [SerializeField] private Vector2 cubeSize;
    [SerializeField] private Vector2 numberCubes;
    [Range(0, 1),SerializeField] private float delayUpdate;
    [SerializeField] private LevelMemory memory;
    [SerializeField] private GameObject spawnablePointOfInterest;
    
    private List<List<CubeFacts>> _gridList = new();
    private Vector2 _min;
    private Vector2 _max;
    private int _layerMask;

    private float _traceTime = 20f;

    private List<RaycastHit2D> _hitList = new();

    public List<WalkableGround> walkableGround = new();

    private float _maxJumpDistance = 6f;

    private void Awake()
    {
        _layerMask = 1 << 6 | 1 << 10 | 1 << 11;
        SetNewList();
    }

    void Start()
    {
        ScanAll();
    }

    private void ScanAll()
    {
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var cube = _gridList[i][j];
                var start = cube.location + new Vector2(-1,-1f) * new Vector2(cubeSize.x / 2, cubeSize.y / 2);
                AdvancedTrace(new Vector2(i,j));
            }
        }
        SpawnPointsOfInterest();
    }


    private void SpawnPointsOfInterest()
    {
        foreach (var p in walkableGround)
        {
            Instantiate(spawnablePointOfInterest, p.end, Quaternion.identity, transform);
            Instantiate(spawnablePointOfInterest, p.start, Quaternion.identity, transform);
        }
    }
    
    public CubeFacts GetSquareFromPoint(Vector2 thePoint)
    {
        var mag = thePoint - _min;
        var indexX = (mag.x - (mag.x % cubeSize.x)) / cubeSize.x;
        var indexY = (mag.y - (mag.y % cubeSize.y)) / cubeSize.y;

        var square = _gridList[(int)indexY][(int)indexX];

        return square;
    }

    private Vector2 ScanUntilEdge(Vector2 startPoint)
    {
        var breakLoop = false;
        
        List<Vector2> newHits = new();

        while (!breakLoop)
        {
            var aHit = SingleTrace(startPoint, new Vector2(0,-1), 0.4f);
            if (!aHit) breakLoop = true;

            startPoint += new Vector2(0.5f,0);
            
            newHits.Add(aHit.point);
        }

        return newHits[^2];
    }

    private bool HasHitBeenRegistered(Vector2 point)
    {
        foreach (var g in walkableGround)
        {
            if (!(point.x > g.start.x) || !(point.x < g.end.x)) continue;
            
            if ((int) point.y == (int) g.start.y)
            {
                return true;
            }
        }
        return false;
    }
    
    private void AdvancedTrace(Vector2 startIndex)
    {
        _hitList.Clear();
        var cube = _gridList[(int) startIndex.x][(int) startIndex.y];
        var topCorner = cube.location + new Vector2(-cubeSize.x / 2, cubeSize.y / 2);
        var numberTraces = 15 ;
        var increment = cubeSize.x  / numberTraces;
        
        for (var i = 0; i < numberTraces; i++)
        {
            var aHit = SingleTrace(topCorner, new Vector2(0,-1), cubeSize.y * numberCubes.y);
            
            if (aHit)
            {
                if (!HasHitBeenRegistered(aHit.point))
                {
                    if (_hitList.SingleOrDefault(x => (int) x.point.y == (int) aHit.point.y) == default)
                    {
                        _hitList.Add(aHit);
                        cube.pointsList.Add(new PointsOfInterest()
                        {
                            location = aHit.point,
                            pointType = LevelElements.Edge
                        });
                    }
                }
            }
            topCorner += new Vector2(increment, 0);
        }
        
        foreach (var h in _hitList)
        {
            var hitPoint = ScanUntilEdge(h.point + new Vector2(0,0.2f));
            
            walkableGround.Add(new WalkableGround()
            {
                start = h.point,
                end = hitPoint
            });

            cube.pointsList.Add(new PointsOfInterest()
            {
                location = hitPoint,
                pointType = LevelElements.Edge
            });
        }
    }
    
    private RaycastHit2D SingleTrace(Vector2 startPos, Vector2 traceDir, float traceLength)
    {
        var hit = Physics2D.Raycast(startPos, traceDir, traceLength, _layerMask);
        
        if (hit)
        {
            Debug.DrawLine(startPos, hit.point, Color.green, _traceTime);
        }
        else
        {
            Debug.DrawLine(startPos, startPos + traceDir *traceLength, Color.red, _traceTime);
        }
        
        return hit;
    }
    
    private void SetNewList()
    {
        for (var i = 0; i < numberCubes.y; i++)
        {
            var newList = new List<CubeFacts>((int)numberCubes.x);
            for (var j = 0; j < numberCubes.x; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                var minMax = GetMinMax(next, cubeSize);
                newList.Add( new CubeFacts()
                {
                    color = Color.red,
                    location = next,
                    min = minMax.Item1,
                    max = minMax.Item2,
                });
            }
            _gridList.Add(newList);
        }
 
        _max = _gridList[^1][^1].location + cubeSize / 2;
        _min = _gridList[0][0].location - cubeSize / 2;
    }
    
    private Tuple<Vector2, Vector2> GetMinMax(Vector2 location, Vector2 size)
    {
        var min = location - new Vector2(size.x  / 2, size.y / 2);
        var max = location + new Vector2(size.x  / 2, size.y / 2);

        return new Tuple<Vector2, Vector2>(min, max);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                Gizmos.DrawWireCube(next, new Vector3(cubeSize.x ,cubeSize.y));
            }
        }

        if (!Application.isPlaying) return;
        
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var cube = _gridList[i][j];

                foreach (var p in cube.pointsList)
                {
                    if (p.pointType.HasFlag(LevelElements.Edge))
                    {
                        Gizmos.color = Color.yellow;
                    }
                    
                    else if (p.pointType.HasFlag(LevelElements.Gap))
                    {
                        Gizmos.color = Color.red;
                    }

                    else
                    {
                        Gizmos.color = Color.black;
                    }

                    Gizmos.DrawWireSphere(p.location, 0.3f);
                }

                foreach (var w in walkableGround)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(w.start, w.end);
                }
                
            }
        }
        
    }
    #endif
}
