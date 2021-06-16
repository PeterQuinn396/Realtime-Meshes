#version 330 core
out vec4 FragColor;
uniform vec3 color;
uniform vec3 camPos;
#define PI 3.14159265

in vec3 worldSpacePos;
in vec3 normal;
void main()
{
    
    vec3 light_pos = vec3(3,3,3);
    float light_intensity = 5.0f;
    vec3 ambient = .15 * color;

    vec3 wi = normalize(light_pos - worldSpacePos);
    vec3 wo = normalize(camPos - worldSpacePos);
    vec3 diffuse = color * max(0.0f, dot(wi, normal));

    int p = 10;
    vec3 wi_r = 2 * dot(wi, normal) * normal - wi;
    float cos_a = dot(wi_r, wo);
    vec3 spec = vec3(1,1,1) * max(0, pow(cos_a, p)) * max(0.0f, dot(wi, normal));
    vec3 final_color = diffuse  + spec/(2*PI) * (p + 2) + ambient;
    FragColor = vec4(final_color, 1.0);
    //FragColor = vec4(ambient, 1.0);
}