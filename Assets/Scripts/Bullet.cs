using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTime = 4f;
    public string shooterName = "";
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    
}
