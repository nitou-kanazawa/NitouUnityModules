using UnityEngine;

namespace nitou.HitSystem {

    /// <summary>
    /// Enumeration of caching target types.
    /// Caches objects under specific conditions to avoid multiple hits.
    /// </summary>
    public enum CachingTarget {
        RootObject = 0,                     // Cache the root object
        Collider = 1,                       // Cache colliders
        Rigidbody = 2,                      // Cache objects with Rigidbody
        CharacterController = 3,            // cache object with character controller.
        RigidbodyOrCharacterController = 4  // Rigidbody or Character controller.
    }


    /// <summary>
    /// 干渉検出関連の汎用メソッド集
    /// </summary>
    public static class DetectiorUtil {

        /// ----------------------------------------------------------------------------
        #region Detection



        #endregion


        /// <summary>
        /// 検出されたコライダーから，そのコライダーを保有する<see cref="GameObject"/>を取得する
        /// </summary>
        public static GameObject GetHitObject(this Collider hitCol, CachingTarget targetType) {

            switch (targetType) {
                case CachingTarget.RootObject: {
                        // コライダーのルートを返す
                        return hitCol.transform.root.gameObject;
                    }
                case CachingTarget.Rigidbody: {
                        // ※Rigidbodyがnullの場合，コライダーを返す.
                        var attachedRigidbody = hitCol.attachedRigidbody;
                        return (attachedRigidbody != null) ? attachedRigidbody.gameObject : hitCol.gameObject;
                    }
                case CachingTarget.CharacterController: {
                        // If the parent object of the collided target has a CharacterController, return the object with the CharacterController attached.
                        var controller = hitCol.transform.GetComponentInParent<CharacterController>();
                        return controller != null ? controller.gameObject : hitCol.gameObject;
                    }
                case CachingTarget.RigidbodyOrCharacterController: {
                        // If the parent object has a Rigidbody, return the object with the Rigidbody attached.
                        var attachedRigidbody = hitCol.attachedRigidbody;
                        if (attachedRigidbody != null)
                            return attachedRigidbody.gameObject;

                        // If the parent object has a CharacterController, return the object with the CharacterController attached.
                        var controller = hitCol.transform.GetComponentInParent<CharacterController>();
                        if (controller != null)
                            return controller.gameObject;

                        // If none of the above conditions are met, return the current Collider.
                        return hitCol.gameObject;
                    }
                default: {
                        // コライダーのGameObjectを返す
                        return hitCol.gameObject;
                    }
            }

        }
    }
}
