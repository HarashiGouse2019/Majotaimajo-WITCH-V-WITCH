using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler Instance;

    [System.Serializable]
    public class ObjectPoolItem
    {
        public string name;
        public int size;
        public GameObject projectile;
        public bool expandPool;
    }

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;
    // Start is called before the first frame update

    public int poolIndex;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitObjectPooler();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitObjectPooler()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.size; i++)
            {
                GameObject newMember = Instantiate(item.projectile);

                newMember.SetActive(false);
                item.projectile.name = item.name;
                pooledObjects.Add(newMember);
            }

        }
    }

    public GameObject GetMember(string name)
    {

        #region Iteration
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            
            if (!pooledObjects[i].activeSelf && (name + "(Clone)") == pooledObjects[i].name)
            {
                return pooledObjects[i];
            }
        }
        #endregion

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (name == item.projectile.name)
            {
                if (item.expandPool)
                {
                    Debug.Log("Ran out of members in pool. Creating more members!!!");
                    GameObject newMember = Instantiate(item.projectile);
                    newMember.SetActive(false);
                    pooledObjects.Add(newMember);
                    return newMember;
                }
            }
        }
        Debug.LogWarning("We couldn't find a prefab of this name");
        return null;
    }
}
