using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class LevelExitBehaviour : MonoBehaviour
    {
        public LevelStartLocation startLocation;
        public LayerMask triggerMask;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            LevelBuilder builder = transform.root.GetComponent<LevelBuilder>();
            if (builder == null || startLocation == null || startLocation.targetLevel == null)
            {
                Debug.Log("Error finding next Start Location!");
                return;
            }
            builder.Load(startLocation.targetLevel);
            Instantiate(builder.playerPrefab, builder.transform.TransformPoint(startLocation.position), Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}