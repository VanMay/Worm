#ifndef _MYLIB_CGINC
#define _MYLIB_CGINC

inline float GetFresnel(float3 viewDir, float3 normal) {
	return 0.55 + 0.5 * (pow(saturate(1 - dot(viewDir, normal)), 2));
}

#endif