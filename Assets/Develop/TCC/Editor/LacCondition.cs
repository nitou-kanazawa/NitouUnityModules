using UnityEngine;

namespace nitou.LevelActors.EditorScripts
{
    using nitou.LevelActors.Attributes;
    using System.Collections.Generic;

    internal class LacCondition{

        /// <summary>
        /// True if there are components with errors.
        /// </summary>
        public bool HasErrorMessage { get; private set; } = false;

        /// <summary>
        /// List of error messages.
        /// </summary>
        public List<string> ErrorMessages { get; } = new();

    }
}
