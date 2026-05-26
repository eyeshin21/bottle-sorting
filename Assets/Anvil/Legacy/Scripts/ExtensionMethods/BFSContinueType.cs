using UnityEngine;

namespace Anvil.Legacy
{
    public enum BFSContinueType
    {
        Continue,
        ContinueSkipChildren,
        Break,
    }

    public static partial class ExtensionMethods
    {
        public static BFSContinueType ToBFSContinueType(this bool continueSkipChildren)
        {
            return continueSkipChildren ? BFSContinueType.ContinueSkipChildren : BFSContinueType.Continue;
        }
    }
}