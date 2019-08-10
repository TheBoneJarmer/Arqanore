using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Seanuts.OpenGL
{
    public unsafe static partial class GLU
    {
        #region Constants

        public const int GLU_VERSION_1_1 = 1;
        public const int GLU_VERSION_1_2 = 1;
        public const int GLU_INVALID_ENUM = 100900;
        public const int GLU_INVALID_VALUE = 100901;
        public const int GLU_OUT_OF_MEMORY = 100902;
        public const int GLU_INCOMPATIBLE_GL_VERSION = 100903;
        public const int GLU_VERSION = 100800;
        public const int GLU_EXTENSIONS = 100801;
        public const int GLU_TRUE = 1;
        public const int GLU_FALSE = 0;
        public const int GLU_SMOOTH = 100000;
        public const int GLU_FLAT = 100001;
        public const int GLU_NONE = 100002;
        public const int GLU_POINT = 100010;
        public const int GLU_LINE = 100011;
        public const int GLU_FILL = 100012;
        public const int GLU_SILHOUETTE = 100013;
        public const int GLU_OUTSIDE = 100020;
        public const int GLU_INSIDE = 100021;
        //public const float GLU_TESS_MAX_COORD = 1.0e150;
        public const int GLU_TESS_WINDING_RULE = 100140;
        public const int GLU_TESS_BOUNDARY_ONLY = 100141;
        public const int GLU_TESS_TOLERANCE = 100142;
        public const int GLU_TESS_WINDING_ODD = 100130;
        public const int GLU_TESS_WINDING_NONZERO = 100131;
        public const int GLU_TESS_WINDING_POSITIVE = 100132;
        public const int GLU_TESS_WINDING_NEGATIVE = 100133;
        public const int GLU_TESS_WINDING_ABS_GEQ_TWO = 100134;
        public const int GLU_TESS_BEGIN = 100100;
        public const int GLU_TESS_VERTEX = 100101;
        public const int GLU_TESS_END = 100102;
        public const int GLU_TESS_ERROR = 100103;
        public const int GLU_TESS_EDGE_FLAG = 100104;
        public const int GLU_TESS_COMBINE = 100105;
        public const int GLU_TESS_BEGIN_DATA = 100106;
        public const int GLU_TESS_VERTEX_DATA = 100107;
        public const int GLU_TESS_END_DATA = 100108;
        public const int GLU_TESS_ERROR_DATA = 100109;
        public const int GLU_TESS_EDGE_FLAG_DATA = 100110;
        public const int GLU_TESS_COMBINE_DATA = 100111;
        public const int GLU_TESS_ERROR1 = 100151;
        public const int GLU_TESS_ERROR2 = 100152;
        public const int GLU_TESS_ERROR3 = 100153;
        public const int GLU_TESS_ERROR4 = 100154;
        public const int GLU_TESS_ERROR5 = 100155;
        public const int GLU_TESS_ERROR6 = 100156;
        public const int GLU_TESS_ERROR7 = 100157;
        public const int GLU_TESS_ERROR8 = 100158;
        public const int GLU_TESS_MISSING_BEGIN_POLYGON = 100151;
        public const int GLU_TESS_MISSING_BEGIN_CONTOUR = 100152;
        public const int GLU_TESS_MISSING_END_POLYGON = 100153;
        public const int GLU_TESS_MISSING_END_CONTOUR = 100154;
        public const int GLU_TESS_COORD_TOO_LARGE = 100155;
        public const int GLU_TESS_NEED_COMBINE_CALLBACK = 100156;
        public const int GLU_AUTO_LOAD_MATRIX = 100200;
        public const int GLU_CULLING = 100201;
        public const int GLU_SAMPLING_TOLERANCE = 100203;
        public const int GLU_DISPLAY_MODE = 100204;
        public const int GLU_PARAMETRIC_TOLERANCE = 100202;
        public const int GLU_SAMPLING_METHOD = 100205;
        public const int GLU_U_STEP = 100206;
        public const int GLU_V_STEP = 100207;
        public const int GLU_PATH_LENGTH = 100215;
        public const int GLU_PARAMETRIC_ERROR = 100216;
        public const int GLU_DOMAIN_DISTANCE = 100217;
        public const int GLU_MAP1_TRIM_2 = 100210;
        public const int GLU_MAP1_TRIM_3 = 100211;
        public const int GLU_OUTLINE_POLYGON = 100240;
        public const int GLU_OUTLINE_PATCH = 100241;
        public const int GLU_NURBS_ERROR1 = 100251;
        public const int GLU_NURBS_ERROR2 = 100252;
        public const int GLU_NURBS_ERROR3 = 100253;
        public const int GLU_NURBS_ERROR4 = 100254;
        public const int GLU_NURBS_ERROR5 = 100255;
        public const int GLU_NURBS_ERROR6 = 100256;
        public const int GLU_NURBS_ERROR7 = 100257;
        public const int GLU_NURBS_ERROR8 = 100258;
        public const int GLU_NURBS_ERROR9 = 100259;
        public const int GLU_NURBS_ERROR10 = 100260;
        public const int GLU_NURBS_ERROR11 = 100261;
        public const int GLU_NURBS_ERROR12 = 100262;
        public const int GLU_NURBS_ERROR13 = 100263;
        public const int GLU_NURBS_ERROR14 = 100264;
        public const int GLU_NURBS_ERROR15 = 100265;
        public const int GLU_NURBS_ERROR16 = 100266;
        public const int GLU_NURBS_ERROR17 = 100267;
        public const int GLU_NURBS_ERROR18 = 100268;
        public const int GLU_NURBS_ERROR19 = 100269;
        public const int GLU_NURBS_ERROR20 = 100270;
        public const int GLU_NURBS_ERROR21 = 100271;
        public const int GLU_NURBS_ERROR22 = 100272;
        public const int GLU_NURBS_ERROR23 = 100273;
        public const int GLU_NURBS_ERROR24 = 100274;
        public const int GLU_NURBS_ERROR25 = 100275;
        public const int GLU_NURBS_ERROR26 = 100276;
        public const int GLU_NURBS_ERROR27 = 100277;
        public const int GLU_NURBS_ERROR28 = 100278;
        public const int GLU_NURBS_ERROR29 = 100279;
        public const int GLU_NURBS_ERROR30 = 100280;
        public const int GLU_NURBS_ERROR31 = 100281;
        public const int GLU_NURBS_ERROR32 = 100282;
        public const int GLU_NURBS_ERROR33 = 100283;
        public const int GLU_NURBS_ERROR34 = 100284;
        public const int GLU_NURBS_ERROR35 = 100285;
        public const int GLU_NURBS_ERROR36 = 100286;
        public const int GLU_NURBS_ERROR37 = 100287;

        #endregion

        #region Functions

        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern sbyte* gluErrorString(int errCode);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern sbyte* gluglGetString(int name);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluOrtho2D(float left, float right, float bottom, float top);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluPerspective(float fovy, float aspect, float zNear, float zFar);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluPickMatrix(float x, float y, float width, float height, int[] viewport);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluLookAt(float eyex, float eyey, float eyez, float centerx, float centery, float centerz, float upx, float upy, float upz);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluProject(float objx, float objy, float objz, float[] modelMatrix, float[] projMatrix, int[] viewport, float winx, float winy, float winz);

        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluUnProject(float winx, float winy, float winz, float[] modelMatrix, float[] projMatrix, int[] viewport, float objx, float objy, float objz);

        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluScaleImage(int format, int widthin, int heightin, int typein, int[] datain, int widthout, int heightout, int typeout, int[] dataout);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluBuild1DMipmaps(int target, int components, int width, int format, int type, IntPtr data);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluBuild2DMipmaps(int target, int components, int width, int height, int format, int type, IntPtr data);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gluNewQuadric();
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluDeleteQuadric(IntPtr state);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluQuadricNormals(IntPtr quadObject, int normals);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluQuadricTexture(IntPtr quadObject, int textureCoords);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluQuadricOrientation(IntPtr quadObject, int orientation);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluQuadricDrawStyle(IntPtr quadObject, int drawStyle);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluCylinder(IntPtr qobj, float baseRadius, float topRadius, float height, int slices, int stacks);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluDisk(IntPtr qobj, float innerRadius, float outerRadius, int slices, int loops);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluPartialDisk(IntPtr qobj, float innerRadius, float outerRadius, int slices, int loops, float startAngle, float sweepAngle);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluSphere(IntPtr qobj, float radius, int slices, int stacks);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gluNewTess();
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluDeleteTess(IntPtr tess);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessBeginPolygon(IntPtr tess, IntPtr polygonData);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessBeginContour(IntPtr tess);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessVertex(IntPtr tess, float[] coords, float[] data);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessEndContour(IntPtr tess);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessEndPolygon(IntPtr tess);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessProperty(IntPtr tess, int which, float value);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluTessNormal(IntPtr tess, float x, float y, float z);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Begin callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.BeginData callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Combine callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.CombineData callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlag callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlagData callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.End callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EndData callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Error callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.ErrorData callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Vertex callback);
        //[DllImport(LIBRARY_GLU, CallingConvention = CallingConvention.Cdecl)] public static extern void  gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.VertexData callback);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluGetTessProperty(IntPtr tess, int which, float value);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gluNewNurbsRenderer();
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluDeleteNurbsRenderer(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluBeginSurface(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluBeginCurve(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluEndCurve(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluEndSurface(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluBeginTrim(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluEndTrim(IntPtr nobj);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluPwlCurve(IntPtr nobj, int count, float array, int stride, int type);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluNurbsCurve(IntPtr nobj, int nknots, float[] knot, int stride, float[] ctlarray, int order, int type);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluNurbsSurface(IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[] tknot, int s_stride, int t_stride, float[] ctlarray, int sorder, int torder, int type);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluLoadSamplingMatrices(IntPtr nobj, float[] modelMatrix, float[] projMatrix, int[] viewport);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluNurbsProperty(IntPtr nobj, int property, float value);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gluGetNurbsProperty(IntPtr nobj, int property, float value);
        [DllImport("Glu32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IntPtrCallback(IntPtr nobj, int which, IntPtr Callback);

        #endregion
    }
}
