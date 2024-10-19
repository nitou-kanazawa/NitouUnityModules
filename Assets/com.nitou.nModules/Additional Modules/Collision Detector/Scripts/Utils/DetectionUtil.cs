using UnityEngine;

namespace nitou.Detecor {

    /// <summary>
    /// キャッシュするターゲットの種類．
    /// 多重判定等を避けるためのキャッシュに用いる．
    /// </summary>
    public enum CachingTarget {
        RootObject = 0,                     // Cache the root object
        Collider = 1,                       // Cache colliders
        Rigidbody = 2,                      // Cache objects with Rigidbody
        CharacterController = 3,            // cache object with character controller.
        RigidbodyOrCharacterController = 4  // Rigidbody or Character controller.
    }


    /// <summary>
    /// 
    /// </summary>
    public static class DetectionUtil {

        /// <summary>
        /// 検出されたコライダーから，そのコライダーを保有する<see cref="GameObject"/>を取得する
        /// </summary>
        public static GameObject GetHitObject(this Collider hitCollider, CachingTarget targetType) {

            switch (targetType) {

                case CachingTarget.RootObject: {
                        return hitCollider.transform.root.gameObject;
                    }

                case CachingTarget.Rigidbody: {
                        // ※Rigidbodyがnullの場合はコライダーを返す
                        var rigidbody = hitCollider.attachedRigidbody;
                        return (rigidbody != null) ? rigidbody.gameObject : hitCollider.gameObject;
                    }

                case CachingTarget.CharacterController: {
                        // ※Controllerがnullの場合はコライダーを返す
                        var controller = hitCollider.GetComponentInParent<CharacterController>();
                        return (controller != null) ? controller.gameObject : hitCollider.gameObject;
                    }

                case CachingTarget.RigidbodyOrCharacterController: {
                        // Rigidbody
                        var rigidbody = hitCollider.attachedRigidbody;
                        if (rigidbody != null) return rigidbody.gameObject;

                        // Charactor Controller
                        var controller = hitCollider.GetComponentInParent<CharacterController>();
                        if (controller != null) return controller.gameObject;

                        // ※どちらもnullならコライダーを返す
                        return hitCollider.gameObject;
                    }

                default:
                    return hitCollider.gameObject;
            }

        }



    }
}
