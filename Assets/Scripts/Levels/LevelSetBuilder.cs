using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Levels
{
    public class LevelSetBuilder : MonoBehaviour, IBuilder<LevelSet>
    {
        [SerializeField]
        private LevelSet current;
        public Transform levelBuilderPrefab;

        public void Clear()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform) {
                children.Add(child);
            }
            Undo.RecordObjects(children.ToArray(), "Clear LevelSetBuilder");
            foreach (Transform child in children)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        public LevelSet GetCurrent()
        {
            return current;
        }

        public void Load(LevelSet data)
        {
            current = data;
            Reload();
        }

        public void Reload()
        {
            if (levelBuilderPrefab == null || levelBuilderPrefab.GetComponent<LevelBuilder>() == null)
            {
                Debug.LogWarning(gameObject.name + ": LevelSet load failed! Attach a valid builder prefab.");
            }
            Clear();

            foreach (LevelChunk chunk in current.chunks)
            {
                Transform target = PrefabUtility.InstantiatePrefab(levelBuilderPrefab) as Transform;
                target.parent = transform;
                target.localPosition = chunk.offset;

                LevelBuilder script = target.GetComponent<LevelBuilder>();
                script.Load(chunk.level);
            }
        }

        public void SaveChunks()
        {
            foreach (LevelBuilder builder in GetComponentsInChildren<LevelBuilder>())
            {
                builder.Save();
            }
        }

        public void Save()
        {
            List<LevelChunk> chunks = new List<LevelChunk>();
            foreach (LevelBuilder builder in GetComponentsInChildren<LevelBuilder>())
            {
                chunks.Add(new LevelChunk(builder.GetCurrent(), builder.transform.localPosition));
            }

            Undo.RecordObject(current, "Save Level Set Data");
            current.chunks = chunks.ToArray();
            EditorUtility.SetDirty(current);
            AssetDatabase.SaveAssets();
        }

        public void SetCurrent(LevelSet data)
        {
            current = data;
        }
    }
}