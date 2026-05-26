using System;
using UnityEngine;

namespace Anvil
{
    public interface IAuthorer<T>
    {
        public T authoringObject { get; }

    }
}
