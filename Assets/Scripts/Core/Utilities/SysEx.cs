#region

using UnityEngine;

#endregion

namespace Core.Utilities
{
    public static class SysEx
    {
        public static class Unity
        {
            public static float ToDeltaTime => Time.deltaTime * 1_000f;

            public struct ZIndex
            {
                public static readonly ZIndex Background = new(0.5f);
                public static readonly ZIndex Default = new(0.0f);
                public static readonly ZIndex Map = new(-0.2f);
                public static readonly ZIndex Item = new(-0.3f);
                public static readonly ZIndex Character = new(-0.5f);
                public static readonly ZIndex MainCharacter = new(-0.51f);
                public static readonly ZIndex Foreground = new(-0.8f);

                private ZIndex(float index)
                {
                    this.index = index;
                }

                private readonly float index;

                public static implicit operator float(ZIndex zIndex) => zIndex.index;
            }
        }
        // TODO: Implement System environment getter
    }
}