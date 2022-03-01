using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length = 0, startpos;
    private float offSet;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        if(GetComponent<SpriteRenderer>() != null)
           length = GetComponent<SpriteRenderer>().bounds.size.x;
        offSet = startpos - cam.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp + offSet > startpos + length) startpos += length;
        else if (temp + offSet < startpos - length) startpos -= length;
    }
}