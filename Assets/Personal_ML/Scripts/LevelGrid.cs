using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class CubeFacts
{
    public Vector2 location;
    public Vector2 min;
    public Vector2 max;
}

[Serializable]
public class WalkableGround
{
    public Vector2 start;
    public Vector2 end;
    public float groundSize;
    public Vector2 min;
    public Vector2 max;
}

public class LevelGrid : MonoBehaviour
{
    [Header("Grid values")]
    [SerializeField] private Vector2 cubeSize;
    [Range(0, 1),SerializeField] private float delayUpdate;
    [SerializeField, Range(1, 20)] private int numberCubesX;
    [SerializeField, Range(1, 20)] private int numberCubesY;
    [SerializeField, Range(1, 6)] private float areaHeight;
    [SerializeField, Range(1, 4)] private float minimumPlatformWidth;

    [HideInInspector] public List<WalkableGround> walkableGround = new();
    
    private List<List<CubeFacts>> _gridList = new();
    private Vector2 _min;
    private Vector2 _max;
    private int _layerMask;

    private float _traceTime = 0.5f;

    private List<RaycastHit2D> _hitList = new();
    
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

    private void ScanForEnemies(Vector2 pos)
    {
        var enemyMask = 1 << 7;
        var result = Physics2D.BoxCastAll(pos, 
            cubeSize, 0, transform.up, cubeSize.y / 2, enemyMask);

        foreach (var e in result)
        {
          
        }
    }

    private void ScanAll()
    {
        for (var i = 0; i < numberCubesY; i++)
        {
            for (var j = 0; j < numberCubesX; j++)
            {
                var cube = _gridList[i][j];
                AdvancedTrace(new Vector2(i,j));
                ScanForEnemies(cube.location);
            }
        }
    }

    public WalkableGround GetCurrentGround(Vector2 currentPos)
    {
        foreach (var w in walkableGround)
        {
            if (currentPos.x > w.min.x && currentPos.x < w.max.x)
            {
                if (currentPos.y >= w.min.y && currentPos.y < w.max.y)
                {
                    return w;
                }
            }
        }
        return null;
    }
    
    public CubeFacts GetSquareFromPoint(Vector2 thePoint)
    {
        var mag = thePoint - _min;
        var indexX = (mag.x - (mag.x % cubeSize.x)) / cubeSize.x;
        var indexY = (mag.y - (mag.y % cubeSize.y)) / cubeSize.y;

        var square = _gridList[(int)indexY][(int)indexX];

        return square;
    }
    
    private Vector2 ScanUntilEdge(Vector2 startPoint, float increment)
    {
        var breakLoop = false;
        List<Vector2> newHits = new();
        
        while (!breakLoop)
        {
            var aHit = SingleTrace(startPoint, new Vector2(0,-1), 0.4f);
            if (!aHit) breakLoop = true;

            newHits.Add(aHit.point);
            
            startPoint += new Vector2(increment,0);
            
            if(aHit.point.x > _max.x) breakLoop = true;
            if(aHit.point.x < _min.x) breakLoop = true;
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
            var aHit = SingleTrace(topCorner, new Vector2(0,-1), cubeSize.y * numberCubesY);
            
            if (aHit)
            {
                if (!HasHitBeenRegistered(aHit.point))
                {
                    if (_hitList.SingleOrDefault(x => (int) x.point.y == (int) aHit.point.y) == default)
                    {
                        _hitList.Add(aHit);
                    }
                }
            }
            topCorner += new Vector2(increment, 0);
        }
        
        foreach (var h in _hitList)
        {
            var hitPointEnd = ScanUntilEdge(h.point + new Vector2(0,0.2f), 0.2f);
            var hitPointStart = ScanUntilEdge(h.point + new Vector2(0,0.2f), -0.2f);
            var start = hitPointStart;
            var end = hitPointEnd;

            var center = new Vector2(start.x + (end.x - start.x) / 2, start.y + areaHeight / 2);
            var size = new Vector2(end.x - start.x, areaHeight);

            if(size.x < minimumPlatformWidth) continue;
            
            if(walkableGround.Where(x => (int)x.start.x == (int)hitPointStart.x).ToArray().Length > 0)
                continue;
            
            walkableGround.Add(new WalkableGround()
            {
                start = hitPointStart,
                end = hitPointEnd,
                min = center - size / 2,
                max = center + size / 2,
                groundSize = size.x
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
        for (var i = 0; i < numberCubesY; i++)
        {
            var newList = new List<CubeFacts>((int)numberCubesX);
            for (var j = 0; j < numberCubesX; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                var minMax = GetMinMax(next, cubeSize);
                newList.Add( new CubeFacts()
                {
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
        for (var i = 0; i < numberCubesY; i++)
        {
            for (var j = 0; j < numberCubesX; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                Gizmos.DrawWireCube(next, new Vector3(cubeSize.x ,cubeSize.y));
            }
        }

        if (!Application.isPlaying) return;
      
        foreach (var w in walkableGround)
        {
            Gizmos.color = Color.magenta;
            var center = new Vector2(w.start.x + (w.end.x - w.start.x) / 2, w.start.y + areaHeight / 2);
            var size = new Vector2(w.end.x - w.start.x, areaHeight);
            Gizmos.DrawWireCube(center, size);
        }
    }
    #endif
}
