using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    namespace UI
    {
        namespace TweenCoroutine
        {
            internal interface ITweenValue
            {
                bool ignoreTimeScale
                {
                    get;
                }

                float duration
                {
                    get;
                }

                void TweenValue(float floatPercentage);

                bool ValidTarget();
            }
        }
    }
}