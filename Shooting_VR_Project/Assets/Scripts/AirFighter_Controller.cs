using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AirFighter_Controller : MonoBehaviour
{

    //飛行ルート
    [HideInInspector]
    public List<Vector3> Route_List;

    //飛行スピード
    [SerializeField]
    public float airFighter_speed = 1.0f;

    //目標座標
    private List<Vector3> target_vector3s;

    //飛行しているか
    private bool isFring = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //戦闘機を飛ばす
    public void Launch_AriFighter()
    {

        int RouteCount = Route_List.Count; //データ数
        //ルートのデータのチェック
        if (RouteCount <= 0) return;

        if (!isFring) //飛ぶ命令を受けているかどうか
        {
            isFring=true;
            First_Set_TargetList();
            StartCoroutine(Fly_AriFighter2(target_vector3s)); //コルーチン

            return;
        }
    }

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

    private Vector3 GetControl_Point(Vector3 poss1, Vector3 poss2, Vector3 rotate)
    {
        float con = 0.5f;
        float twoPoint_distance = Vector3.Distance(poss1, poss2);
        return transform.position + con * twoPoint_distance * rotate;

    }

    //飛び始める時にリストを用意する
    protected void First_Set_TargetList()
    {
        target_vector3s = new List<Vector3>(Conversion_RouteList(Route_List, transform.position)); //生成

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

#if UNITY_EDITOR

    [CustomEditor(typeof(AirFighter))]
    //[CustomEditor(typeof(PlayerBase))]
    public class AirFighter_Inspector : Editor
    {
        ReorderableList reorderableList;
        Vector3 snap;
        protected AirFighter component;

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

            if (!component.onHandle) return; //ハンドルがオフになっている
            if (EditorApplication.isPlaying) return; //実行されている

            for (int i = 0; i < component.Route_List.Count; i++)
            {
                //component.Route_List[i] = PositionHandle(component.Route_List[i] + transform.position) - transform.position;
                component.Route_List[i] = Handles.PositionHandle(component.Route_List[i] + transform.position, transform.rotation) - transform.position;
            }

        }

        public virtual void SetComponent()
        {
            component = target as AirFighter;
        }

        bool foldout = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SetComponent();
            serializedObject.Update();
            component.onGizmo = EditorGUILayout.Toggle("ギズモを表示する", component.onGizmo);
            component.onHandle = EditorGUILayout.Toggle("ハンドルを表示する", component.onHandle);

            reorderableList.DoLayoutList();

            if (GUILayout.Button("更新"))
                serializedObject.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();


        }

    }

    //ギズモ
    public class EditorGizmo_AirFighter
    {

        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
        static void DrawExampleGizmos(AirFighter airFighter, GizmoType gizmoType)
        {
            if (airFighter.Route_List == null) return; //ルートが設定されていない
            if (!airFighter.onGizmo) return; //ギズモがオフになっている

            var vertexes = airFighter.Route_List;
            var basePoss = airFighter.transform.position;

            Gizmos.color = new Color32(200, 200, 0, 210);
            Gizmos.DrawWireSphere(airFighter.transform.position, 0.1f);
            if (vertexes.Count <= 0) return;

            if (!EditorApplication.isPlaying)
                Gizmos.DrawLine(basePoss, vertexes[0] + basePoss);
            //--- --- --- --- --- --- --- --- --- --- ---- --- ---- --- --- --- ---
            Gizmos.color = new Color32(145, 139, 244, 210);
            //GizmoType.Active の時は色を変える
            if ((gizmoType & GizmoType.Active) == GizmoType.Active)
                Gizmos.color = new Color32(45, 30, 244, 255);

            //ポイント

            foreach (Vector3 v3 in vertexes)
            {
                if (!EditorApplication.isPlaying)
                { Gizmos.DrawWireSphere(v3 + basePoss, 0.1f); }
                else
                { Gizmos.DrawWireSphere(v3 + airFighter.Edi_start_Poss, 0.1f); }
            }

            //線を引く
            for (int i = 0; i < vertexes.Count - 1; i++)
            {
                if (!EditorApplication.isPlaying)
                { Gizmos.DrawLine(vertexes[i] + basePoss, vertexes[i + 1] + basePoss); }
                else
                { Gizmos.DrawLine(vertexes[i] + airFighter.Edi_start_Poss, vertexes[i + 1] + airFighter.Edi_start_Poss); }
            }

        }

    }

#endif
}

