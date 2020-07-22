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
        public GameObject prefab;
        public bool expandPool;
    }

    public List<ObjectPoolItem> itemsToPool;

    public List<GameObject> pooledObjects { get; private set; }

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


    void InitObjectPooler()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.size; i++)
            {
                GameObject newMember = Instantiate(item.prefab);

                newMember.SetActive(false);
                item.prefab.name = item.name;
                pooledObjects.Add(newMember);
            }
        }
    }

    public static GameObject GetMember(string name)
    {

        #region Iteration
        for (int i = 0; i < Instance.pooledObjects.Count; i++)
        {

            if (Instance.pooledObjects[i] != null && 
                !Instance.pooledObjects[i].activeInHierarchy && 
                (name + "(Clone)") == Instance.pooledObjects[i].name)
            {
                return Instance.pooledObjects[i];
            }
        }
        #endregion

        foreach (ObjectPoolItem item in Instance.itemsToPool)
        {
            if (name == item.prefab.name && item.expandPool)
            {

                GameObject newMember = Instantiate(item.prefab);
                newMember.SetActive(false);
                Instance.pooledObjects.Add(newMember);
                return newMember;
            }
        }
        Debug.LogWarning("We couldn't find a prefab of this name " + name);
        return null;
    }
}
