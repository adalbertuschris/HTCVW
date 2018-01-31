#version 330
 
in vec3 position;
in vec3 normal;

out vec3 Normal;
out vec3 FragPos;

uniform mat4 modelViewProj;

void main()
{
    gl_Position = modelViewProj * vec4(position, 1.0f);
    FragPos = vec3(modelViewProj * vec4(position, 1.0f));
    Normal = mat3(transpose(inverse(modelViewProj))) * normal;  
} 
