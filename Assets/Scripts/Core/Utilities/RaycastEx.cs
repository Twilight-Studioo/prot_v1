#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Core.Utilities
{
    public static class RaycastEx
    {
        private static Vector2 lastOrigin;
        private static float lastPointSize;

        /// <summary>
        ///     指定オブジェクトの下方向に指定距離内に他のオブジェクトが存在するかどうかを判定します。
        /// </summary>
        /// <param name="targetObj">判定対象のオブジェクト</param>
        /// <param name="distance">判定距離</param>
        /// <param name="layerMask">判定対象となるレイヤーのマスク</param>
        /// <returns>下方向に指定距離内に他のオブジェクトが存在するかどうか</returns>
        public static bool IsGroundBelow(this GameObject targetObj, float distance)
        {
            // Raycastを飛ばす方向
            var direction = Vector3.down;

            // Raycastの開始位置
            var startPosition = targetObj.transform.position;

            // Raycastを実行
            var isHit = Physics.Raycast(startPosition, direction, out var hit, distance);

            // Raycastが当たった場合は、地面が存在すると判定
            if (isHit)
            {
                return true;
            }

            // Raycastが当たらない場合は、地面が存在しないと判定
            return false;
        }

        public static bool FindObjectWithPosition(Vector2 origin, float pointSize, ref List<GameObject> result)
        {
            // Initialize the result list
            if (result == null)
            {
                result = new();
            }
            else
            {
                result.Clear(); // Clear the list to remove any old data
            }

            lastOrigin = origin;
            lastPointSize = pointSize;
            // Find all colliders at the specified position within the given radius
            var colliders = new Collider2D[10];
            var size = Physics2D.OverlapCircleNonAlloc(origin, pointSize, colliders);

            if (size == 0)
            {
                return false;
            }

            // Add all objects found at the position to the result list
            result.AddRange(colliders.Where(collider => collider != null).Select(collider => collider.gameObject));

            return true;
        }

        public static void DrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastOrigin, lastPointSize);
        }
    }
}