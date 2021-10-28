using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GamerWolf.Utils{
    public interface IPooledObject{
        void OnObjectReuse();
        void DestroyMySelf();
    }
}
