using UnityEngine;

namespace nitou.LevelActors.Inputs {

    /// <summary>
    /// This ScriptableObject contains all the names used as input actions by the human brain. The name of the action will matters depending on the input handler used.
    /// </summary>
    [CreateAssetMenu(
        fileName = "LevelActorActionsAsset",
        menuName = AssetMenu.Prefix.ScriptableObject + "LevelActor actions asset"
    )]
    public class ActorActionsAsset : ScriptableObject {

        [SerializeField] string[] boolActions;
        [SerializeField] string[] floatActions;      
        [SerializeField] string[] vector2Actions;
    }

}