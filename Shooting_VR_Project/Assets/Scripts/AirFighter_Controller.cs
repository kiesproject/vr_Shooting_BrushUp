using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class AirFighter_Controller : MonoBehaviour
{

    GameManager GM;

    //遭遇するまでの時間
    [SerializeField]
    private float encounterTime = 0;

    //中心とする
    [SerializeField]
    private GameObject centerPoint;

    //飛行ルート
    [HideInInspector]
    public List<Vector3> Route_List;

    //飛行スピード
    [SerializeField]
    public float airFighter_speed = 1.0f;

    //目標座標
    private List<Vector3> target_vector3s;

    //飛行しているか
    public bool isFring = false;

    private Player PL;
    //プレイヤーかどうか(???)
    public bool startRun = false;

    //出現するまでに消すモデル
    [SerializeField]
    private GameObject destroyModel;

    //--- エディター用のフィールド --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --
    [HideInInspector] public bool onGizmo = false;
    [HideInInspector] public bool onHandle = false;
    [HideInInspector] public bool allMove = false;
    [HideInInspector] public Vector3 Edi_start_Poss;
    [HideInInspector] public Vector3 all_move_poss;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.instance;
        PL = GM.Player.GetComponent<Player>();
        //Edi_start_Poss = transform.parent.position;

        //---プレイヤー以外---
        if (!startRun)
        {
            destroyModel.gameObject.SetActive(false);
        }
        else
        {
            Launch_AriFighter();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (startRun)
        {

            //今の所何もしない
        }
        else
        {
            var player_time = PL.Get_LocalTime();
            Debug.Log("[AirFighter_Controller] player_time: " + (int)player_time + " s");
            if ((player_time >= encounterTime) && !isFring)
            {
                if (destroyModel != null)
                    Debug.Log("[AirFighter_Controller] モデルを有効");
                    destroyModel.gameObject.SetActive(true);
                Launch_AriFighter();
            }
        }


    }



    //戦闘機を飛ばす
    public void Launch_AriFighter()
    {

        int RouteCount = Route_List.Count; //データ数
        //ルートのデータのチェック
        if (RouteCount <= 0) return;

        if (!isFring) //飛ぶ命令を受けているかどうか
        {
            this.gameObject.SetActive(true);
            isFring=true;
            First_Set_TargetList();

            if (startRun)
            {
                StartCoroutine(Fly_AriFighter3(target_vector3s)); //コルーチン
            }
            else
            {
                StartCoroutine(Fly_AriFighter3(target_vector3s)); //コルーチン
            }
            return;
        }
    }

    /// <summary>
    /// 指定したルートを飛行する(Player専用)
    /// </summary>
    /// <param name="RoutList"></param>
    /// <returns></returns>
    protected virtual IEnumerator Fly_AriFighter2(List<Vector3> RoutList)
    {
        Vector3 control_point; // 制御点
        var list = new List<Vector3>(target_vector3s); //移動点のリスト
        list.Insert(0, transform.position);
        int count = list.Count;

        for (int i = 0; i < count - 1; i++)
        {
            control_point = GetControl_Point(list[0], list[1], transform.forward); //制御点を決定
            float t = 0;
            Debug.DrawLine(list[0], list[1], Color.red);
            Debug.Log("[AC] list0: " + list[0] + " list1: " + list[1]);

            while (true)
            {
                transform.position = GetMovePoint(list[0], control_point, list[1], t);
                transform.LookAt(GetMovePoint(list[0], control_point, list[1], t + 0.0001f));

                if (t > 1.0f)
                {
                    list.RemoveAt(0); //0番目を除外する。
                    break;
                }

                t += Time.deltaTime * airFighter_speed;
                yield return null;
            }
        }
    }

    /// <summary>
    /// 指定したルートを飛行する(Player以外)
    /// </summary>
    /// <param name="RoutList"></param>
    /// <returns></returns>
    protected virtual IEnumerator Fly_AriFighter3(List<Vector3> RoutList)
    {
        Vector3 control_point; // 制御点
        var list = new List<Vector3>(target_vector3s); //移動点のリスト
        list.Insert(0, transform.localPosition);
        int count = list.Count;

        for (int i = 0; i < count - 1; i++)
        {
            control_point = GetControl_Point(list[0], list[1], transform.forward); //制御点を決定
            float t = 0;

            while (true)
            {
                var pp = PL.transform.localPosition;

                transform.localPosition = GetMovePoint(list[0], control_point, list[1] , t);
                transform.LookAt(GetMovePoint(list[0], control_point, list[1], t + 0.0001f));

                if (t > 1.0f)
                {
                    list.RemoveAt(0); //0番目を除外する。
                    break;
                }

                t += Time.deltaTime * airFighter_speed;

                yield return null;
            }
        }
    }

    /// <summary>
    /// コントロールするための座標を割り出す
    /// </summary>
    /// <param name="poss1">あ？</param>
    /// <param name="poss2"></param>
    /// <param name="rotate"></param>
    /// <returns></returns>
    private Vector3 GetControl_Point(Vector3 poss1, Vector3 poss2, Vector3 rotate)
    {
        float con = 0.5f;
        float twoPoint_distance = Vector3.Distance(poss1, poss2);
        return transform.localPosition + con * twoPoint_distance * rotate;

    }

    //飛び始める時にリストを用意する
    protected void First_Set_TargetList()
    {
        if (startRun)
        {
            target_vector3s = new List<Vector3>(Conversion_RouteList(Route_List, transform.position)); //生成
        }
        else
        {
            target_vector3s = new List<Vector3>(Route_List); //生成
        }
    }

    //ルートリストのローカル座標をワールド座標に置き換える(動き始める前に実行すること)
    protected List<Vector3> Conversion_RouteList(List<Vector3> rl, Vector3 poss)
    {
        var outList = new List<Vector3>();

        foreach (Vector3 v in rl)
        {
            outList.Add(v + poss);
        }
        return outList;
    }

    //ベジェ曲線を用いた飛行座標取得
    protected Vector3 GetMovePoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t);
        var b = Vector3.Lerp(p1, p2, t);

        return Vector3.Lerp(a, b, t);

    }

    //中心のゲームオブジェクトを設定する
    public void Set_CenterObject(GameObject centor)
    {
        //飛行中は変更できない
        if (isFring) return;
        centerPoint = centor;
    }



#if UNITY_EDITOR

    [CustomEditor(typeof(AirFighter_Controller))]
    //[CustomEditor(typeof(PlayerBase))]
    public class AirFighter_Inspector : Editor
    {
        ReorderableList reorderableList;
        Vector3 snap;
        protected AirFighter_Controller component;

        void OnEnable()
        {
            var prop = serializedObject.FindProperty("Route_List");

            reorderableList = new ReorderableList(serializedObject, prop);

            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);

            };

            //SnapSettings の値を取得する
            var snapX = EditorPrefs.GetFloat("MoveSnapX", 1f);
            var snapY = EditorPrefs.GetFloat("MoveSnapY", 1f);
            var snapZ = EditorPrefs.GetFloat("MoveSnapZ", 1f);
            snap = new Vector3(snapX, snapY, snapZ);

            //component.all_move_poss = component.transform.position;

        }

        //自作したハンドル
        Vector3 PositionHandle(Vector3 position)
        {
            //var position = transform.position;
            //var size = 0.1f;
            var size = HandleUtility.GetHandleSize(position);

            //X 軸
            Handles.color = Handles.xAxisColor;
            position = Handles.Slider(position, Vector3.right, size, Handles.ArrowCap, snap.x);

            //Y 軸
            Handles.color = Handles.yAxisColor;
            position = Handles.Slider(position, Vector3.up, size, Handles.ArrowCap, snap.y);

            //Z 軸
            Handles.color = Handles.zAxisColor;
            position = Handles.Slider(position, Vector3.forward, size, Handles.ArrowCap, snap.z);

            return position;
        }

        

        protected virtual void OnSceneGUI()
        {
            //Tools.current = Tool.None;
            SetComponent();
            var transform = component.transform;
            var par = component.transform.parent;
            Vector3 pp;
            if (par == null)
            {
                pp = transform.position;
            }
            else
            {
                pp = par.position;
            }


            if (EditorApplication.isPlaying) return; //実行されている
            if (component.onHandle) //ハンドルがオフになっている
            {
                for (int i = 0; i < component.Route_List.Count; i++)
                {
                    //component.Route_List[i] = PositionHandle(component.Route_List[i] + transform.position) - transform.position;
                    component.Route_List[i] = Handles.PositionHandle(component.Route_List[i] + pp, transform.rotation) - pp;
                }
            }

            if (component.allMove)
            {
                if (transform.position != component.all_move_poss)
                {
                    for(int i=0; i<component.Route_List.Count; i++)
                    {
                        component.Route_List[i] += (transform.position - component.all_move_poss); 
                    }
                }
                component.all_move_poss = transform.position;
                //component.transform.position = Handles.PositionHandle(component.transform.position, transform.rotation);

            }

        }

        public virtual void SetComponent()
        {
            component = target as AirFighter_Controller;
        }

        bool foldout = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SetComponent();
            serializedObject.Update();
            component.onGizmo = EditorGUILayout.Toggle("ギズモを表示する", component.onGizmo);
            component.onHandle = EditorGUILayout.Toggle("ハンドルを表示する", component.onHandle);
            component.allMove = EditorGUILayout.Toggle("全ハンドルを操作", component.allMove);

            reorderableList.DoLayoutList();

            if (GUILayout.Button("更新"))
            {
                DrawDefaultInspector();
            }
            serializedObject.ApplyModifiedProperties();


        }

    }

    //ギズモ
    public class EditorGizmo_AirFighter
    {

        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
        static void DrawExampleGizmos(AirFighter_Controller airFighter, GizmoType gizmoType)
        {
            if (airFighter.Route_List == null) return; //ルートが設定されていない
            if (!airFighter.onGizmo) return; //ギズモがオフになっている

            var vertexes = airFighter.Route_List;
            var basePoss = airFighter.transform.position;
            var par = airFighter.transform.parent;
            Vector3 pp;

            if (par == null)
            {
                pp = airFighter.transform.position;
            }
            else
            {
                pp = airFighter.transform.parent.position;
            }

            Gizmos.color = new Color32(200, 150, 50, 210);
            Gizmos.DrawWireSphere(airFighter.transform.position + new Vector3(1,1,1), 0.1f);

            Gizmos.color = new Color32(200, 200, 0, 210);
            Gizmos.DrawWireSphere(airFighter.transform.position, 0.1f);
            if (vertexes.Count <= 0) return;

            if (!EditorApplication.isPlaying)
                Gizmos.DrawLine(basePoss, vertexes[0] + pp);
            //--- --- --- --- --- --- --- --- --- --- ---- --- ---- --- --- --- ---
            Gizmos.color = new Color32(145, 139, 244, 210);
            //GizmoType.Active の時は色を変える
            if ((gizmoType & GizmoType.Active) == GizmoType.Active)
                Gizmos.color = new Color32(45, 30, 244, 255);

            //ポイント


            foreach (Vector3 v3 in vertexes)
            {
                /*
                if (!EditorApplication.isPlaying)
                { Gizmos.DrawWireSphere(v3 , 0.1f); }
                else
                { Gizmos.DrawWireSphere(v3 , 0.1f); }
                */
                Gizmos.DrawWireSphere(v3 + pp, 0.1f);
            }

            //線を引く
            for (int i = 0; i < vertexes.Count - 1; i++)
            {
                /*
                if (!EditorApplication.isPlaying)
                { Gizmos.DrawLine(vertexes[i] , vertexes[i + 1] ); }
                else
                { Gizmos.DrawLine(vertexes[i] , vertexes[i + 1]); }*/
                Gizmos.DrawLine(vertexes[i] + pp, vertexes[i + 1] + pp); ;
            }

        }

    }

#endif
}

