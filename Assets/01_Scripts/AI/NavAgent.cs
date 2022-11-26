using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class NavAgent : MonoBehaviour
{
    private PriorityQueue<Node> _openList;
    private List<Node> _closeList;

    private List<Vector3Int> _routePath;

    public float speed = 5f;
    public bool cornerCheck = false;
    private bool _isMove = false;
    private int _moveIdx = 0; // ���Ʈ �н��� �� ��°�� �����ϰ� �ִ���
    private Vector3 _nextPos; // ������ �̵��� ���� ������

    [SerializeField]
    private bool _isdebug = false;
    
    private Vector3Int _currentPosition; // ���� Ÿ�� ��ġ
    private Vector3Int _destination; // ��ǥ Ÿ�� ��ġ

    public Vector3Int Destination
    {
        get => _destination;
        set
        {
            SetCurrentPosition();
            _destination = value;
            CalcuRoute();
            _moveIdx = 0;
            if (_isdebug) PrintRoute();
        }
    }

    public bool CanMovePath => _routePath.Count > _moveIdx;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _openList = new PriorityQueue<Node>();
        _closeList = new List<Node>();
        _routePath = new List<Vector3Int>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        SetCurrentPosition();
        transform.position = MapManager.Instance.GetWorldPos(_currentPosition);
    }

    private void SetCurrentPosition()
    {
        Vector3Int cellPos = MapManager.Instance.GetTilePos(transform.position);
        _currentPosition = cellPos;
    }


    public Vector3Int GetNextTarget()
    {
        if(_moveIdx >= _routePath.Count)
        {
            return Vector3Int.zero;
        }

        return _routePath[_moveIdx++];
    }
 

    public void SetNextTarget() // agent movement �� �ƶ�����
    {
        if (_moveIdx >= _routePath.Count)
        {
            _isMove = false;
            return;
        }

        _currentPosition = _routePath[_moveIdx];    
        _nextPos = MapManager.Instance.GetWorldPos(_currentPosition);
        _moveIdx++;
    }

    private void PrintRoute() // ����� ��θ� ����׷� ����.
    {
        _lineRenderer.positionCount = _routePath.Count;
        _lineRenderer.SetPositions(_routePath.Select(p => MapManager.Instance.GetWorldPos(p)).ToArray());

        /*for (int i = 0; i < _routePath.Count; i++)
        {
            Vector3 worldPos = MapManager.Instance.GetWorldPos(_routePath[i]);
            _lineRenderer.SetPosition(i, worldPos);
        }*/
    }

    #region Aster �˰���

    private bool CalcuRoute()
    {
        _openList.Clear();
        _closeList.Clear();

        _openList.Push(new Node { pos = _currentPosition, _parent = null, G = 0, F = CalcH(_currentPosition) });

        bool result = false; // �� �� �ִ� ������
        int cnt = 0;  // ���� �ڵ�
        
        while (_openList.Count > 0)
        {
            Node n = _openList.Pop(); // ���� ������ �� �� �ִ� �༮�� ���� ��
            FindOpenList(n);
            _closeList.Add(n); // n�� �� �� ��������� Ŭ���� ����Ʈ�� �ֱ�
            if (n.pos == _destination) // ������ �湮�ߴ� �༮�� ��������. �׷� ������.
            {
                result = true;
                break;
            }

            // �����ڵ�
            cnt++;
            if (cnt >= 10000)
            {
                Debug.Log("while ���� �ʹ� ���� ���Ƽ� ����");
                break;
            }
        }

        if (result) // ���� ã��
        {
            _routePath.Clear();
            Node last = _closeList[_closeList.Count - 1];
            while (last._parent != null)
            {
                _routePath.Add(last.pos);
                last = last._parent;
            }
            _routePath.Reverse(); // ���� ����Ʈ�� ��������� �ٽ� ������ ������ ��
        }

        return result;
    }

    // Node n �� ����� ���� ����Ʈ�� �� ã�Ƽ� _openList�� �־���
    private void FindOpenList(Node n)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == y) continue; // �̰� ���� ��ġ�� �ڸ��ϱ� ����
                
                Vector3Int nextPos = n.pos + new Vector3Int(x, y, 0);

                Node temp = _closeList.Find(x => x.pos == nextPos); // �̹� �湮
                if (temp != null) continue;

                // Ÿ�Ͽ��� ��¥ �� �� �ִ� ������
                if (MapManager.Instance.CanMove(nextPos))
                {
                    float g = (n.pos - nextPos).magnitude + n.G;

                    Node nextOpennode = new Node { pos = nextPos, _parent = n, G = g, F = g + CalcH(nextPos) };

                    // �ֱ� ���� �˻�
                    Node exist = _openList.Contains(nextOpennode);

                    if (exist != null)
                    {
                        if (nextOpennode.G < exist.G)
                        {
                            exist.G = nextOpennode.G;
                            exist.F = nextOpennode.F;
                            exist._parent = nextOpennode._parent;
                        }
                    }
                    else
                    {
                        _openList.Push(nextOpennode);
                    }
                }
            }
        }
    }

    private float CalcH(Vector3Int pos)
    {
        // F = G + H
        Vector3Int distance = _destination - pos;
        return distance.magnitude;
    }
    #endregion
}
