#version 330 core
out vec4 FragColor;
uniform vec3 color;

in vec3 worldSpacePos;
in vec3 normal;
void main()
{
    vec3 light_pos = vec3(3,3,3);
    float light_intensity = 5.0f;
    vec3 ambient = vec3(.1,.1,.1)*color;

    vec3 wi = normalize(light_pos - worldSpacePos);
    vec3 light_color = color * max(0.0f, dot(wi, normal));
    //light_color += ambient;
    FragColor = vec4(light_color, 1.0);
}