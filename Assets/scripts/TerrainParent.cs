using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [ExecuteInEditMode]
public class TerrainParent : MonoBehaviour
{
    System.Random random;
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
    public GameObject player;
    public int chunks = 3;
    [HideInInspector]
    public List<float> mins= new List<float>();
    public Material material;

    public bool isUpdate = true;
    public List<float> dists = new List<float>();
    public List<float> freqs = new List<float>();
    public List<float> ratios = new List<float>();
    
    [Header("Color")]
    public float textureScale = 1f;

    List<TerrainGround> tgs = new List<TerrainGround>();


    
    public void Start()
    {
        // if(player == null){
        //     player = Generation.cam;
        //     random = new System.Random(Generation.SEED);
        //     seedOffset.x = random.Next(0, 100000000);
        //     seedOffset.y = random.Next(0, 100000000);
        // }
        Clear();
        mins.Clear();
        tgs.Clear();
        calcMax = minHeight;

        float zz = -0.5f*chunks*zSize+(zSize*0.5f);
        for(int i = 0; i < chunks; i++){
            float xx = -0.5f*chunks*xSize+(xSize*0.5f);
            for(int x = 0; x < chunks; x++){
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(xx , 0f, zz);
                TerrainGround tg = go.AddComponent<TerrainGround>();
                MeshFilter mf = go.AddComponent<MeshFilter>();
                MeshRenderer mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterial = material;
                go.AddComponent<MeshCollider>();
                Assaigner(tg);
                tgs.Add(tg);
                xx += xSize;
            }
            zz += zSize;
        }

        float min = mins[0];
        for(int i = 1; i < mins.Count; i++){
            if(mins[i] < min){
                min = mins[i];
            }
        }
        for(int i = 0; i < tgs.Count; i++){
            tgs[i].Ystepper(min);
        }
        maxHeight = calcMax;
        // Resources.UnloadUnusedAssets();


    }

    void PlayerAssaign(){
        if(player != null){
            for(int i = 0; i < tgs.Count; i++){
                tgs[i].player = player;
            }
        }
    }

    public void Clear(){
        while(transform.childCount > 0){
            if(transform.GetChild(0).gameObject.GetComponent<MeshFilter>()){
                Mesh mesh = transform.GetChild(0).gameObject.GetComponent<MeshFilter>().sharedMesh;
                DestroyImmediate(mesh, true);
            }
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }


    void Assaigner(TerrainGround tg){

        tg.xSize = xSize;
        tg.zSize = zSize;
        tg.xPoints = xPoints;
        tg.zPoints = zPoints;

        tg.maxStep = maxStep;
        tg.maxHeight = maxHeight;
        tg.minHeight = minHeight;
        tg.seedOffset = seedOffset;
        tg.noiseLayers = noiseLayers;

        tg.isGroupped = true;
    
        tg.textureScale = textureScale;

        tg.player = player;

        tg.isUpdate = isUpdate;
        tg.dists = dists;
        tg.freqs = freqs;
        tg.ratios = ratios;

        float cmax = tg.Start();
        if(cmax > calcMax){calcMax = cmax;}

    }

    public float GetHeight(float x, float z){
        // float h = 0;
        // for(int j = 0; j < noiseLayers.Count; j++){
        //     h += Mathf.PerlinNoise(x*noiseLayers[j].noiseSample, x*noiseLayers[j].noiseSample) * noiseLayers[j].noiseScale;
        //     h += Mathf.PerlinNoise(x*noiseLayers[j].noiseSample + seedOffset.x, z*noiseLayers[j].noiseSample+ seedOffset.y) * noiseLayers[j].noiseScale;
        // }
        // return h;

        if(Physics.Raycast(new Vector3(x, maxHeight+1f, z), Vector3.down, out RaycastHit rayCastHit)){
            //Debug.Log(rayCastHit.transform.name);
            //Debug.Log("height: " + rayCastHit.point.y.ToString() + "   (coordinates:  x:" + x.ToString() + "  z:" + z.ToString());
            return rayCastHit.point.y;
        }
        else{
            Debug.Log("TERRAIN ERROR");
            return 0f;
        }
    }

    public void ClearUnused(){
        Resources.UnloadUnusedAssets();
    }



}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainParent))]
public class TerrainParent_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainParent script = (TerrainParent)target;

        if (GUILayout.Button("UPDATE"))
        {
            script.Start();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("CLEAR"))
        {
            script.Clear();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("CLEAR UNUSED"))
        {
            script.ClearUnused();
            EditorUtility.SetDirty(target);
        }

        DrawDefaultInspector(); 
        //EditorUtility.SetDirty(target);
    }
}
#endif

