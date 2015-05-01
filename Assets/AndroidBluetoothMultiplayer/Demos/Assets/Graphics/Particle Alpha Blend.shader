Shader "Mobile/Particles/Alpha Blended Tinted" {
Properties {
	_Color ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	Cull Off 
	Lighting Off 
	ZWrite Off
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	
	// ---- Dual texture cards
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				constantColor [_Color]
				combine constant * primary
			}
			SetTexture [_MainTex] {
				combine texture * previous
			}
		}
	}
}
}
