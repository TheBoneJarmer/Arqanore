import bpy
import bpy_extras
import math
import mathutils

from bpy_extras.io_utils import ExportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator

bl_info = {
    "name": "Arqanore Model Exporter",
    "description": "Exports your model to an arqanore model file",
    "author": "Ruben Labruyere",
    "blender": (2, 80, 0),
    "version": (0, 1, 0),
    "category": "Import-Export"
}

major = bl_info["version"][0]
minor = bl_info["version"][1]
patch = bl_info["version"][2]

class Face:
    vi = []
    li = []


class Color:
    r = 0
    g = 0
    b = 0
    a = 0
    
    def __init__(self):
        self.r = 0
        self.g = 0
        self.b = 0
        self.a = 255
        
    def __init__(self,r,g,b,a):
        self.r = r
        self.g = g
        self.b = b
        self.a = a
        
    def __init__(self,f):
        self.r = f
        self.g = f
        self.b = f
        self.a = f


class ArqanoreModelParser:

    def find_object(self, name):
        for obj in bpy.context.scene.objects:
            if obj.name == name:
                return obj

        return None

    def generate(self):
        scene = bpy.context.scene
        objects = scene.objects
        str_arqm = ""
        
        # Add metadata to begin of file
        str_arqm += f"VERSION {major}.{minor}.{patch}\n\n"
        
        # Generate rotation matrix to transform vertices
        m = mathutils.Matrix.Rotation(math.radians(90.0), 4, 'X')
        mi = m.inverted()  

        for mat in bpy.data.materials:
            color = Color(255)
            diffuse = Color(204)
            specular = Color(125)
            ambient = Color(204)
            shininess = 16.0
            diffuse_map = None
            ambient_map = None
            specular_map = None
            
            color.r = round(mat.diffuse_color[0] * 255)
            color.g = round(mat.diffuse_color[1] * 255)
            color.b = round(mat.diffuse_color[2] * 255)
            color.a = round(mat.diffuse_color[3] * 255)
            
            specular.r = round(mat.specular_color[0] * 255)
            specular.g = round(mat.specular_color[1] * 255)
            specular.b = round(mat.specular_color[2] * 255)

            if mat.node_tree:
                for node in mat.node_tree.nodes:               
                    if node.type == "TEX_IMAGE" and node.image:
                        diffuse_map = node.image.filepath[2:]

                    if node.type == "BSDF_DIFFUSE":
                        for input in node.inputs:
                            if input.type == "RGBA":
                                values = input.default_value
                                color.r = round(values[0] * 255)
                                color.g = round(values[1] * 255)
                                color.b = round(values[2] * 255)
                                color.a = round(values[3] * 255)

                            if input.type == "RGB":
                                values = input.default_value
                                color.r = round(values[0] * 255)
                                color.g = round(values[1] * 255)
                                color.b = round(values[2] * 255)
                                color.a = 255

                    if node.type == "RGB":
                        values = node.outputs[0].default_value
                        color.r = round(values[0] * 255)
                        color.g = round(values[1] * 255)
                        color.b = round(values[2] * 255)
                        color.a = round(values[3] * 255)

            str_arqm += f"BEGIN_MAT {mat.name}\n"
            str_arqm += f"clr {color.r} {color.g} {color.b} {color.a}\n"
            str_arqm += f"dif {diffuse.r} {diffuse.g} {diffuse.b} {diffuse.a}\n"
            str_arqm += f"spc {specular.r} {specular.g} {specular.b} {specular.a}\n"
            str_arqm += f"amb {ambient.r} {ambient.g} {ambient.b} {ambient.a}\n"
            str_arqm += f"shn {shininess}\n"

            if diffuse_map is not None:
                str_arqm += f"dif_map {diffuse_map}\n"

            if ambient_map is not None:
                str_arqm += f"amb_map {ambient_map}\n"

            if specular_map is not None:
                str_arqm += f"spc_map {specular_map}\n"

            str_arqm += "END_MAT\n"

        for obj in objects:
            if obj.type == "ARMATURE":
                bones = obj.pose.bones
                                            
                str_arqm += f"\nBEGIN_ARMATURE {obj.name}\n"
                
                for bone in bones:
                    str_arqm += f"BEGIN_BONE {bone.name}\n"
                    
                    if bone.parent != None:
                        str_arqm += f"p {bone.parent.name}\n"
                    
                    for i in range(scene.frame_start - 1, scene.frame_end + 1):
                        scene.frame_set(i)
                        
                        loc = bone.location
                        scl = bone.scale
                        
                        rot_current = bone.rotation_euler
                        rot_new = mathutils.Euler()
                        rot_new.x = rot_current.x
                        rot_new.y = rot_current.z
                        rot_new.z = -rot_current.y
                        
                        rot = mathutils.Quaternion()
                        rot.rotate(rot_new)
                                                                
                        str_arqm += f"bf"
                        str_arqm += f" {round(loc.x, 2)} {round(loc.z, 2)} {round(loc.y, 2)}"
                        str_arqm += f" {round(rot.x, 2)} {round(rot.y, 2)} {round(rot.z, 2)} {round(rot.w, 2)}"
                        str_arqm += f" {round(scl.x, 2)} {round(scl.z, 2)} {round(scl.y, 2)}"
                        str_arqm += f"\n"

                    scene.frame_set(0)
                    
                    str_arqm += "END_BONE\n"
                                    
                str_arqm += "END_ARMATURE"

            if obj.type == "MESH":
                faces = []
                mesh = obj.data
                uv_layer = mesh.uv_layers.active.data
                vertex_groups = obj.vertex_groups
                
                loc = obj.location
                scl = obj.scale
                
                rot_current = obj.rotation_euler
                rot_new = mathutils.Euler()
                rot_new.x = rot_current.x
                rot_new.y = rot_current.z
                rot_new.z = -rot_current.y
                
                rot = mathutils.Quaternion()
                rot.rotate(rot_new)
                
                for poly in mesh.polygons:
                    face = Face()
                    face.li = poly.loop_indices
                    face.vi = poly.vertices

                    faces.append(face)
                    
                str_arqm += f"\nBEGIN_MESH {obj.name}\n"
                str_arqm += f"loc {round(loc.x, 2)} {round(loc.z, 2)} {round(loc.y, 2)}\n"
                str_arqm += f"rot {round(rot.x, 2)} {round(rot.y, 2)} {round(rot.z, 2)} {round(rot.w, 2)}\n"
                str_arqm += f"scl {round(scl.x, 2)} {round(scl.z, 2)} {round(scl.y, 2)}\n"
                                
                for vg in vertex_groups:
                    str_arqm += f"g {vg.index} {vg.name}\n"

                if obj.active_material:
                    str_arqm += f"mat {obj.active_material.name}\n"

                for v in mesh.vertices:
                    v_co = v.co @ m
                    v_nor = v.normal @ m
                    
                    str_arqm += f"v {round(v_co[0], 2)} {round(v_co[1], 2)} {round(v_co[2], 2)}\n"
                    str_arqm += f"n {round(v_nor[0], 2)} {round(v_nor[1], 2)} {round(v_nor[2], 2)}\n"
                    
                    if len(v.groups) > 0:
                        str_arqm += "vg"
                        
                        for vge in v.groups:                            
                            str_arqm += f" {vge.group}"
                            
                        str_arqm += "\n"

                for layer in uv_layer:
                    str_arqm += f"tc {round(layer.uv[0], 2)} {round(layer.uv[1], 2)}\n"

                for face in faces:
                    str_arqm += "f"

                    for i in range(3):
                        vi = face.vi[i]
                        li = face.li[i]

                        str_arqm += f" {vi}/{li}"

                    str_arqm += "\n"

                str_arqm += "END_MESH\n"
        
        return str_arqm


class ArqanoreModelExporter(Operator, ExportHelper):
    bl_idname = "arqanore.export"
    bl_label = "Export Arqanore Model"
    filename_ext = ".arqm"

    def execute(self, context):
        parser = ArqanoreModelParser()

        file = open(self.filepath, "w")
        file.write(parser.generate())
        file.close()

        return {'FINISHED'}


def menu_func_export(self, context):
    self.layout.operator(ArqanoreModelExporter.bl_idname, text="Arqanore Model (.arqm)")


def register():
    bpy.utils.register_class(ArqanoreModelExporter)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)


def unregister():
    bpy.utils.unregister_class(ArqanoreModelExporter)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
    register()

    # for testing
    #bpy.ops.arqanore.export('INVOKE_DEFAULT')
