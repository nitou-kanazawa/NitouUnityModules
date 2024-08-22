using System;
using UnityEngine;
using UniRx;

// [éQçl]

namespace nitou.SceneSystem{

    public interface ISceneEntity<TScene>
        where TScene : Enum{
        
        IReactiveProperty<TScene> SceneRP { get; }
        void ChangeScene(TScene targetScene);
    }


    public abstract class SceneEntity<TScene> 
        where TScene : Enum{

        /// <summary>
        /// 
        /// </summary>
        public IReactiveProperty<TScene> SceneRP { get; }

        public void ChangeScene(TScene targetScene) {
            SceneRP.Value = targetScene;
        }
    }



}
