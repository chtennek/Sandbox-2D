namespace MoenenGames.SimpleMapEditor {

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


[DisallowMultipleComponent]
public class SimpleMap : MonoBehaviour {
	public bool IsLevel = false;
	public float GridSize = 1f;
}


	
#if UNITY_EDITOR



[CanEditMultipleObjects, CustomEditor(typeof(SimpleMap))]
public class SimpleMapInspector : Editor {

	public override void OnInspectorGUI () {
		(target as SimpleMap).hideFlags = HideFlags.HideInInspector;
	}

}



[InitializeOnLoad]
public static class SimpleMapEditor {



	#region -------- SUB --------




	private enum SelectionFlag {
		None = 0,
		Single = 1,
		Muti = 2,
	}



	private struct TransformData {

		public Vector3 localPosition;
		public Vector3 localScale;
		public Quaternion localRotation;

		public TransformData (Transform tf) {
			localPosition = tf.localPosition;
			localScale = tf.localScale;
			localRotation = tf.localRotation;
		}

		public TransformData (Vector3 pos, Quaternion rot, Vector3 scl) {
			localPosition = pos;
			localScale = scl;
			localRotation = rot;
		}

		public bool EqualTo (Transform tf) {
			return tf.localPosition == this.localPosition && tf.localRotation == this.localRotation && tf.localScale == this.localScale;
		}

	}



	public struct TextureUtil {


		public static Texture2D QuickTexture (int w, int h, int wB, int hB, Color c, Color cB) {
			Texture2D txt = new Texture2D(w, h, TextureFormat.ARGB32, false);
			Color[] colors = new Color[w * h];
			for (int i = 0; i < w; i++) {
				for (int j = 0; j < h; j++) {
					colors[j * w + i] = i < wB || j < hB ? cB : c;
				}
			}
			txt.SetPixels(colors);
			txt.Apply();
			return txt;
		}

		public static Texture2D QuickTexture (int w, int h, Color c) {
			return QuickTexture(w, h, 0, 0, c, Color.clear);
		}

	}



	#endregion



	#region -------- VAR --------


	

	#region  --- Texture ---


	private static Texture2D ObjectTagTexture {
		get {
			if (!objectTagTexture) {
				objectTagTexture = TextureUtil.QuickTexture(1, 1, new Color(1f, 1f, 1f, 0.04f));
			}
			return objectTagTexture;
		}
	}
	private static Texture2D objectTagTexture = null;


	private static Texture2D AreaBGTexture {
		get {
			if (!areaBGTexture) {
				areaBGTexture = TextureUtil.QuickTexture(1, 1, new Color(0, 0, 0, 0.6f));
			}
			return areaBGTexture;
		}
	}
	private static Texture2D areaBGTexture = null;


	#endregion


	#region --- Style ---


	private static GUIStyle InfoStyle {
		get {
			if (infoStyle == null) {
				infoStyle = new GUIStyle(EditorStyles.helpBox);
				infoStyle.fontSize = 12;
				infoStyle.alignment = TextAnchor.MiddleCenter;
			}
			return infoStyle;
		}
	}
	private static GUIStyle infoStyle = null;


	private static GUIStyle RemoveBtnStyle {
		get {
			if (removeBtnStyle == null) {
				removeBtnStyle = new GUIStyle(EditorStyles.miniButtonMid);
				removeBtnStyle.fontSize = 14;
				removeBtnStyle.alignment = TextAnchor.MiddleCenter;
			}
			return removeBtnStyle;
		}
	}
	private static GUIStyle removeBtnStyle = null;


	#endregion


	#region --- Call Back ---


	private static SceneView.OnSceneFunc SceneViewUpdateCallBack {
		get {
			if (sceneViewUpdateCallBack == null) {
				sceneViewUpdateCallBack = SceneGUI;
			}
			return sceneViewUpdateCallBack;
		}
	}
	private static SceneView.OnSceneFunc sceneViewUpdateCallBack = null;

	private static EditorApplication.HierarchyWindowItemCallback HierarchyUpdateCallBack {
		get {
			if (hierarchyUpdateCallBack == null) {
				hierarchyUpdateCallBack = OnHierarchyGUI;
			}
			return hierarchyUpdateCallBack;
		}
	}
	private static EditorApplication.HierarchyWindowItemCallback hierarchyUpdateCallBack = null;

	private static EditorApplication.CallbackFunction UpdateCallBack {
		get {
			if (updateCallBack == null) {
				updateCallBack = Update;
			}
			return updateCallBack;
		}
	}
	private static EditorApplication.CallbackFunction updateCallBack = null;


	#endregion


	#region --- Grid Cache ---


	private static Mesh GridMesh {
		get {
			if (!gridMesh) {
				gridMesh = new Mesh();
				gridMesh.vertices = new Vector3[]{
					new Vector3(0,0,0),
					new Vector3(1,0,0),
					new Vector3(1,0,1),
					new Vector3(0,0,1),
				};
				gridMesh.triangles = new int[]{
					0,1,2,
					0,2,3,
				};
				gridMesh.uv = new Vector2[]{
					new Vector2(0,0),
					new Vector2(1,0),
					new Vector2(1,1),
					new Vector2(0,1),
				};
				;
				gridMesh.UploadMeshData(true);
			}
			return gridMesh;
		}
	}
	private static Mesh gridMesh = null;

	private static Material GridMat {
		get {
			if (!gridMat) {
				gridMat = new Material(Shader.Find("Hidden/Grid"));
				Texture2D texture = TextureUtil.QuickTexture(30, 30, 1, 1, Color.clear, Color.white);
				texture.filterMode = FilterMode.Trilinear;
				gridMat.SetTexture("_Texture", texture);
			}
			return gridMat;
		}
	}
	private static Material gridMat = null;

	private static Material GridBGMat {
		get {
			if (!gridBGMat) {
				gridBGMat = new Material(Shader.Find("Hidden/Grid"));
				Texture2D texture = TextureUtil.QuickTexture(1, 1, Color.black);
				texture.filterMode = FilterMode.Point;
				gridBGMat.SetTexture("_Texture", texture);
			}
			return gridBGMat;
		}
	}
	private static Material gridBGMat = null;

	private static Color GridColor {
		set {
			GridMat.SetColor(GridColorID, value);
			GridBGMat.SetColor(GridColorID, value);
		}
	}

	private static Vector2 GridTileNum {
		set {
			GridMat.SetVector(TileNumID, value);
			GridBGMat.SetVector(TileNumID, value);
		}
	}

	private static int GridColorID {
		get {
			if (gridColorID == null) {
				gridColorID = Shader.PropertyToID("_Color");
			}
			return gridColorID.Value;
		}
	}
	private static int? gridColorID = null;

	private static int TileNumID {
		get {
			if (tileNumID == null) {
				tileNumID = Shader.PropertyToID("_TileNum");
			}
			return tileNumID.Value;
		}
	}
	private static int? tileNumID = null;

	#endregion


	#region --- Data ---

	// Const
	private const string PREFS_TAG = "MoenenGames.SimpleMapFlag.";
	private const float SPF = 0.04f;
	private const float AREA_WIDTH = 160f;
	private const float AREA_HEIGHT = 294f;
	private const float FIELD_WIDTH = 64f;
	private const float FIELD_HEIGHT = 18f;
	private const int DYNAMIC_SNAP_MAX = 150;

	// Data
	private static bool DraggingCamera = false;
	private static bool DraggingObject = false;
	private static bool NeedRepaint = true;
	private static float AreaFading = 0f;
	private static float AreaAimFading = 1f;
	private static float GridPosY = 0f;
	private static float OldGridSize = 1f;
	private static float UpdatingTime = 0f;
	private static int PaintingID = 0;
	private static int PrevSelectingTransformNum = 0;
	private static Transform EditingRoot = null;
	private static Transform RootGrid = null;
	private static TransformData? DraggingObjectTFData = null;
	private static Vector3? DraggingStart = null;
	private static Vector3? ReadyToDrag = null;
	private static Vector3? LastCursorPos = null;
	private static Vector3 LastHoveringPos = Vector2.one * float.MaxValue;
	private static Quaternion PaintingRot = Quaternion.identity;
	private static Quaternion OldMapRot = Quaternion.identity;


	// Saving Data
	private static float GridAlpha = 0.5f;
	private static bool ShowGridBG = false;
	private static bool ShowHighlightFrame = false;
	

	#endregion


	#endregion


	
	#region -------- MSG --------



	static SimpleMapEditor () {
		SceneView.onSceneGUIDelegate -= SceneViewUpdateCallBack;
		SceneView.onSceneGUIDelegate += SceneViewUpdateCallBack;
		EditorApplication.hierarchyWindowItemOnGUI -= HierarchyUpdateCallBack;
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyUpdateCallBack;
		EditorApplication.update -= UpdateCallBack;
		EditorApplication.update += UpdateCallBack;
		EditorApplication.RepaintHierarchyWindow();
		Load();
	}



	static void SceneGUI (SceneView sceneView) {

		

		#region --- Cache ---

		

		#region --- Root ---


		if (!EditingRoot) {
			return;
		}

		EditingRoot.rotation = Quaternion.identity;
		EditingRoot.localScale = Vector3.one;

		Vector3 rootPos = EditingRoot.position;
		rootPos.y = RootGrid.position.y - 0.001f;
		Quaternion rootRot = EditingRoot.rotation;
		SimpleMap rootFlag = EditingRoot.GetComponent<SimpleMap>();


		if (!rootFlag || !rootFlag.IsLevel) {
			return;
		}

		
		#endregion


		#region --- Grid ---

		// Grid Size
		float gridSize = rootFlag.GridSize;
		if (OldGridSize != gridSize) {
			AddAllColliders(EditingRoot, gridSize);
			SnapAllObject(EditingRoot, gridSize);
			OldGridSize = gridSize;
		}

		Vector3 gridMin;
		Vector3 gridMax;

		{
			Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

			bool hasObjFlag = false;

			ForEachWith<SimpleMap>(EditingRoot, (sm) => {
				hasObjFlag = true;
				if (!sm.IsLevel) {
					Vector3 pos = WorldToLocal(sm.transform.position, rootPos, rootRot);
					min = Vector3.Min(min, pos);
					max = Vector3.Max(max, pos);
				}
			});

			if (!hasObjFlag) {
				min = max = Vector3.zero;
			}

			gridMin = Snap(min - new Vector3(
				gridSize * 4.5f,
				gridSize * 0.5f,
				gridSize * 4.5f
			), gridSize, Vector3.zero);

			gridMax = Snap(max + new Vector3(
				gridSize * 4.5f,
				gridSize * 1.5f,
				gridSize * 4.5f
			), gridSize, Vector3.zero);

		}

		

		#endregion


		#region --- DraggingObject ---

		DraggingObject = DraggingObject || (DraggingObjectTFData != null && Selection.activeTransform && !DraggingObjectTFData.Value.EqualTo(Selection.activeTransform));
		if (Selection.activeTransform) {
			DraggingObjectTFData = new TransformData(Selection.activeTransform);
		} else {
			DraggingObjectTFData = null;
		}

		if (DraggingObject && Selection.transforms.Length < DYNAMIC_SNAP_MAX) {
			// Snap Selecting
			for (int i = 0; i < Selection.transforms.Length; i++) {
				if (Selection.transforms[i].parent == EditingRoot) {
					SnapObject(Selection.transforms[i], EditingRoot, gridSize);
				}
			}
		}

		#endregion


		#region --- Other ---


		Transform cameraTF = sceneView.camera.transform;
		Vector3 cameraLocalPos = WorldToLocal(cameraTF.position, rootPos, rootRot);

		bool inGUI = new Rect(0, 0, AREA_WIDTH, Mathf.Max(18, (AREA_HEIGHT + 24f) * AreaAimFading)).Contains(Event.current.mousePosition);

		Vector3 selectionSize = new Vector3();

		
		#endregion


		#region --- Selection ---

		if (PrevSelectingTransformNum != Selection.transforms.Length) {
			PrevSelectingTransformNum = Selection.transforms.Length;
		}

		List<GameObject> selectingPrefabs;

		{
			Object[] selectingAssets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
			selectingPrefabs = new List<GameObject>(selectingAssets.Length);
			for (int i = 0; i < selectingAssets.Length; i++) {
				if (selectingAssets[i] is GameObject) {
					if (!selectingPrefabs.Contains(selectingAssets[i] as GameObject)) {
						SimpleMap sm = GetFlag((selectingAssets[i] as GameObject).transform);
						if (!sm || !sm.IsLevel) {
							selectingPrefabs.Add(selectingAssets[i] as GameObject);
						}
					}
				} else if(selectingAssets[i] is UnityEditor.DefaultAsset){
					string path = AssetDatabase.GetAssetPath(selectingAssets[i]);
					if (!string.IsNullOrEmpty(path)) {
						GameObject[] objs = GetAtPath<GameObject>(path);
						for (int j = 0; j < objs.Length; j++) {
							if (!selectingPrefabs.Contains(objs[j])) {
								SimpleMap sm = GetFlag(objs[j].transform);
								if (!sm || !sm.IsLevel) {
									selectingPrefabs.Add(objs[j]);
								}
							}
						}
					}
				}
			}
		}

		
		PaintingID = selectingPrefabs.Count > 0 ? Mathf.Clamp(PaintingID, 0, selectingPrefabs.Count - 1) : 0;


		#endregion


		#region --- Cursor ---

		Vector3? cursorPos = null;
		Transform hoveringObject = null;
		BoxCollider hoveringCollider = null;
		Vector3 hoveringNormal = Vector3.up;
		float cursorCameraDis = float.MaxValue;
		
		{

			// Ray Cast Grid
			Vector3 worldCursorPos;
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			RaycastHit[] gridHits = Physics.RaycastAll(ray, float.MaxValue, -1, QueryTriggerInteraction.Collide);
			for (int i = 0; i < gridHits.Length; i++) {
				if (gridHits[i].transform == RootGrid) {
					worldCursorPos = gridHits[i].point;
					Vector3 v = Snap(WorldToLocal(worldCursorPos, rootPos, rootRot), gridSize, new Vector3(gridSize * 0.5f, 0, gridSize * 0.5f));
					v.y = 0;
					cursorPos = v;
					cursorCameraDis = Vector3.Distance(cursorPos.Value, cameraLocalPos);
					break;
				}
			} 

			// Ray Cast Object
			RaycastHit[] hits = Physics.RaycastAll(ray, float.MaxValue, -1, QueryTriggerInteraction.Collide);
			float dis = float.MaxValue;
			for (int i = 0; i < hits.Length; i++) {
				if (hits[i].collider.hideFlags == HideFlags.HideInInspector) {
					float d = Vector3.Distance(hits[i].point, ray.origin);
					if (d < dis) {

						BoxCollider bc = hits[i].collider as BoxCollider;
						Vector3 hPos = hits[i].point;
						hPos = WorldToLocal(hPos, rootPos, rootRot);
						hPos = Snap(hPos, gridSize, new Vector3(gridSize * 0.5f, gridSize * 0.5f, gridSize * 0.5f));
						Vector3 pos = WorldToLocal(hits[i].transform.position, rootPos, rootRot);

						Vector3 min = bc.center - 0.5f * bc.size;
						Vector3 max = bc.center + 0.5f * bc.size;
						Quaternion r = hits[i].transform.rotation * Quaternion.Inverse(rootRot);
						Vector3 tempMin = r * min;
						Vector3 tempMax = r * max;
						min = Vector3.Min(tempMin, tempMax) + pos;
						max = Vector3.Max(tempMin, tempMax) + pos;

						hPos = Cling(hPos, hits[i].normal,
							min,
							max
						);

						if (Vector3.Distance(hPos, cameraLocalPos) < cursorCameraDis) {
							dis = d;
							hoveringObject = hits[i].transform;
							hoveringCollider = hits[i].collider as BoxCollider;
							hoveringNormal = hits[i].normal;
							cursorPos = hPos;
						}
						
					}
				}
			}

			if (cursorPos != null) {
				LastCursorPos = cursorPos.Value;
			}

			if (hoveringObject) {
				hoveringNormal = SnapRotation(Quaternion.FromToRotation(Vector3.up, hoveringNormal), Quaternion.identity) * Vector3.up;
			}
		}


		#endregion

		
		
		#endregion

		

		#region --- Event ---


		SelectionFlag paintFlag = SelectionFlag.None;
		SelectionFlag selectionFlag = SelectionFlag.None;

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		switch (Event.current.type) {


			#region --- Mouse Up ---

			case EventType.MouseUp:

				if (Event.current.button == 0) {

					if (ReadyToDrag != null && DraggingStart != null) {

						if (selectingPrefabs.Count > 0) {
							// Try Add Muti
							paintFlag = SelectionFlag.Muti;
						} else {
							// Try Selct Muti
							selectionFlag = SelectionFlag.Muti;
						}

						// Clear Dragging
						DraggingObject = false;
						NeedRepaint = true;

					} else {

						if (selectingPrefabs.Count > 0) {
							// Tri Add Single
							paintFlag = SelectionFlag.Single;
						} else {
							// Try Selct Single
							selectionFlag = SelectionFlag.Single;
						}

						// Snap Selecting
						int len = Selection.transforms.Length;
						for (int i = 0; i < len; i++) {
							Transform tf = Selection.transforms[i];
							if (tf.parent == EditingRoot) {
								SnapObject(tf, EditingRoot, gridSize);
							}
						}

					}

				} else if (Event.current.button == 1) {
					if (DraggingCamera) {
						DraggingCamera = false;
					} else {
						if (selectingPrefabs.Count > 0) {
							// Rotate Object
							PaintingRot = Quaternion.Euler(0f, PaintingRot.eulerAngles.y, 0f);
							PaintingRot *= Quaternion.Euler(0f, 90f, 0f);
						}
					}
				}
				break;

			#endregion


			#region --- Mouse Drag ---

			case EventType.MouseDrag:

				if (Event.current.button == 0) {

					if (inGUI) {
						break;
					}

					// Dragging Frame
					if (LastCursorPos != null && cursorPos != null) {
						if (ReadyToDrag == null) {
							if (!Event.current.shift) {
								ReadyToDrag = cursorPos;
							}
						} else if (DraggingStart == null) {
							if (!Event.current.shift) {
								if (Vector3.Distance(ReadyToDrag.Value, cursorPos.Value) >= gridSize) {
									DraggingStart = ReadyToDrag.Value;
								}
							}
						}
					}
					
					if (Event.current.alt) {
						Event.current.Use();
					}

				} else if (Event.current.button == 1) {
					// Mosue Right Drag
					if (!Event.current.alt) {
						// View Rotate
						DraggingCamera = true;
						Vector2 del = Event.current.delta * 0.2f;

						float angle = cameraTF.rotation.eulerAngles.x + del.y;
						angle = angle > 89 && angle < 180 ? 89 : angle;
						angle = angle > 180 && angle < 271 ? 271 : angle;
						sceneView.LookAt(
							sceneView.pivot,
							Quaternion.Euler(
								angle,
								cameraTF.rotation.eulerAngles.y + del.x,
								0f
							),
							sceneView.size,
							sceneView.orthographic,
							true
						);
						Event.current.Use();
					}
				}
				break;

			#endregion


			#region --- Mouse Move ---


			case EventType.MouseMove:

				// Clear Dragging
				if (ReadyToDrag != null || DraggingStart != null) {

					if (selectingPrefabs.Count > 0) {
						// Try Add Muti
						paintFlag = SelectionFlag.Muti;
					} else {
						// Try Select Muti
						selectionFlag = SelectionFlag.Single;
					}

					// Snap Selecting
					int len = Selection.transforms.Length;
					for (int i = 0; i < len; i++) {
						Transform tf = Selection.transforms[i];
						if (tf.parent == EditingRoot) {
							SnapObject(tf, EditingRoot, gridSize);
						}
					}
					
					DraggingStart = null;
					ReadyToDrag = null;
					NeedRepaint = true;
				}

				DraggingCamera = false;
				DraggingObject = false;

				break;

			#endregion


			#region --- Key Down ---

			case EventType.KeyDown:

				switch (Event.current.keyCode) {
					case KeyCode.F:
						Transform oldSelect = Selection.activeTransform;
						Selection.activeTransform = Selection.activeTransform ?? EditingRoot;
						sceneView.FrameSelected(false);
						Selection.activeTransform = oldSelect;
						Event.current.Use();
						break;
					case KeyCode.Space:
					case KeyCode.Return:
						Selection.objects = new Object[0];
						break;
					case KeyCode.UpArrow:
					case KeyCode.W:
						if (Event.current.shift) {
							GridPosY += gridSize;
							Event.current.Use();
						}
						break;
					case KeyCode.DownArrow:
					case KeyCode.S:
						if (Event.current.shift) {
							GridPosY -= gridSize;
							Event.current.Use();
						}
						break;
				}
				break;

			#endregion


		}

		#endregion

		

		#region --- Drawing ---

		

		#region --- Grid ---


		RootGrid.localPosition = new Vector3(gridMin.x, GridPosY + 0.001f, gridMin.z);
		RootGrid.localScale = gridMax - gridMin;
		GridTileNum = new Vector2((gridMax.x - gridMin.x) / gridSize, (gridMax.z - gridMin.z) / gridSize);
		GridColor = new Color(1, 1, 1, GridAlpha);
		RootGrid.GetComponent<MeshRenderer>().enabled = ShowGridBG;

		#endregion



		#region --- Hovering ---

		{
			if (!DraggingCamera && !DraggingObject && !inGUI) {

				Vector3 offsetMin = -Vector3.one * gridSize * 0.5f;
				Vector3 offsetMax = Vector3.one * gridSize * 0.5f;
				
				if (selectingPrefabs.Count > 0) {
					Bounds b = GetMeshBounds(selectingPrefabs[PaintingID].transform, gridSize);
					offsetMin = b.min;
					offsetMax = b.max;
				}

				Vector3[] roomedLines = new Vector3[4];

				if (hoveringObject && cursorPos != null) {

					Quaternion rot = SnapRotation(Quaternion.FromToRotation(Vector3.up, hoveringNormal), Quaternion.identity);

					roomedLines = new Vector3[] { 
						cursorPos.Value + PaintingRot * new Vector3(offsetMax.x, 0f, offsetMax.z), 
						cursorPos.Value + PaintingRot * new Vector3(offsetMax.x, 0f, offsetMin.z), 
						cursorPos.Value + PaintingRot * new Vector3(offsetMin.x, 0f, offsetMin.z), 
						cursorPos.Value + PaintingRot * new Vector3(offsetMin.x, 0f, offsetMax.z),
					};

					roomedLines[0] = rot * (roomedLines[0] - cursorPos.Value) + cursorPos.Value;
					roomedLines[1] = rot * (roomedLines[1] - cursorPos.Value) + cursorPos.Value;
					roomedLines[2] = rot * (roomedLines[2] - cursorPos.Value) + cursorPos.Value;
					roomedLines[3] = rot * (roomedLines[3] - cursorPos.Value) + cursorPos.Value;

					// Frame

					if (ShowHighlightFrame && DraggingStart == null) {

						Vector3 pos = WorldToLocal(hoveringObject.position, rootPos, rootRot);
						Vector3 min = hoveringCollider.center - 0.5f * hoveringCollider.size;
						Vector3 max = hoveringCollider.center + 0.5f * hoveringCollider.size;

						Quaternion r = hoveringObject.rotation * Quaternion.Inverse(rootRot);
						Vector3 tempMin = r * min;
						Vector3 tempMax = r * max;
						min = Vector3.Min(tempMin, tempMax) + pos;
						max = Vector3.Max(tempMin, tempMax) + pos;

						List<Vector3> frameLines = new List<Vector3>();

						// Half Clamp

						bool uFlag = true;
						bool dFlag = true;

						if (cursorPos.Value.y > 0) {
							if (min.y > -gridSize * 0.1f != max.y > -gridSize * 0.1f) {
								min.y = Mathf.Max(0f, min.y);
								dFlag = false;
							}
						}

						if (cursorPos.Value.y < 0) {
							if (min.y > gridSize * 0.1f != max.y > gridSize * 0.1f) {
								max.y = Mathf.Min(0f, max.y);
								uFlag = false;
							}
						}

						// Shadow
						Handles.color = new Color(0, 0, 0, 0.4f);
						Handles.DrawAAConvexPolygon(new Vector3[] {
							LocalToWorld(new Vector3(min.x, 0f, min.z), rootPos, rootRot),
							LocalToWorld(new Vector3(min.x, 0f, max.z), rootPos, rootRot),
							LocalToWorld(new Vector3(max.x, 0f, max.z), rootPos, rootRot),
							LocalToWorld(new Vector3(max.x, 0f, min.z), rootPos, rootRot),
						});

						

						if (uFlag) {
							frameLines.Add(new Vector3(min.x, max.y, min.z));
							frameLines.Add(new Vector3(max.x, max.y, min.z));
							frameLines.Add(new Vector3(max.x, max.y, max.z));
							frameLines.Add(new Vector3(min.x, max.y, max.z));
							frameLines.Add(new Vector3(max.x, max.y, max.z));
							frameLines.Add(new Vector3(max.x, max.y, min.z));
							frameLines.Add(new Vector3(min.x, max.y, min.z));
							frameLines.Add(new Vector3(min.x, max.y, max.z));
						}

						if (dFlag) {
							frameLines.Add(new Vector3(min.x, min.y, min.z));
							frameLines.Add(new Vector3(max.x, min.y, min.z));
							frameLines.Add(new Vector3(min.x, min.y, min.z));
							frameLines.Add(new Vector3(min.x, min.y, max.z));
							frameLines.Add(new Vector3(min.x, min.y, max.z));
							frameLines.Add(new Vector3(max.x, min.y, max.z));
							frameLines.Add(new Vector3(max.x, min.y, min.z));
							frameLines.Add(new Vector3(max.x, min.y, max.z));
						}

						frameLines.Add(new Vector3(min.x, min.y, min.z));
						frameLines.Add(new Vector3(min.x, max.y, min.z));
						frameLines.Add(new Vector3(max.x, max.y, max.z));
						frameLines.Add(new Vector3(max.x, min.y, max.z));
						frameLines.Add(new Vector3(max.x, min.y, min.z));
						frameLines.Add(new Vector3(max.x, max.y, min.z));
						frameLines.Add(new Vector3(min.x, min.y, max.z));
						frameLines.Add(new Vector3(min.x, max.y, max.z));


						Vector3[] dottedFrame = frameLines.ToArray();

						LocalToWorld(ref dottedFrame, rootPos, rootRot, Vector3.one);

						Handles.color = new Color(0, 0, 0, 0.7f);
						Handles.DrawLines(dottedFrame);
						Handles.color = new Color(1, 1, 1, 0.7f);
						Handles.DrawDottedLines(dottedFrame, 2f);

						

					}

				} else {
					Vector3? pos = cursorPos != null ? cursorPos : LastCursorPos != null ? LastCursorPos : null;
					if (pos != null) {
						roomedLines = new Vector3[] { 
							Round(pos.Value + PaintingRot * new Vector3(offsetMax.x, 0f, offsetMax.z), gridSize, Vector3.zero),
							Round(pos.Value + PaintingRot * new Vector3(offsetMax.x, 0f, offsetMin.z), gridSize, Vector3.zero),
							Round(pos.Value + PaintingRot * new Vector3(offsetMin.x, 0f, offsetMin.z), gridSize, Vector3.zero),
							Round(pos.Value + PaintingRot * new Vector3(offsetMin.x, 0f, offsetMax.z), gridSize, Vector3.zero),
						};
					}
				}
				

				
				// Hovering
				if (DraggingStart == null && cursorPos != null) {

					// Not Dragging

					LocalToWorld(ref roomedLines, rootPos, rootRot, Vector3.one);

					Color hColor = selectingPrefabs.Count > 0 ?
						(hoveringNormal == Vector3.up ? Color.green : Color.red) : 
						(hoveringObject ? Color.white : Color.red);
					hColor.a = 0.3f;
					Handles.color = hColor;
					Handles.DrawAAConvexPolygon(roomedLines);
					hColor = Color.white;
					hColor.a = 0.3f;
					Handles.color = hColor;
					Handles.DrawLines(new Vector3[] { 
						roomedLines[0], roomedLines[1],
						roomedLines[3], roomedLines[0],
					});

					// Cursor Axis

					if (selectingPrefabs.Count > 0) {
						Color axisColorZ = new Color(58f / 256f, 122f / 256f, 248f / 256f, 1);
						Color axisColorX = new Color(219f / 256f, 62f / 256f, 29f / 256f, 1);
						Handles.color = axisColorX;
						Handles.DrawLine(roomedLines[1], roomedLines[2]);
						Handles.color = axisColorZ;
						Handles.DrawLine(roomedLines[2], roomedLines[3]);
					}

				}

				selectionSize = Vector3.Max(
					Vector3.Max(roomedLines[0], roomedLines[1]),
					Vector3.Max(roomedLines[2], roomedLines[3])
				) - Vector3.Min(
					Vector3.Min(roomedLines[0], roomedLines[1]),
					Vector3.Min(roomedLines[2], roomedLines[3])
				);

				if (selectionSize.x < gridSize * 0.1f) {
					selectionSize.x = selectionSize.y;
				} else if (selectionSize.z < gridSize * 0.1f) {
					selectionSize.z = selectionSize.y;
				}

				selectionSize.y = Mathf.Abs(offsetMax.y - offsetMin.y);
				
			}

			if (cursorPos != null) {
				if (LastHoveringPos != cursorPos) {
					NeedRepaint = true;
				}
				LastHoveringPos = cursorPos.Value;
			}

		}

		#endregion



		#region --- Dragging Frame ---


		Bounds? selectionZone = null;

		if (!DraggingCamera && !DraggingObject && DraggingStart != null && LastCursorPos != null && !inGUI) {

			Vector3 ext = selectionSize * 0.5f;
			ext.y = 0f;
			Vector3 min = Vector3.Min(LastCursorPos.Value - ext, LastCursorPos.Value + ext);
			Vector3 max = Vector3.Max(LastCursorPos.Value - ext, LastCursorPos.Value + ext);

			min = Vector3.Min(min, DraggingStart.Value - ext);
			max = Vector3.Max(max, DraggingStart.Value + ext);

			Bounds b = new Bounds();
			b.SetMinMax(min, max);
			selectionZone = b;

			if (min.y % gridSize > gridSize * 0.1f) {
				min.y -= gridSize * 0.5f;
			}

			if (max.y % gridSize > gridSize * 0.1f) {
				max.y += gridSize * 0.5f;
			}

			if (max.y - min.y < gridSize * 0.1f) {
				max.y += gridSize;
			}

			Handles.color = selectingPrefabs.Count > 0 ? new Color(0.1f, 0.8f, 0.1f, 0.2f) : new Color(1, 1, 1, 0.2f);
			
			Handles.DrawAAConvexPolygon(new Vector3[] { 
				LocalToWorld(new Vector3(min.x, min.y, min.z), rootPos, rootRot),
				LocalToWorld(new Vector3(min.x, min.y, max.z), rootPos, rootRot),
				LocalToWorld(new Vector3(max.x, min.y, max.z), rootPos, rootRot),
				LocalToWorld(new Vector3(max.x, min.y, min.z), rootPos, rootRot),
			});

			Vector3[] lines = new Vector3[]{

				new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z),
				new Vector3(min.x, min.y, min.z), new Vector3(min.x, max.y, min.z),
				new Vector3(min.x, min.y, min.z), new Vector3(min.x, min.y, max.z),

				new Vector3(max.x, max.y, max.z), new Vector3(min.x, max.y, max.z),
				new Vector3(max.x, max.y, max.z), new Vector3(max.x, min.y, max.z),
				new Vector3(max.x, max.y, max.z), new Vector3(max.x, max.y, min.z),

				new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z),
				new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z),
				new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z),

				new Vector3(max.x, min.y, min.z), new Vector3(max.x, min.y, max.z),
				new Vector3(min.x, max.y, min.z), new Vector3(min.x, max.y, max.z),
				new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z),

			};

			LocalToWorld(ref lines, rootPos, rootRot, Vector3.one);
			Color hColor = Handles.color;
			hColor.a = 0.5f;
			Handles.color = hColor;
			Handles.DrawLines(lines);
			Handles.color = new Color(0, 0, 0, 0.6f);
			Handles.DrawDottedLines(lines, 2f);

		}

		
		#endregion



		#region --- Intersection ---


		bool hasInter = false;
		if (selectingPrefabs.Count > 0 && cursorPos != null && hoveringNormal == Vector3.up) {
			
			Bounds b = LocalToWorld(new Bounds(cursorPos.Value, selectionSize), rootPos, rootRot, Vector3.one);

			b.center += (Vector3.up * (b.extents.y + gridSize * 0.01f)) * gridSize;
			b.extents += Vector3.down * 0.02f * gridSize;

			GameObject[] objs = TryGetObjects(b, gridSize);

			if (objs.Length > 0) {
				for (int i = 0; i < objs.Length; i++) {
					
					BoxCollider bc = objs[i].GetComponent<BoxCollider>();
					Bounds objBound = new Bounds();
					
					Vector3 _min = objs[i].transform.rotation * (bc.center - 0.5f * bc.size) + objs[i].transform.position;
					Vector3 _max = objs[i].transform.rotation * (bc.center + 0.5f * bc.size) + objs[i].transform.position;
					
					objBound.SetMinMax(
						Vector3.Min(_min, _max),
						Vector3.Max(_min, _max)
					);

					Bounds? inter = Intersection(b, objBound);

					if (inter != null) {
						hasInter = true;

						Vector3 min = inter.Value.min;
						Vector3 max = inter.Value.max;

						Vector3[] lines = new Vector3[]{

							new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z),
							new Vector3(min.x, min.y, min.z), new Vector3(min.x, max.y, min.z),
							new Vector3(min.x, min.y, min.z), new Vector3(min.x, min.y, max.z),

							new Vector3(max.x, max.y, max.z), new Vector3(min.x, max.y, max.z),
							new Vector3(max.x, max.y, max.z), new Vector3(max.x, min.y, max.z),
							new Vector3(max.x, max.y, max.z), new Vector3(max.x, max.y, min.z),

							new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z),
							new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z),
							new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z),

							new Vector3(max.x, min.y, min.z), new Vector3(max.x, min.y, max.z),
							new Vector3(min.x, max.y, min.z), new Vector3(min.x, max.y, max.z),
							new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z),

						};

						Handles.color = Color.red;
						Handles.DrawLines(lines);
						
					}
				}
			}
		} else {
			hasInter = paintFlag != SelectionFlag.Muti;
		}


		#endregion



		#endregion



		#region --- Paint ---



		if (!DraggingCamera && !DraggingObject && !inGUI && !hasInter) {
			
			if (paintFlag != SelectionFlag.None) {

				if (cursorPos != null) {

					if (selectingPrefabs[PaintingID]) {

						if (paintFlag == SelectionFlag.Single) {

							Transform tf = selectingPrefabs[PaintingID].transform;

							Spawn(
								tf.gameObject,
								EditingRoot,
								LocalToWorld(cursorPos.Value, rootPos, rootRot),
								PaintingRot,
								gridSize
							);

							RandomPaintingID(selectingPrefabs.Count);

						} else {
							// Muti Paint
							if (selectionZone != null) {

								Bounds b = LocalToWorld(selectionZone.Value, rootPos, rootRot, Vector3.one);

								b.center += Vector3.up * 0.1f * gridSize;
								b.extents -= new Vector3(gridSize * 0.1f, 0f, gridSize * 0.1f);

								Vector3 start = b.min + new Vector3(selectionSize.x, 0, selectionSize.z) * 0.5f;
								Vector3 end = b.min + b.size;
								for (float i = start.x; i <= end.x; i += selectionSize.x) {
									for (float j = start.y; j <= end.y; j += selectionSize.y) {
										for (float k = start.z; k <= end.z; k += selectionSize.z) {
											
											Transform tf = selectingPrefabs[PaintingID].transform;

											if(
												TryGetObjects(new Bounds(
													new Vector3(i, j, k) + Vector3.up * gridSize * 0.1f,
													new Vector3(1, 0, 1) * gridSize * 0.1f
												), gridSize
												).Length <= 0
											) {
												Spawn(
												tf.gameObject,
												EditingRoot,
												new Vector3(i, j, k),
												PaintingRot,
												gridSize
											);

												RandomPaintingID(selectingPrefabs.Count);
											}

										}
									}
								}



							}
						}
					}
				}
			}
		}


		if (paintFlag != SelectionFlag.None || DraggingCamera || DraggingObject || inGUI) {
			DraggingStart = null;
			ReadyToDrag = null;
		}


		#endregion

		

		#region --- Select ---


		if (!DraggingCamera && !DraggingObject && !inGUI) {

			if (selectionFlag != SelectionFlag.None) {

				List<GameObject> objs = new List<GameObject>();

				if (Event.current.control || Event.current.alt) {
					for (int i = 0; i < Selection.transforms.Length; i++) {
						objs.Add(Selection.transforms[i].gameObject);
					}
				}

				if (selectionFlag == SelectionFlag.Single) {

					if (hoveringObject) {
						if (Event.current.alt) {
							if (objs.Contains(hoveringObject.gameObject)) {
								objs.Remove(hoveringObject.gameObject);
							}
						} else {
							objs.Add(hoveringObject.gameObject);
						}
					}

				} else {

					if (selectionZone != null) {
						
						Bounds b = LocalToWorld(selectionZone.Value, rootPos, rootRot, Vector3.one);
						
						if (b.center.y % gridSize < 0.1f * gridSize) {
							b.center += Vector3.up * 0.5f * gridSize;
						}

						b.extents -= new Vector3(gridSize * 0.1f, 0f, gridSize * 0.1f);

						if (Event.current.alt) {
							GameObject[] selecting = TryGetObjects(b, gridSize);
							for (int i = 0; i < selecting.Length; i++) {
								if(objs.Contains(selecting[i])){
									objs.Remove(selecting[i]);
								}
							}
						} else {
							objs.AddRange(TryGetObjects(b, gridSize));
						}
					}
				}

				Selection.objects = objs.ToArray();

				DraggingStart = null;
				ReadyToDrag = null;

			}

		} else {
			DraggingStart = null;
			ReadyToDrag = null;
		}


		#endregion



		


		#region --- GUI ---

		{

			Handles.BeginGUI();

			Rect areaRect = new Rect(0, Mathf.Lerp(-AREA_HEIGHT + 18f, 0, AreaFading), AREA_WIDTH, AREA_HEIGHT);

			GUILayout.BeginArea(areaRect);

			GUI.DrawTexture(new Rect(0, 0, AREA_WIDTH, AREA_HEIGHT), AreaBGTexture);

			GUILayout.Space(8);


			#region --- Grid Size ---

			GUILayout.BeginHorizontal();

			GUI.Label(GUIRect(0, FIELD_HEIGHT, true), "Grid Size");
			GUILayout.Space(4);
			rootFlag.GridSize = Mathf.Clamp(EditorGUI.DelayedFloatField(GUIRect(FIELD_WIDTH, FIELD_HEIGHT), rootFlag.GridSize), 0.01f, float.MaxValue);
			gridSize = rootFlag.GridSize;
			GUILayout.Space(4);
			GUILayout.EndHorizontal();

			GUILayout.Space(4);

			#endregion


			#region --- GridAlpha ---


			GUILayout.BeginHorizontal();

			GUI.Label(GUIRect(0, FIELD_HEIGHT, true), "Grid Alpha");
			GUILayout.Space(4);
			GridAlpha = GUI.Slider(GUIRect(FIELD_WIDTH, FIELD_HEIGHT), GridAlpha, 0f, 0f, 1f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, GUIUtility.GetControlID(FocusType.Passive));
			GUILayout.Space(4);
			GUILayout.EndHorizontal();

			GUILayout.Space(4);


			#endregion


			#region --- Grid BG ---

			GUILayout.BeginHorizontal();

			GUI.Label(GUIRect(0, FIELD_HEIGHT, true), "Grid BG");
			GUILayout.Space(4);
			ShowGridBG = GUI.Toggle(GUIRect((FIELD_WIDTH + FIELD_HEIGHT) * 0.5f, FIELD_HEIGHT), ShowGridBG, "");
			GUILayout.Space(4);
			GUILayout.EndHorizontal();

			GUILayout.Space(4);

			#endregion


			#region --- HighLight Frame ---

			GUILayout.BeginHorizontal();

			GUI.Label(GUIRect(0, FIELD_HEIGHT, true), "Highlight Frame");
			GUILayout.Space(4);
			ShowHighlightFrame = GUI.Toggle(GUIRect((FIELD_WIDTH + FIELD_HEIGHT) * 0.5f, FIELD_HEIGHT), ShowHighlightFrame, "");
			GUILayout.Space(4);
			GUILayout.EndHorizontal();

			GUILayout.Space(4);

			#endregion


			#region --- Half Fold Btn ---

			float halfRant = 0.5f;
			{
				Color oldGUIColor = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, 0.8f);
				Rect rect = GUIRect(0, 18, true);
				halfRant = 1f - (rect.y + 6f) / AREA_HEIGHT;
				if (GUI.Button(rect, AreaFading > Mathf.Lerp(halfRant, 1f, 0.5f) ? "▲" : "▼", EditorStyles.miniButtonMid)) {
					AreaAimFading = AreaAimFading > halfRant ? halfRant : 1f;
				}
				GUI.color = oldGUIColor;
			}


			#endregion


			#region --- Preview ---

			{

				Rect rect = GUIRect(AREA_WIDTH, AREA_WIDTH);

				if (selectingPrefabs.Count > 0) {
					if (selectingPrefabs.Count == 1) {
						Texture2D texture = AssetPreview.GetAssetPreview(selectingPrefabs[0]);
						if (texture) {
							GUI.DrawTexture(rect, texture);
							if (GUI.Button(
								new Rect(rect.x + rect.width - 24, rect.y, 24, 24),
								"×", RemoveBtnStyle)
							) {
								selectingPrefabs.Clear();
								Selection.objects = new Object[0];
							}
						}
					} else {
						float gap = 2;
						float w = rect.width / 2f - gap;
						float h = rect.height / 2f - gap;
						Rect[] rects = new Rect[] { 
							new Rect(rect.x + + gap / 2f, rect.y + gap / 2f, w, h), 
							new Rect(rect.x + rect.width/2f + + gap / 2f, rect.y + + gap / 2f, w, h), 
							new Rect(rect.x + + gap / 2f, rect.y + rect.height/2f + + gap / 2f, w, h), 
							new Rect(rect.x + rect.width/2f + + gap / 2f, rect.y + rect.height/2f + + gap / 2f, w, h), 
						};
						int removeFlag = -1;
						for (int i = 0; i < 4; i++) {
							Texture2D texture = null;
							if (i < selectingPrefabs.Count) {
								texture = AssetPreview.GetAssetPreview(selectingPrefabs[i]);
							}
							if (texture && (i != 3 || selectingPrefabs.Count == 4)) {
								GUI.DrawTexture(rects[i], texture);
								if (GUI.Button(
									new Rect(rects[i].x + rects[i].width - 24, rects[i].y, 24, 24),
									"×", RemoveBtnStyle)
								) {
									removeFlag = i;
								}
							} else {
								GUI.Label(rects[i], i == 3 && selectingPrefabs.Count > 4 ? ((selectingPrefabs.Count - 3) + " prefabs etc.") : "", InfoStyle);
							}
						}
						if (removeFlag >= 0) {
							selectingPrefabs.RemoveAt(removeFlag);
							Object[] tempObjs = new Object[selectingPrefabs.Count];
							for (int j = 0; j < selectingPrefabs.Count; j++) {
								tempObjs[j] = selectingPrefabs[j].gameObject;
							}
							Selection.objects = tempObjs;
						}
					}
				} else {
					GUI.Label(rect, "Select a Prefab in Project View.", InfoStyle);
				}
			}

			#endregion


			#region --- Fold Btn ---

			{
				Color oldGUIColor = GUI.color;
				GUI.color = AreaAimFading > 0.001f ? oldGUIColor : new Color(1f, 1f, 1f, 0.4f);
				if (GUI.Button(
					GUIRect(0, 18, true),
					AreaFading > halfRant * 0.5f ? "▲" : "▼",
					EditorStyles.miniButtonMid)
				) {
					AreaAimFading = AreaAimFading > 0.001f ? 0f : halfRant;
				}
				GUI.color = oldGUIColor;
			}

			#endregion


			GUILayout.EndArea();


			GUILayout.BeginArea(new Rect(0, Mathf.Lerp(-AREA_HEIGHT + 18f, 0, AreaFading) + AREA_HEIGHT, AREA_WIDTH, 30));


			GUILayout.BeginHorizontal();



			#region --- Done Btn ---


			if (GUI.Button(GUIRect(0, 30, true), "Done")) {
				Done();
			}


			#endregion


			GUILayout.EndHorizontal();


			GUILayout.EndArea();


			if (GUI.changed) {
				Save();
			}

			Handles.EndGUI();

		}

		#endregion


		
	}



	static void OnHierarchyGUI (int instanceID, Rect rect) {
		
		Object obj = EditorUtility.InstanceIDToObject(instanceID);
		
		if (obj && obj is GameObject) {

			Transform tf = (obj as GameObject).transform;
			SimpleMap tfFlag = GetFlag(tf);

			if (tfFlag) {
				if (tfFlag.IsLevel) {
					// Level Btn
					rect.x += rect.width - 38;
					rect.width = 38;
					Color oldColor = GUI.color;
					if (tf == EditingRoot) {
						GUI.color = new Color(0.6f, 1f, 0.6f, 1);
					}
					if (GUI.Button(rect, tf == EditingRoot ? "Done" : "Edit", EditorStyles.miniButtonMid)) {
						if (tf == EditingRoot) {
							Done();
						} else {
							EditMap(tf);
						}
					}
					GUI.color = oldColor;
				} else {
					// Object Tag
					rect.x += rect.width - 4f;
					rect.width = 4f;
					GUI.DrawTexture(rect, ObjectTagTexture);
				}
			}

		}
	}




	static void Update () {

		if (!EditingRoot)
			return;

		// Time
		UpdatingTime -= 0.01f;

		if (UpdatingTime > 0f) {
			return;
		}
		UpdatingTime += SPF;

		//bool needRepaint = false;
		if (!LerpTo(ref AreaFading, AreaAimFading, 0.6f)) {
			NeedRepaint = true;
		}

		if (NeedRepaint) {
			SceneView.RepaintAll();
		}

		

	}





	#endregion



	#region -------- API --------




	[MenuItem("GameObject/Simple Map Editor/New Map", false, 0)]
	[MenuItem("Tools/Simple Map Editor/New Map", false, 0)]
	public static void CreateNewMap () {
		Done(true);
		EditingRoot = new GameObject("New Map", typeof(SimpleMap)).transform;
		EditingRoot.GetComponent<SimpleMap>().IsLevel = true;
		Selection.activeTransform = EditingRoot;
		EditMap(EditingRoot);
		EditorApplication.RepaintHierarchyWindow();
	}




	public static void Done (bool ignoreSaving = false) {

		if (!EditingRoot) {
			return;
		}

		ClearFlagGrid(EditingRoot, RootGrid);
		ClearAllColliders(EditingRoot);
		EditingRoot.rotation = OldMapRot;
		EditingRoot = null;

		EditorApplication.RepaintHierarchyWindow();
		SceneView.RepaintAll();
	}




	public static void EditMap (Transform root) {

		if (EditingRoot) {
			Done();
		}

		SimpleMap flag = GetFlag(root);
		if (flag && flag.IsLevel) {

			// Root
			EditingRoot = root;
			flag.hideFlags = HideFlags.HideInInspector;

			// Repaint
			EditorApplication.RepaintHierarchyWindow();
			SceneView.RepaintAll();

			// Focus
			Transform oldSelect = Selection.activeTransform;
			Selection.activeTransform = root;
			if (SceneView.lastActiveSceneView) {
				SceneView.lastActiveSceneView.FrameSelected(false);
			}
			Selection.activeTransform = oldSelect;

			// Cache
			PaintingID = 0;
			AreaFading = 0f;
			PaintingRot = Quaternion.identity;

			// Grid
			OldGridSize = flag.GridSize;
			GridPosY = 0;
			RootGrid = AddFlagGrid(EditingRoot);

			// Colliders
			AddAllColliders(EditingRoot, flag.GridSize);

			// Snap All
			OldMapRot = EditingRoot.rotation;
			EditingRoot.rotation = Quaternion.identity;
			SnapAllObject(EditingRoot, flag.GridSize);

			
		}
	}



	#endregion



	#region -------- LGC --------



	#region --- System ---


	private static Rect GUIRect (float w, float h, bool exW = false, bool exH = false) {
		return GUILayoutUtility.GetRect(w, h, GUILayout.ExpandWidth(exW), GUILayout.ExpandHeight(exH));
	}


	private static void ForEachWith<T> (Transform root, System.Action<T> action, bool applyForRoot = false) where T : Component {
		if (!root) {
			return;
		}
		T[] comps = root.GetComponentsInChildren<T>(true);
		int num = comps.Length;
		for (int i = 0; i < num; i++) {
			if (applyForRoot || (comps[i] && comps[i].transform != root)) {
				action(comps[i]);
			}
		}
	} 


	private static T[] GetAtPath<T> (string path) where T : Object {

		try {
			ArrayList al = new ArrayList();

			string[] fileEntries = System.IO.Directory.GetFiles(path);

			foreach (string fileName in fileEntries) {
				int assetPathIndex = fileName.IndexOf("Assets");
				string localPath = fileName.Substring(assetPathIndex);

				Object t = AssetDatabase.LoadAssetAtPath<T>(localPath);

				if (t != null)
					al.Add(t);
			}
			T[] result = new T[al.Count];
			for (int i = 0; i < al.Count; i++)
				result[i] = (T)al[i];

			return result;
		} catch {
			return new T[0];
		}

	}


	private static void Load () {

		GridAlpha = EditorPrefs.GetFloat(PREFS_TAG + "GridAlpha", 0.5f);
		AreaAimFading = EditorPrefs.GetFloat(PREFS_TAG + "AreaAimFading", 1f);
		ShowGridBG = EditorPrefs.GetBool(PREFS_TAG + "ShowGridBG", false);
		ShowHighlightFrame = EditorPrefs.GetBool(PREFS_TAG + "ShowHighlightFrame", false);
		
	}


	private static void Save () {

		EditorPrefs.SetFloat(PREFS_TAG + "GridAlpha", GridAlpha);
		EditorPrefs.SetFloat(PREFS_TAG + "AreaAimFading", AreaAimFading);
		EditorPrefs.SetBool(PREFS_TAG + "ShowGridBG", ShowGridBG);
		EditorPrefs.SetBool(PREFS_TAG + "ShowHighlightFrame", ShowHighlightFrame);
		
	}



	#endregion



	#region --- Transform ---


	private static Vector3 LocalToWorld (Vector3 pos, Vector3 rootPos, Quaternion rootRot) {
		return LocalToWorld(pos, rootPos, rootRot, Vector3.one);
	}


	private static Vector3 LocalToWorld (Vector3 pos, Vector3 rootPos, Quaternion rootRot, Vector3 rootScale) {
		pos.Scale(rootScale);
		return rootRot * pos + rootPos;
	}


	private static void LocalToWorld (ref Vector3[] pos, Vector3 rootPos, Quaternion rootRot, Vector3 rootScale) {
		for (int i = 0; i < pos.Length; i++) {
			pos[i] = LocalToWorld(pos[i], rootPos, rootRot, rootScale);
		}
	}


	private static Bounds LocalToWorld(Bounds b, Vector3 rootPos, Quaternion rootRot, Vector3 rootScale){
		Vector3 min = LocalToWorld(b.min, rootPos, rootRot, rootScale);
		Vector3 max = LocalToWorld(b.max, rootPos, rootRot, rootScale);
		b.SetMinMax(
			Vector3.Min(min, max),
			Vector3.Max(min, max)
		);
		return b;
	}


	private static Vector3 WorldToLocal (Vector3 pos, Vector3 rootPos, Quaternion rootRot) {
		return WorldToLocal(pos, rootPos, rootRot, Vector3.one);
	}


	private static Vector3 WorldToLocal (Vector3 pos, Vector3 rootPos, Quaternion rootRot, Vector3 rootScale) {
		Vector3 v = Quaternion.Inverse(rootRot) * (pos - rootPos);
		v.Scale(rootScale);
		return v;
	}


	private static void WorldToLocal (ref Vector3[] pos, Vector3 rootPos, Quaternion rootRot, Vector3 rootScale) {
		for (int i = 0; i < pos.Length; i++) {
			pos[i] = WorldToLocal(pos[i], rootPos, rootRot, rootScale);
		}
	}


	#endregion



	#region --- Math ---


	private static bool LerpTo (ref float f, float aim, float rant = 0.4f) {
		if (Mathf.Abs(f - aim) < 0.01f) {
			if (f == aim) {
				return true;
			} else {
				f = aim;
				return false;
			}
		} else {
			f = Mathf.Lerp(f, aim, rant);
			return false;
		}
	}


	private static Vector3 Cling (Vector3 v, Vector3 normal, Vector3 min, Vector3 max, bool check = true) {

		if (normal == Vector3.up) {
			v.y = max.y;
		} else if (normal == Vector3.down) {
			v.y = min.y;
		} else if (normal == Vector3.left) {
			v.x = min.x;
		} else if (normal == Vector3.right) {
			v.x = max.x;
		} else if (normal == Vector3.forward) {
			v.z = max.z;
		} else if (normal == Vector3.back) {
			v.z = min.z;
		} else if (check) {
			Quaternion rot = SnapRotation(Quaternion.FromToRotation(Vector3.up, normal), Quaternion.identity);
			normal = rot * Vector3.up;
			v = Cling(v, normal, min, max, false);
		}

		return v;
	}


	private static Bounds? Intersection (Bounds a, Bounds b) {
		Bounds c = new Bounds();
		if (a.Intersects(b)) {
			c.SetMinMax(Vector3.Max(a.min, b.min), Vector3.Min(a.max, b.max));
			return c;
		} else {
			return null;
		}
		
	}


	private static void RandomPaintingID (int max) {
		long tick = System.DateTime.Now.Ticks;
		System.Random random = new System.Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
		PaintingID = random.Next() % max;
	}


	#endregion



	#region --- Snap ---


	private static Vector3 Snap (Vector3 v, Vector3 size, Vector3 offset, bool snapY = true) {
		return new Vector3(
			Mathf.Floor(v.x / size.x) * size.x,
			snapY ? Mathf.Floor(v.y / size.y) * size.y : v.y,
			Mathf.Floor(v.z / size.z) * size.z
		) + offset;
	}



	private static Vector3 Snap (Vector3 v, float size,  Vector3 offset, bool snapY = true) {
		return Snap(v, Vector3.one * size, offset, snapY);
	}



	private static Vector3 Round (Vector3 v, float size, Vector3 offset, bool roundY = true) {
		return Round(v, Vector3.one * size, offset, roundY);
	}


	private static Vector3 Round (Vector3 v, Vector3 size, Vector3 offset, bool roundY = true) {
		return new Vector3(
			Mathf.Round(v.x / size.x) * size.x,
			roundY ? Mathf.Round(v.y / size.y) * size.y : v.y,
			Mathf.Round(v.z / size.z) * size.z
		) + offset;
	}


	private static Quaternion SnapRotation (Quaternion r, Quaternion rootRot) {

		Quaternion q = r * Quaternion.Inverse(rootRot);

		q = Quaternion.Euler(
			Mathf.Floor((q.eulerAngles.x + 45f) / 90f) * 90f,
			Mathf.Floor((q.eulerAngles.y + 45f) / 90f) * 90f,
			Mathf.Floor((q.eulerAngles.z + 45f) / 90f) * 90f
		);

		return q * rootRot;
	}


	#endregion



	#region --- Flag ---


	private static SimpleMap GetFlag (Transform tf) {
		return tf ? tf.GetComponent<SimpleMap>() : null;
	}


	private static Transform AddFlagGrid (Transform root) {

		ClearFlagGrid(root);
		Transform tf = new GameObject("GridBG", typeof(MeshFilter), typeof(MeshRenderer)).transform;
		tf.SetParent(root);
		tf.localPosition = Vector3.zero;
		tf.localRotation = Quaternion.identity;
		tf.localScale = Vector3.one;
		tf.gameObject.hideFlags = HideFlags.HideAndDontSave;
		tf.GetComponent<MeshRenderer>().material = GridBGMat;
		tf.GetComponent<MeshFilter>().mesh = GridMesh;

		Transform subTf = new GameObject("Grid", typeof(MeshFilter), typeof(MeshRenderer)).transform;
		subTf.SetParent(tf);
		subTf.localPosition = Vector3.up * 0.001f;
		subTf.localRotation = Quaternion.identity;
		subTf.localScale = Vector3.one;
		subTf.GetComponent<MeshRenderer>().material = GridMat;
		subTf.GetComponent<MeshFilter>().mesh = GridMesh;

		BoxCollider bc = tf.gameObject.AddComponent<BoxCollider>();
		bc.center = new Vector3(0.5f, 0, 0.5f);
		bc.size = new Vector3(1, 0.001f, 1);
		bc.isTrigger = true;

		return tf;
	}


	private static void ClearFlagGrid (Transform root, Transform firstFind = null) {
		if (firstFind) {
			GameObject.DestroyImmediate(firstFind.gameObject, false);
		}
		ForEachWith<MeshFilter>(root, (mf) => {
			if (mf.gameObject.hideFlags == HideFlags.HideAndDontSave) {
				GameObject.DestroyImmediate(mf.gameObject, false);
			}
		}, false);
	}

	 

	private static void AddCollider (Transform tf, float gridSize) {
		BoxCollider bc = tf.gameObject.AddComponent<BoxCollider>();
		bc.isTrigger = true;
		Bounds b = GetMeshBounds(tf, gridSize);
		bc.center = b.center;
		bc.size = b.size;
		bc.hideFlags = HideFlags.HideInInspector;
	}


	private static void AddAllColliders (Transform root, float gridSize) {
		ClearAllColliders(root);
		ForEachWith<SimpleMap>(root, (sm) => {
			if (!sm.IsLevel) {
				AddCollider(sm.transform, gridSize);
			}
		}, false);
		GameObject g = new GameObject("", typeof(BoxCollider));
		UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(g.GetComponent<BoxCollider>(), false);
		GameObject.DestroyImmediate(g, false);
	}


	private static void ClearAllColliders (Transform root) {
		ForEachWith<BoxCollider>(root, (bc) => {
			if (bc.hideFlags == HideFlags.HideInInspector) {
				GameObject.DestroyImmediate(bc, false);
			}
		}, false);
	}


	#endregion



	#region --- Object Util ---


	private static void Spawn (GameObject source, Transform parent, Vector3 pos, Quaternion rot, float gridSize) {
		Transform tf = (PrefabUtility.InstantiatePrefab(source) as GameObject).transform;
		tf.name = source.name;
		tf.SetParent(parent);
		tf.position = pos;
		tf.rotation = rot;
		tf.gameObject.AddComponent<SimpleMap>().IsLevel = false;
		Undo.RegisterCreatedObjectUndo(tf.gameObject, "Create object");

		AddCollider(tf, gridSize);
		SnapObject(tf, parent, gridSize);

	}


	private static void SnapAllObject (Transform root, float gridSize) {
		ForEachWith<SimpleMap>(root, (sm) => {
			if (!sm.IsLevel) {
				SnapObject(sm.transform, root, gridSize);
			}
		}, false);
	}


	private static void SnapObject (Transform tf, Transform root, float gridSize) {

		Vector3 rootPos = root.position;
		Quaternion rootRot = root.rotation;
		
		tf.position = LocalToWorld(
			Snap(
				WorldToLocal(tf.position, rootPos, rootRot),
				gridSize,
				new Vector3(gridSize * 0.5f, 0, gridSize * 0.5f),
				true
			),
		rootPos, rootRot);

		Quaternion rot = SnapRotation(tf.rotation, rootRot);
		rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
		tf.rotation = rot;

		tf.localScale = Vector3.one;
		
	}


	private static GameObject[] TryGetObjects (Bounds zone, float gridSize) {
		
		RaycastHit[] hits = Physics.BoxCastAll(
			zone.center - Vector3.up * (zone.extents.y + gridSize * 0.1f),
			new Vector3(zone.extents.x - gridSize * 0.01f, gridSize * 0.01f, zone.extents.z - gridSize * 0.01f),
			Vector3.up,
			Quaternion.identity,
			zone.extents.y * 2f + gridSize * 0.1f,
			-1,
			QueryTriggerInteraction.Collide
		);

		List<GameObject> objs = new List<GameObject>();

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider.hideFlags == HideFlags.HideInInspector) {
				objs.Add(hits[i].transform.gameObject);
			}
		}

		return objs.ToArray();
	}


	private static Bounds GetMeshBounds (Transform root, float gridSize) {

		Bounds bound = new Bounds(Vector3.up * 0.5f * gridSize, Vector3.one * gridSize);

		ForEachWith<MeshFilter>(root, (mf) => {
			Vector3 min, max;
			GetMeshMargin(mf.sharedMesh, mf.transform, root, gridSize, out min, out max);
			Bounds b = new Bounds();
			b.SetMinMax(min, max);
			bound.Encapsulate(b);
		}, true);

		return bound;


	}


	private static void GetMeshMargin (Mesh mesh, Transform tf, Transform axis, float gridSize, out Vector3 min, out Vector3 max) {
		
		if (!mesh) {
			min = max = Vector3.zero;
			return;
		}

		min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

		Vector3[] vs = mesh.vertices;

		if (axis != tf) {
			LocalToWorld(ref vs, tf.position, tf.rotation, tf.lossyScale);
			WorldToLocal(ref vs, axis.position, axis.rotation, axis.lossyScale);
		}

		if (gridSize > 0) {

			for (int i = 0; i < vs.Length; i++) {

				vs[i] = Round(
					vs[i] + new Vector3(0.5f, 0, 0.5f) * gridSize,
					gridSize,
					Vector3.zero, true
				) - new Vector3(0.5f, 0, 0.5f) * gridSize;

			}

		}
		

		foreach (Vector3 v in vs) {
			min = Vector3.Min(min, v);
			max = Vector3.Max(max, v);
		}

	}





	#endregion




	#endregion



}




#endif

}
