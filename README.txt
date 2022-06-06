TEAM MEMBERS:

Xavier Flores
Joel Mateu

Exercises Done:

Post Processing:
	Vignetting/Blur -> Vignetting was implemented with the blur combination, this effect can be found when the rainstorm appears, the vignetting effect is used to simulate the rain better. Can be found in Assets\Shaders\VignetteBlurRenderTexture
	Bloom with Blur -> that effect can be found while there's no any storm on the island, the bloom gives to the scene a sunny day effect meanwhile the blur effect gives a sunny day effect in the far view. Can be found in Assets\Shaders\BlurForRenderTexture

Shader in Unity:
	Adding features to our PBR shader -> BRDF Phong shader implemented with shadow pass (the shadow pass could be seen at the scene with darker sides). Could be found in Assets\Shaders\Phong
	Create materials for the whole scene using your material -> Textures using our Phong shader implemented, all the implementation could be found in the Assets\Delivery2\Delivery2 Scene

Compute Shaders:
	Boids Implementation -> Boids implemented as a seagull group the are flying through the island, they are able to dodge obstacles and follow a random target in the scene that gets a random position in a range. Can be found in Assets\Scripts\BoidAgentScript and Assets\Scripts\BoidComputeShader 

Additional Implementations:
	Vertex shader animation -> Vertex Shader animation inplemented in the waves and the ship sail as a sin wave, it can be found in Assets\Shaders\VertexAnimWithPhong and Assets\Shaders\VertexAnimWithPhong Transparent
	Texture animation -> Texture animation implemented to give a water moving effect, it is obtained by incrementing a vertex axis by a sin timer, it can be found in Assets\Shaders\VertexAnimWithPhong Transparent