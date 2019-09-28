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
                GameObject newMember = (GameObject)Instantiate(item.projectile);
                newMember.SetActive(false);
                item.projectile.name = item.name;
                item.expandPool = false;
                pooledObjects.Add(newMember);
            }

        }
    }

    public GameObject GetObject(string name)
    {
        #region Iteration
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && name == pooledObjects[i].name)
                return pooledObjects[i];
        }
        #endregion

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.projectile.name == name)
            {
                if (item.expandPool)
                {
                    GameObject newMember = Instantiate(item.projectile);
                    newMember.SetActive(false);
                    pooledObjects.Add(newMember);
                    return newMember;
                }
            }
        }
        Debug.Log("We couldn't find a prefab of this name");
        return null;
    }
}
