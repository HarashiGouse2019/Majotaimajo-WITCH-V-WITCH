using Extensions;
using System.Collections.Generic;
using UnityEngine;

public class ESSequenceScript : MonoBehaviour
{
    public static List<EnemyPawn> Enemies { get; internal set; } = new List<EnemyPawn>();
    static bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        FindAllEnemyPawns().Start();
    }

    IEnumerator<bool> FindAllEnemyPawns()
    {
        while (!completed)
        {
            EnemyPawn[] childrenObjs = gameObject.GetComponentsInChildren<EnemyPawn>();
            foreach (EnemyPawn child in childrenObjs)
            {
                Enemies.Add(child);
                
            }
            yield return completed = true;
        }
    }

    public static bool Complete { get
        {
            return completed;
        } }
}
