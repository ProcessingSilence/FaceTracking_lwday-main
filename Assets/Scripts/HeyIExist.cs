using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeyIExist : MonoBehaviour
{
    [HideInInspector] public GetImage textureGiver;
    // Start is called before the first frame update
    void Start()
    {
        textureGiver = GameObject.FindWithTag("TextureGiver").GetComponent<GetImage>();
        textureGiver.faceMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
