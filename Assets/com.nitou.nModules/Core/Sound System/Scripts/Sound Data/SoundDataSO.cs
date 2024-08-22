using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.Sound {

    /// <summary>
    /// AudioClipに付加情報を加えた音源データオブジェクト
    /// </summary>
    [CreateAssetMenu(
        fileName ="New SoundData", 
        menuName = AssetMenu.Prefix.ScriptableObject + "Sound/Sound Data"
    )]
    public class SoundDataSO : ScriptableObject {

        /// ----------------------------------------------------------------------------
        // Field

        [Header("Sound data")]
  
        [SerializeField] private string _id;
        
        [SerializeField] private SoundType _soundType = SoundType.SE;
        
        [SerializeField] private AudioClip _audioClip;


        [Header("")]

        [SerializeField] private string _title = "unknown";
        
        [TextArea(4,8)]
        [SerializeField] private string _copylight = "unknown";


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// サウンドID
        /// </summary>
        public string ID => _id;

        /// <summary>
        /// 音源
        /// </summary>
        public AudioClip Clip => _audioClip;


    }

}