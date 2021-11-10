using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public float followSpeed;
    public Vector3 offset;
    private Transform cammyCam;

    // Start is called before the first frame update
    void Start()
    {
        cammyCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = transform.position + offset;
        cammyCam.position = Vector3.Lerp(cammyCam.position, targetPos, followSpeed * Time.deltaTime);
    }
}
