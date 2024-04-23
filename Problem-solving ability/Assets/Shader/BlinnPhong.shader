Shader "BlinnPhong/YellowBlinnPhongShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1, 1, 1, 1)
        _LightDirection("LightDirection", Vector) = (0, 1, 0, 0)
        _SpecularColor("SpecularColor", Color) = (1, 1, 1, 1)
        _Shininess("Shininess", Float) = 32
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD0;
            };

            float4 _DiffuseColor;
            float4 _SpecularColor;
            float _Shininess;
            float4 _LightDirection;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDirection);
                float3 normal = normalize(i.normal);
                float3 viewDir = normalize(i.viewDir);
                float3 halfwayDir = normalize(lightDir + viewDir);

                float diffuseIntensity = max(dot(normal, lightDir), 0);
                float specularIntensity = pow(max(dot(normal, halfwayDir), 0), _Shininess);

                fixed4 diffuse = _DiffuseColor * diffuseIntensity;
                fixed4 specular = _SpecularColor * specularIntensity;

                return diffuse + specular;
            }
            ENDCG
        }
    }
}
