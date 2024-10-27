shader_type canvas_item;

uniform vec4 pulse_color = vec4(0.18, 0.18, 0.18, 1.0); // Pulsing color (blue)
uniform float pulse_entr = 0;
uniform float pulse_speed = 2.0; // Speed of the pulsing effect

void fragment() {
    // Calculate the pulse factor using a sine wave, oscillating between 0 and 1
    float pulse = (sin(TIME * pulse_speed) + 1.0) * 0.5;

    // Sample the texture and extract the base color (RGB)
    vec4 tex_color = texture(TEXTURE, UV);

    // Mix only the RGB values between the texture color and the pulse color, keeping the alpha from the texture
    vec3 final_rgb = mix(tex_color.rgb * vec3(0.2, 0.2, 0.2) * (vec3(1,0,0)*(pulse_entr/100.0)), pulse_color.rgb, pulse);

    // Output the final color, keeping the texture's alpha unchanged
    COLOR = vec4(final_rgb, tex_color.a);
}
