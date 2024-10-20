using UnityEngine;
using UnityEngine.Timeline;

namespace nitou.Timeline {

    [CustomStyle("Annotation")]
    public class Annotation : Marker {
        [TextArea] public string annotation;

    }

}