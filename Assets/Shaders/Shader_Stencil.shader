Shader "Custom/Shader_Stencil"
{
    Properties
    {
        _mainTex("Main Texture", 2D) = "white" {}
        _color("Color", Color) = (1, 1, 1,1)
        _normal("Normal", 2D) = "bump" {}
        _normalStrength("Normal Strength", Range(0, 10)) = 1
        [IntRange] _stencilRef ("Stencil Ref Value", Range(0, 255)) = 0
    }
    
    SubShader
    {
        Tags{ "RenderType"="Opaque" "Queue"="Geometry-1"}
        
        Stencil
        {
            Pass Keep
            Comp NotEqual
            Ref [_stencilRef]
        }
        CGPROGRAM
        
        #pragma surface surf Lambert

        sampler2D _mainTex;
        sampler2D _normal;
        float4 _color;
        float _normalStrength;
        
        
        struct Input
        {
            float2 uv_mainTex;
            float2 uv_normal;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = (tex2D(_mainTex, IN.uv_mainTex)).rgb * _color.rgb;
            o.Normal = UnpackNormal(tex2D(_normal, IN.uv_normal));
            o.Normal *= float3(_normalStrength, _normalStrength, 1);
        }
         ENDCG
    }
    FallBack "Diffuse"
}
