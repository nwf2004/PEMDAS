using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldObject : MonoBehaviour
{
    public int initLayer;
    public bool memorized;
    public int memLayer;
    public int viewMemReq;
    public int viewCover;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = initLayer;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void memorize()
    {
        memorized = true;
        gameObject.layer = memLayer;
    }
}
