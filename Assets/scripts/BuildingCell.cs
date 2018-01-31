using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public Material Material
    {
        get { return meshRenderer.sharedMaterial; }
        set { meshRenderer.sharedMaterial = value; }
    }

    public bool Busy { get; set; }
}
