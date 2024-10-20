using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace nitou.Timeline {

    [DisplayName("Jump/DestinationMarker")]
    //[CustomStyle("DestinationMarker")]
    public class DestinationMarker : Marker {

        [SerializeField] public bool active;

        void Reset() {
            active = true;
        }

    }

}