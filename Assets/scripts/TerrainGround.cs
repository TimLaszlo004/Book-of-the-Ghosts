using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class NoiseLayer{
    public float noiseScale;
    public float noiseSample;
}

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class TerrainGround : MonoBehaviour
{
    float distance = 100f;
    [Header("Area")]
    public float xSize = 500f;
    public float zSize = 500f;
    public int xPoints = 50;
    public int zPoints = 50;

    [Header("Height")]
    public float maxStep = 1.2f;
    public float maxHeight = 1000f;
    public float minHeight = 0f;
    public Vector2 seedOffset = new Vector2(0, 0);
    public List<NoiseLayer> noiseLayers = new List<NoiseLayer>();
    private float calcMax = 0f;
    // public float noiseScale = 2f;
    // public float noiseSample = 0.3f;

    [Header("Render Optimalisation")]
    //public int chunks = 3;
    //public int groups = 3;
    public GameObject player;
    public bool isGroupped = false;
    public bool isUpdate = true;
    public float quickAbort = 100f;
    public List<float> dists = new List<float>();
    public List<float> freqs = new List<float>();
    public List<float> ratios = new List<float>();

    
    [Header("Color")]
    public float textureScale = 1f;

    Mesh mesh;
    List<Vector3> verts = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> tris = new List<int>();


    float Distance(){
        if(player == null){
            return 0f;
        }
        float posX = transform.position.x - player.transform.position.x;
        float posZ = transform.position.z - player.transform.position.z;
        if(posX < 0){
            posX = -posX;
        }
        if(posZ < 0){
            posZ = -posZ;
        }
        float max = posX;
        if(posZ > posX){
            max = posZ;
        }
        return (posX + posZ + max)*0.5f;
    }

    public float Start()
    {
    
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Draw(xPoints, zPoints);
        if(isUpdate){
            StartCoroutine(updater());
        }
        return (calcMax > maxHeight)? maxHeight: calcMax;
    }

    // public void Optimizer(){
    //     if(isUpdate){
    //         StartCoroutine(updater());
    //     }
    // }

    IEnumerator updater(){
        yield return new WaitForSeconds(0.5f);
        while(isUpdate){
            float dist = Distance();
            float updateFrequency = 50f;
            if(Mathf.Abs(distance-dist) > quickAbort){
                distance = dist;
                updateFrequency = dists[dists.Count-1];
                if(dist > dists[dists.Count-1]){
                    updateFrequency = freqs[dists.Count-1];
                    Draw((int)(ratios[dists.Count-1] * xPoints), (int)(ratios[dists.Count-1] * zPoints));
                }
                else{
                    for(int i = 0; i < dists.Count; i++){
                        if(dist < dists[i]){
                            updateFrequency = freqs[i];
                            Draw((int)(ratios[i] * xPoints), (int)(ratios[i] * zPoints));
                            break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(updateFrequency + Random.value-0.5f);
        }
    }

    public void Draw(int xP = 10, int zP = 10){
        verts.Clear();
        tris.Clear();
        uvs.Clear();
        calcMax = minHeight;

        float _xstep = xSize/xP;
        float _zstep = zSize/zP;
        float minH = 0f;
        for(int i = 0, z = 0; z<=zP; z++){
            for(int x = 0; x<=xP; x++){
                float h = 0f;
                for(int j = 0; j < noiseLayers.Count; j++){
                    h += Mathf.PerlinNoise((transform.position.x + x*_xstep)*noiseLayers[j].noiseSample + seedOffset.x, (transform.position.z + z*_zstep)*noiseLayers[j].noiseSample+ seedOffset.y) * noiseLayers[j].noiseScale;
                }
                if(i == 0){minH = h;}
                else if(h<minH){minH = h;}
                if(h > calcMax){calcMax = h;}
                verts.Add(new Vector3(x*_xstep, h, z*_zstep));
                uvs.Add(new Vector2(x*textureScale, z*textureScale));

                if(z >0){
                    if(x >0 && x<xP){
                        tris.Add(i-1-xP);tris.Add(i-2-xP);tris.Add(i);
                        tris.Add(i-1-xP);tris.Add(i);tris.Add(i+1);
                    }
                    else if(x == xP){
                        tris.Add(i-1-xP);tris.Add(i-2-xP);tris.Add(i);
                    }
                    else{//x == 0
                        tris.Add(i-1-xP);tris.Add(i);tris.Add(i+1);
                    }
                }
                
                i++;
            }
        }
        if(isGroupped){
            transform.parent.GetComponent<TerrainParent>().mins.Add(minH);
            minH = 0f;
        }
        for(int i = 0; i<verts.Count; i++){
            verts[i] = new Vector3(verts[i].x-xSize/2, Stepper(i, minH, xP, zP), verts[i].z-zSize/2);

        }


        UpdateMesh(mesh, verts.ToArray(), tris.ToArray(), uvs.ToArray());
    }

    public void Ystepper(float h){
        for(int i = 0; i< verts.Count; i++){
            verts[i]  = new Vector3(verts[i].x, verts[i].y-h, verts[i].z);
        }
        UpdateMesh(mesh, verts.ToArray(), tris.ToArray(), uvs.ToArray());
    }

    public void UpdateMesh(Mesh mesh, Vector3[] vertices, int[] triangles, Vector2[] UV){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UV;
        mesh.Optimize();

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
    }

    float Stepper(int i, float minH, int xP, int zP){
        float height = verts[i].y-minH;
        if((i+1)%(xP+1) != 0){
            if(height-(verts[i+1].y-minH) > maxStep){
                height = verts[i+1].y-minH+maxStep;
            }
            else if((verts[i+1].y-minH)-height > maxStep){
                //height = verts[i+1].y-minH-maxStep;
            }
        }
        if((i+1)%(xP+1) != 1){
            if(height-(verts[i-1].y) > maxStep){
                height = verts[i-1].y+maxStep;
            }
            else if((verts[i-1].y)-height > maxStep){
                //height = verts[i-1].y-maxStep;
            }
        }
        if(i>xP){
            if(height-(verts[i-xP-1].y) > maxStep){
                height = verts[i-xP-1].y+maxStep;
            }
            else if((verts[i-xP-1].y)-height > maxStep){
                //height = verts[i-xP-1].y-maxStep;
            }
        }
        if(i<(xP+1)*(zP+1)-xP-1){
            if(height-(verts[i+xP+1].y-minH) > maxStep){
                height = verts[i+xP+1].y-minH+maxStep;
            }
            else if((verts[i+xP+1].y)-minH-height > maxStep){
                //height = verts[i+xP+1].y-minH-maxStep;
            }
        }

        if(height < minHeight){height = minHeight;}
        if(height > maxHeight){height = maxHeight;}
        return height;
    }

    public float GetHeight(float x, float z){
        // float h = 0;
        // for(int j = 0; j < noiseLayers.Count; j++){
        //     h += Mathf.PerlinNoise(x*noiseLayers[j].noiseSample, x*noiseLayers[j].noiseSample) * noiseLayers[j].noiseScale;
        // }
        // return h;

        if(Physics.Raycast(new Vector3(x, -1f, z), Vector3.up, out RaycastHit rayCastHit)){
            Debug.Log(rayCastHit.transform.name);
            return rayCastHit.point.y;
        }
        else{
            Debug.Log("ERROR");
            return 0f;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainGround))]
public class TerrainGround_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGround script = (TerrainGround)target;

        if (GUILayout.Button("UPDATE"))
        {
            script.Draw();
            EditorUtility.SetDirty(target);
        }

        DrawDefaultInspector(); 
        EditorUtility.SetDirty(target);
    }
}
#endif
