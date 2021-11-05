using UnityEngine;

namespace SPModelImporter.Editor
{
    public static class ShaderProperty
    {
#if UNITY_RP_URP
        private static readonly int URPBaseMap = Shader.PropertyToID("_BaseMap");
        private static readonly int URPMetallicMap = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int URPRoughnesMap = Shader.PropertyToID("_SpecGlossMap");
        private static readonly int URPNormalMap = Shader.PropertyToID("_BumpMap");
        private static readonly int URPHeightMap = Shader.PropertyToID("_ParallaxMap");
        private static readonly int URPAOMap = Shader.PropertyToID("_OcclusionMap");
        private static readonly int URPEmissionMap = Shader.PropertyToID("_EmissionMap");

#elif UNITY_RP_HDRP
        private static readonly int HDRPBaseMap = Shader.PropertyToID("_BaseColorMap");
        private static readonly int HDRPMetallicMap = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int HDRPRoughnesMap = Shader.PropertyToID("_MaskMap");
        private static readonly int HDRPNormalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int HDRPHeightMap = Shader.PropertyToID("_HeightMap");
        // private static readonly int HDRPAOMap = Shader.PropertyToID("_OcclusionMap");
        private static readonly int HDRPEmissionMap = Shader.PropertyToID("_EmissiveColorMap");
#else
        private static readonly int StandardBaseMap = Shader.PropertyToID("_BaseMap");
        private static readonly int StandardMetallicMap = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int StandardRoughnesMap = Shader.PropertyToID("_SpecGlossMap");
        private static readonly int StandardNormalMap = Shader.PropertyToID("_BumpMap");
        private static readonly int StandardHeightMap = Shader.PropertyToID("_ParallaxMap");
        private static readonly int StandardAOMap = Shader.PropertyToID("_OcclusionMap");
        private static readonly int StandardEmissionMap = Shader.PropertyToID("_EmissionMap");
#endif


        public static int BaseMap =>
        #if UNITY_RP_URP
        URPBaseMap;
        #elif UNITY_RP_HDRP
        HDRPBaseMap;
        #else
        StandardBaseMap;
        #endif

        public static int MetallicMap =>
        #if UNITY_RP_URP
        URPMetallicMap;
        #elif UNITY_RP_HDRP
        HDRPBaseMap;
        #else
        StandardBaseMap;
        #endif

        public static int RoughnesMap =>
        # if UNITY_RP_URP
        URPRoughnesMap;
        #elif UNITY_RP_HDRP
        HDRPRoughnesMap;
        #else
        StandardRoughnesMap;
        #endif
        public static int NormalMap =>
        # if UNITY_RP_URP
        URPNormalMap;
        #elif UNITY_RP_HDRP
        HDRPNormalMap;
        #else
        StandardNormalMap;
        #endif
        public static int HeightMap =>
        # if UNITY_RP_URP
        URPHeightMap;
        #elif UNITY_RP_HDRP
        HDRPHeightMap;
        #else
        StandardHeightMap;
        #endif
        # if UNITY_RP_URP        
        public static int AOMap => URPAOMap;
        // StandardAOMap;
        #endif
        public static int EmissionMap =>
        # if UNITY_RP_URP
        URPEmissionMap;
        #elif UNITY_RP_HDRP
        HDRPEmissionMap;
        #else
        StandardEmissionMap;
        #endif
    }
}