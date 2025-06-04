using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public bool canreciveinput;
    public bool inputrecived;
    public bool ignoreAttackInput = false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("CombatManager initialized.");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        canreciveinput = true;
    }

    public void InputManager()
    {
        if(!canreciveinput)
        {
            canreciveinput = true;
        }
        else
        {
            canreciveinput = false;
        }
    }
}
