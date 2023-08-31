#pragma multi_compile_instancing
#pragma require 2darray
#pragma target 3.5

UNITY_DECLARE_TEX2DARRAY(_AnimTextures);
UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimTimeInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimScalar)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimTimeInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimScalar)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeData)
UNITY_INSTANCING_BUFFER_END(Props)

inline float GetPixelOffset(inout float textureIndex, float4 animInfo, float4 animTimeInfo)
{
	const float wrapMode = animInfo.w;	
	const float isPlayingBackwards = animTimeInfo.w < animTimeInfo.z;
	const float animRunTime = lerp(
		(_Time.y - animTimeInfo.z) / (animTimeInfo.w - animTimeInfo.z),
		(_Time.y - animTimeInfo.z) / (animTimeInfo.z - animTimeInfo.w),
		isPlayingBackwards);
	const float isNormalizedLooping = animRunTime > 0.9999;// step(0.9999, animRunTime);
	const float isLoopingWrapMode = step(2.0, wrapMode);
	const float isPingPong = step(4.0, wrapMode);
	const float isEvenLoop = step(0.5, floor(animRunTime) % 2);
	const float invertPingPongLoop = step(1.5, isPingPong + isEvenLoop);
	const float normalizedLoopTime = animRunTime - floor(animRunTime);
	// check wrap mode of animation
	float normalizedTime = lerp(lerp(normalizedLoopTime, 1.0, isNormalizedLooping), // time if clamped, 0 -> 1
		normalizedLoopTime, // time if looping or ping pong		
		isLoopingWrapMode); // check for looping or ping pong
	// if ping pong and animRunTime is in an even loop, invert normalized time
	normalizedTime = lerp(normalizedTime, // not ping pong
		1.0 - normalizedTime, // ping ponging
		invertPingPongLoop);
	normalizedTime = lerp(normalizedTime, // not ping pong
		1.0 - normalizedTime, // ping ponging
		isPlayingBackwards);
	
	const float currentFrame = min(normalizedTime * animTimeInfo.y, animTimeInfo.y - 1);
	const float vertexCount = animInfo.y;
	const float textureSize = animInfo.z;
	const float framesPerTexture = floor((textureSize * textureSize) / (vertexCount * 2));
    const float textureIndexOffset = floor(currentFrame / framesPerTexture);
    textureIndex = floor(textureIndex + textureIndexOffset + 0.1);
	const float frameOffset = floor(currentFrame % framesPerTexture);
    const float pixelOffset = floor(vertexCount * 2 * frameOffset + 0.5);
	return pixelOffset;
}

inline float3 GetUVPos(uint vertexIndex, float textureIndex, float pixelOffset, float textureSize, uint offset)
{
	uint vertexOffset = pixelOffset + (vertexIndex * 2);
	vertexOffset += offset;
	float offsetX = floor(vertexOffset / textureSize);
	float offsetY = vertexOffset - (offsetX * textureSize);
	float3 uvPos = float3(offsetX / textureSize, offsetY / textureSize, textureIndex);
	return uvPos;
}

inline float3 GetAnimationUVPosition(uint vertexIndex, float4 animInfo, float4 animTimeInfo, uint offset)
{
	float textureIndex = animInfo.x - 1.0;
    const float pixelOffset = GetPixelOffset(textureIndex, animInfo, animTimeInfo);
	return GetUVPos(vertexIndex, textureIndex, pixelOffset, animInfo.z, offset);
}

inline float4 DecodeNegativeVectors(float4 positionData)
{
	positionData = float4((positionData.x - 0.5) * 2, (positionData.y - 0.5) * 2, (positionData.z - 0.5) * 2, 1);
	return positionData;
}

inline float4 ApplyAnimationScalar(float4 positionData, float4 animScalar)
{
	positionData = DecodeNegativeVectors(positionData);
	positionData.xyz *= animScalar.xyz;
	return positionData;
}

inline float4 ApplyMeshAnimation(float4 position, uint vertexId)
{
	float4 animInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimInfo);
	if (animInfo.x >= 1.0)
	{
		float4 animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimTimeInfo);

		float3 uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 0);
		float4 positionData = UNITY_SAMPLE_TEX2DARRAY_LOD(_AnimTextures, uvPos, 0);
		float4 animScalar = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimScalar);
		positionData = ApplyAnimationScalar(positionData, animScalar);

		const float4 crossfadeData = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeData);		
		const float crossfadeEndTime = crossfadeData.x + crossfadeData.z;	
		if (crossfadeEndTime > _Time.y)
		{			
			animInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimInfo);
			animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimTimeInfo);
			uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 0);
			float4 crossfadePositionData = UNITY_SAMPLE_TEX2DARRAY_LOD(_AnimTextures, uvPos, 0);
			animScalar = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimScalar);
			crossfadePositionData = ApplyAnimationScalar(crossfadePositionData, animScalar);

			const float blendWeight = (crossfadeEndTime - _Time.y) / crossfadeData.z;
			positionData = lerp(crossfadePositionData, positionData, 1.0 - blendWeight);
		}
		position = positionData;
	}
	return position;
}

inline float3 GetAnimatedMeshNormal(float3 normal, uint vertexId)
{
	float4 animInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimInfo);
	if (animInfo.x >= 1.0)
	{
		float4 animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimTimeInfo);

		float3 uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 1);
		float4 normalData = UNITY_SAMPLE_TEX2DARRAY_LOD(_AnimTextures, uvPos, 0);
		normalData = DecodeNegativeVectors(normalData);
		if (normalData.x != 0.0 || normalData.y != 0.0 || normalData.z != 0.0)
		{
			normal = normalData.xyz;

			const float4 crossfadeData = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeData);		
			const float crossfadeEndTime = crossfadeData.x + crossfadeData.z;	
			if (crossfadeEndTime > _Time.y)
			{
				animInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimInfo);
				animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimTimeInfo);
				uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 1);
				float4 crossfadeNormalData = UNITY_SAMPLE_TEX2DARRAY_LOD(_AnimTextures, uvPos, 0);
				crossfadeNormalData = DecodeNegativeVectors(crossfadeNormalData);
				
				if (crossfadeNormalData.x != 0.0 || crossfadeNormalData.y != 0.0 || crossfadeNormalData.z != 0.0)
				{
					const float blendWeight = (crossfadeEndTime - _Time.y) / crossfadeData.z;
					normal = lerp(crossfadeNormalData, normal, 1.0 - blendWeight);
				}
			}
		}
	}
	return normal;
}