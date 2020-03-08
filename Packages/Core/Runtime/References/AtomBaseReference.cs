using UnityEngine;

namespace UnityAtoms
{
    public class AtomReferenceUsage
    {
        public const int VALUE = 0;
        public const int CONSTANT = 1;
        public const int VARIABLE = 2;
        public const int VARIABLE_INSTANCER = 3;
    }

    /// <summary>
    /// None generic base class for `AtomReference&lt;T, C, V, E1, E2, F, VI&gt;`.
    /// </summary>
    public abstract class AtomBaseReference
    {
        /// <summary>
        /// Describes how we use the Reference and where the value comes from.
        /// </summary>
        [SerializeField]
        protected int _usage;
    }
}