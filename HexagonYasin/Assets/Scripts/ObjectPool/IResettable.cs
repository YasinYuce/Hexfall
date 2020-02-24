using System;
using UnityEngine;

namespace YasinYuce
{
    public interface IResettable
    {
        Action Release { get; set; }

        void Reset();
    }
}