Shader "Graph/Point Surface GPU"  {

	Properties{
		_Smoothness("Smoothness", Range(0,1)) = 0.5
		}

	SubShader{
	CGPROGRAM
	#pragma surface ConfigureSurface Standard fullforwardshadows addshadow
	#pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural 
	#pragma target 4.5 

	
	struct Input{
		float3 worldPos;
	};
	
	float _Smoothness;

	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float3> _Position;
	#endif
	float _Step;
	void ConfigureProcedural(){
		#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			float3 position = _Position[unity_InstanceID];
			unity_ObjectToWorld = 0.0;
			unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
			unity_ObjectToWorld._m00_m11_m22 = _Step;
		#endif
	}

	void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
		surface.Albedo = (input.worldPos * 0.5 + 0.5);
		surface.Smoothness = _Smoothness;
		}

	

	ENDCG
	}
	FallBack "Diffuse"
}
