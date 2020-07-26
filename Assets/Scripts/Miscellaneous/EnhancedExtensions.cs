using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedExtensions
{
    public static class EnhancedExtensions
    {
        public static bool IsFree(this Spell spell)
        {
            return spell == null;
        }
    }
}
