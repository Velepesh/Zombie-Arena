//UNITY_SHADER_NO_UPGRADE

float GetPixelOffset(inout float textureIndex, float4 animInfo, float4 animTimeInfo)
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

float3 GetUVPos(uint vertexIndex, float textureIndex, float pixelOffset, float textureSize, uint offset)
{
	uint vertexOffset = pixelOffset + (vertexIndex * 2);
	vertexOffset += offset;
	float offsetX = floor(vertexOffset / textureSize);
	float offsetY = vertexOffset - (offsetX * textureSize);
	float3 uvPos = float3(offsetX / textureSize, offsetY / textureSize, textureIndex);
	return uvPos;
}

float3 GetAnimationUVPosition(uint vertexIndex, float4 animInfo, float4 animTimeInfo, uint offset)
{
	float textureIndex = animInfo.x - 1.0;
	const float pixelOffset = GetPixelOffset(textureIndex, animInfo, animTimeInfo);
	return GetUVPos(vertexIndex, textureIndex, pixelOffset, animInfo.z, offset);
}

float3 DecodeNegativeVectors(float3 positionData)
{
	positionData.x = (positionData.x - 0.5) * 2;
	positionData.y = (positionData.y - 0.5) * 2;
	positionData.z = (positionData.z - 0.5) * 2;
	return positionData;
}

float3 ApplyAnimationScalar(float3 positionData, float4 animScalar)
{
	positionData = DecodeNegativeVectors(positionData);
	positionData *= animScalar.xyz;
	return positionData;
}

float3 ApplyMeshAnimation(
	float3 position,
	float vertexId,
	float4 animInfo,
	float4 animTimeInfo,
	float4 animScalar,
	float4 crossfadeAnimInfo,
	float4 crossfadeTimeInfo,
	float4 crossfadeData,
	float4 crossfadeAnimScalar,
	Texture2DArray animTextures,
	SamplerState samplerState)
{
	if (animInfo.x >= 1.0)
	{
		float3 uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 0);
		float3 positionData = SAMPLE_TEXTURE2D_ARRAY_LOD(animTextures, samplerState, uvPos.xy, uvPos.z, 0).xyz;
		positionData = ApplyAnimationScalar(positionData, animScalar);
			
		if (crossfadeData.y > _Time.y)
		{		
			uvPos = GetAnimationUVPosition(vertexId, crossfadeAnimInfo, crossfadeTimeInfo, 0);
			float3 crossfadePositionData = SAMPLE_TEXTURE2D_ARRAY_LOD(animTextures, samplerState, uvPos.xy, uvPos.z, 0).xyz;
			crossfadePositionData = ApplyAnimationScalar(crossfadePositionData, crossfadeAnimScalar);

			const float blendWeight = saturate((_Time.y - crossfadeData.x) / crossfadeData.z);
			positionData = lerp(crossfadePositionData, positionData, blendWeight);
		}
		position = positionData;
	}
	return position;
}

float3 GetAnimatedMeshNormal(
	float3 normal,
	uint vertexId,
	float4 animInfo,
	float4 animTimeInfo,
	float4 crossfadeAnimInfo,
	float4 crossfadeAnimTimeInfo,
	float4 crossfadeData,
	Texture2DArray animTextures,
	SamplerState samplerState)
{
	if (animInfo.x >= 1.0)
	{
		float3 uvPos = GetAnimationUVPosition(vertexId, animInfo, animTimeInfo, 1);
		float3 normalData = SAMPLE_TEXTURE2D_ARRAY_LOD(animTextures, samplerState, uvPos.xy, uvPos.z, 0).xyz;
		normalData = DecodeNegativeVectors(normalData);
		if (normalData.x != 0.0 || normalData.y != 0.0 || normalData.z != 0.0)
		{
			normal = normalData;
			if (crossfadeData.y > _Time.y)
			{		
				uvPos = GetAnimationUVPosition(vertexId, crossfadeAnimInfo, crossfadeAnimTimeInfo, 1);
				float3 crossfadeNormalData = SAMPLE_TEXTURE2D_ARRAY_LOD(animTextures, samplerState, uvPos.xy, uvPos.z, 0).xyz;
				crossfadeNormalData = DecodeNegativeVectors(crossfadeNormalData);
				if (crossfadeNormalData.x != 0.0 || crossfadeNormalData.y != 0.0 || crossfadeNormalData.z != 0.0)
				{
					const float blendWeight = saturate((_Time.y - crossfadeData.x) / crossfadeData.z);
					normal = lerp(crossfadeNormalData, normal, blendWeight);
				}
			}
		}
	}
	return normal;
}

void ApplyMeshAnimationValues_float(
	float3 vertexPosition,
	float3 vertexNormal,
	float4 animTimeInfo, 
	Texture2DArray animTextures,
	float4 animInfo, 
	float4 animScalar, 
	float4 crossfadeAnimInfo, 
	float4 crossfadeAnimTimeInfo, 
	float4 crossfadeAnimScalar, 
	float4 crossfadeData,
	float vertexIndex,
	SamplerState samplerState,
	out float3 outputVertexPosition,
	out float3 outputVertexNormal)
{
	outputVertexPosition = ApplyMeshAnimation(
		vertexPosition,
		vertexIndex,
		animInfo,
		animTimeInfo,
		animScalar,
		crossfadeAnimInfo,
		crossfadeAnimTimeInfo,
		crossfadeData,
		crossfadeAnimScalar,
		animTextures,
		samplerState);

	outputVertexNormal = GetAnimatedMeshNormal(
		vertexNormal,
		vertexIndex,
		animInfo,
		animTimeInfo,
		crossfadeAnimInfo,
		crossfadeAnimTimeInfo,
		crossfadeData,
		animTextures,
		samplerState);
}