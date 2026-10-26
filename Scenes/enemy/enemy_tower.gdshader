shader_type canvas_item;

uniform bool active; // 外部控制是否闪烁
uniform vec3 flash_color : source_color = vec3(1.0); //闪烁的颜色


void fragment(){
    vec4 t = texture(TEXTURE, UV);
    if (active == true) {
        COLOR = vec4(flash_color, t.a);
    }
}