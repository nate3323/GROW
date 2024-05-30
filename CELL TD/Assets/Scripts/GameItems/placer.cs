using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class placer : MonoBehaviour
{
    public GameObject tower;
    public TowerInfo_Base info;

    private Camera mainCamera;

    [SerializeField]
    private Material highlightMat;

    void Start()
    {
        mainCamera = Camera.main;
        MeshFilter[] sourceMeshFilters = tower.GetComponentsInChildren<MeshFilter>();

        // Iterate through all the MeshFilters found
        foreach (MeshFilter sourceMeshFilter in sourceMeshFilters)
        {
            //Parent
            GameObject newModel = new GameObject("part");
            newModel.transform.parent = transform;

            //Add Components
            newModel.AddComponent<MeshFilter>();
            newModel.AddComponent<MeshRenderer>();

            //Set Components
            newModel.GetComponent<MeshFilter>().sharedMesh = sourceMeshFilter.sharedMesh;
            newModel.GetComponent<MeshFilter>().transform.localScale = sourceMeshFilter.transform.localScale;
            for (int i = 0; i < newModel.AddComponent<MeshRenderer>().materials.Length; i++)
            {
                newModel.AddComponent<MeshRenderer>().materials[i] = highlightMat;
            }
        }
    }

    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 worldMousePosition = hitInfo.point;

            transform.position = worldMousePosition;
        }
    }
}
