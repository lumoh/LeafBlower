using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeafSpawner : MonoBehaviour
{
    public Leaf LeafPrefab;
    public int NumLeaves;
    public Vector2Int Dimensions;
    public GameObject GroundObj;

    private List<Leaf> leaves;
    private List<int> removeIndices;

    private bool _roundWon;

    public static UnityEvent LeafCollectedEvent = new UnityEvent();
    public static MyIntEvent LeavesSpawned = new MyIntEvent();

    private void Awake()
    {
        GlobalEvents.StartLevel.AddListener(handleLevelStart);
    }

    // Start is called before the first frame update
    void Start()
    {
        GroundObj.transform.localScale = new Vector3(Dimensions.x, 6, Dimensions.y);
        removeIndices = new List<int>();
    }

    private void handleLevelStart()
    {
        spawnLeaves();
    }

    void spawnLeaves()
    {
        _roundWon = false;
        LeavesSpawned.Invoke(NumLeaves);

        leaves = new List<Leaf>();
        for (int i = 0; i < NumLeaves; i++)
        {
            Leaf newLeaf = Instantiate(LeafPrefab, transform);
            newLeaf.transform.position = new Vector3(Random.Range(Dimensions.x / -2f, Dimensions.x / 2f), 5f, Random.Range(Dimensions.y / -2f, Dimensions.y / 2f));
            leaves.Add(newLeaf);
        }
    }

    private void Update()
    {
        if (leaves != null)
        {
            if (leaves.Count > 0)
            {
                for (int i = leaves.Count - 1; i >= 0; i--)
                {
                    Leaf leaf = leaves[i];
                    if (leaf.transform.position.y < -5f)
                    {
                        Destroy(leaf.gameObject);
                        removeIndices.Add(i);

                        LeafCollectedEvent.Invoke();
                    }
                }

                for (int i = 0; i < removeIndices.Count; i++)
                {
                    int index = removeIndices[i];
                    leaves.RemoveAt(index);
                }

                removeIndices.Clear();

            }
            else
            {
                if(!_roundWon)
                {
                    _roundWon = true;
                    GlobalEvents.WinLevel.Invoke();
                    StartCoroutine(respawnLeaves());
                }
            }
        }
    }

    private IEnumerator respawnLeaves()
    {
        yield return new WaitForSeconds(1f);
        spawnLeaves();
    }
}
