// using UnityEngine;
// using UnityEditor;
// using Unity.Collections;

// public class PlaceFences : EditorWindow
// {
//     private Terrain terrain;
//     private GameObject fencePrefab;
//     private GameObject fenceParent;
//     private Vector3 startPos;
//     private float width;
//     private float length;
    

//     private void PlaceFences()
//     {
//         TerrainData terrainData = terrain.terrainData;
//         Vector3 terrainPosition = terrain.GetPosition();

//         float x = startPos.x;
//         float y = startPos.y;
//         float z = DetermineHeight(x, y, terrainData, terrainPosition);
//         float endX = startPos.x - (fencePrefab.transform.localScale.x / 2);
//         float endY = startPos.y - (fencePrefab.transform.localScale.y / 2);
//         float endZ = DetermineHeight(endX, endY, terrainData, terrainPosition);
//         Vector3 endPos = new Vector3(endX, endY, endZ);


//     }

//     private float DetermineHeight(float x, float y, TerrainData data, Vector3 pos)
//     {
        
//         float z;

//         y = data.GetInterpolatedHeight(
//             pos.x + x,
//             pos.x + y
//         );

//         return y;
//     }
// }
