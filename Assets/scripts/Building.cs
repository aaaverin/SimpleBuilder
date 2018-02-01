using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IFigure
{
    [SerializeField] private MeshRenderer meshRenderer;

    public Material Material
    {
        get { return meshRenderer.sharedMaterial; }
        set { meshRenderer.sharedMaterial = value; }
    }

    private void Awake()
    {
       // meshRenderer = GetComponent<MeshRenderer>();
    }

    public int Size { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
}
