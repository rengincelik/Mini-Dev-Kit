using System.Collections.Generic;
using UnityEngine;
public class GPU_Instancer:MonoBehaviour
{

    [Header("GPU Setup")]
    public ComputeShader computeShader;
    public int instanceCount;

    [Header("Instance Settings")]
    public Vector3 areaSize = new Vector3(50,0,50);
    public Vector3 rotationMin = Vector3.zero;
    public Vector3 rotationMax = new Vector3(0,360,0);
    public float scaleMin = 1f;
    public float scaleMax = 1f;
    public List<Vector3> positions = new List<Vector3>();

    [Header("Rendering")]
    public Material material;
    private ComputeBuffer matrixBuffer;
    public GameObject prefab;
    private Mesh mesh;
    private ComputeBuffer positionBuffer;


    void Awake()
    {


        if (prefab == null)
        {
            Debug.LogError("Prefab atanmadı!");
            return;
        }

        GeneratePositions();
    }


    void Start()
    {

        if(prefab == null)
        {
            Debug.LogError("Prefab atanmadı!");
            return;
        }

        MeshFilter mf = prefab.GetComponent<MeshFilter>();
        MeshRenderer mr = prefab.GetComponent<MeshRenderer>();
        if(mf == null || mr == null)
        {
            Debug.LogError("Prefab MeshFilter veya MeshRenderer içermiyor!");
            return;
        }

        mesh = mf.sharedMesh;
        material = mr.sharedMaterial;

        matrixBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 16);
        UpdateMatrices();

        if(mesh == null || material == null || computeShader == null)
        {
            Debug.LogError("Mesh, Material veya ComputeShader atanmadı!");
            return;
        }

        matrixBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 16);

        UpdateMatrices();

        if (prefab == null)
        {
            Debug.LogError("Prefab atanmadı!");
            return;
        }



        InitBuffer();
    }
    void InitBuffer()
    {
        var data = positions;
        if (data == null || data.Count == 0)
        {
            Debug.LogError("Position listesi boş!");
            return;
        }

        positionBuffer = new ComputeBuffer(data.Count, sizeof(float) * 3);
        positionBuffer.SetData(data);
        material.SetBuffer("_Positions", positionBuffer);

        Debug.Log($"[GPUInstancer] Buffer oluşturuldu. Instance sayısı: {data.Count}");
    }



    void Update()
    {
        if(matrixBuffer != null)
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
            Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, instanceCount);
        }

        if (mesh == null || material == null || positionBuffer == null) return;

        Graphics.DrawMeshInstancedProcedural(
            mesh,
            0,
            material,
            new Bounds(Vector3.zero, Vector3.one * 1000f),
            positions.Count
        );
    }
    void GeneratePositions()
    {
        positions.Clear();
        Vector3 center = transform.position;
        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2) + center.x,
                Random.Range(-areaSize.y / 2, areaSize.y / 2) + center.y,
                Random.Range(-areaSize.z / 2, areaSize.z / 2) + center.z
            );
            positions.Add(pos);
        }
    }



    void UpdateMatrices()
    {
        int kernel = computeShader.FindKernel("CSMain");
        computeShader.SetInt("instanceCount", instanceCount);
        computeShader.SetVector("areaSize", areaSize);
        computeShader.SetVector("rotationMin", rotationMin);
        computeShader.SetVector("rotationMax", rotationMax);
        computeShader.SetFloat("scaleMin", scaleMin);
        computeShader.SetFloat("scaleMax", scaleMax);
        computeShader.SetBuffer(kernel, "matrices", matrixBuffer);

        int threadGroups = Mathf.CeilToInt(instanceCount / 64f);
        computeShader.Dispatch(kernel, threadGroups, 1, 1);

        material.SetBuffer("_Matrices", matrixBuffer);
    }

    void OnDisable()
    {
        if (positionBuffer != null)
        {
            positionBuffer.Release();
            positionBuffer = null;
        }
        if(matrixBuffer != null)
        {
            matrixBuffer.Release();
            matrixBuffer = null;
        }
    }



}

