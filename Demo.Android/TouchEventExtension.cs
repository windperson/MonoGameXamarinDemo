using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Input.Touch;

namespace Demo.Android
{
    public static class TouchEventExtension
    {
        public static bool HasAnyActualTouch(this TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return true;
                }
            }
            return false;
        }

        public static TouchLocation? GetLastTouch(this TouchCollection touchLocation)
        {
            if (!HasAnyActualTouch(touchLocation)) { return null; }

            if (touchLocation.Count == 1)
            {
                return touchLocation[0];
            }

            return touchLocation[touchLocation.Count - 1];
        }
    }
}