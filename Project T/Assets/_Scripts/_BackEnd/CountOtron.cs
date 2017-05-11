using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountOtron : MonoBehaviour
{
	void Start ()
    {
        print(GetComponent<MeshFilter>().mesh.vertexCount);
	}
}
