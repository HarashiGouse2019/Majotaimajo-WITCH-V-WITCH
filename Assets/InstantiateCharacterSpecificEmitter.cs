using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacterSpecificEmitter : MonoBehaviour
{
    [SerializeField]
    Emitter characterEmitter;

    // Start is called before the first frame update
    void OnEnable()
    {
        characterEmitter = Instantiate(characterEmitter, transform.parent);
        DontDestroyOnLoad(characterEmitter);
    }

    public void DeactivateEmitter()
    {
        characterEmitter.gameObject.SetActive(false);
    }

    public void ActivateEmitter()
    {
        characterEmitter.gameObject.SetActive(true);
    }
}
