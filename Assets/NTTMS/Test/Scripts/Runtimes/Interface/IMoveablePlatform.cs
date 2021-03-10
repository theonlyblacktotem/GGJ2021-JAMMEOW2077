using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTTMS.Test
{
    public interface IMoveablePlatform
    {
        void RegisterStandFollow(IMoveable hMoveable);
        void UnregisterStandFollow(IMoveable hMoveable);
    }
}
