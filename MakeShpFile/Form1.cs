using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.Office.Interop.Excel;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using System.Data.OleDb;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.Diagnostics;
using System.IO;

namespace MakeShpFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
        }
        int DataMode = -1; // Point Polyline Polygon
        int VehicleMode = -1;//car publicTransp riding walking
        public int NumOfHead = 0;
        public int NumOfRecord = 0;
        public ProgressBar ProgressFm;
        private void Form1_Load(object sender, EventArgs e)
        {
            ProgressFm = new ProgressBar(2, 100);
            ReadExl.Enabled = false;
            CarModeRBtn.Checked = true;
            PolylineRBtn.Checked = true;
        }
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型  
        /// <param name="FILE_NAME">文件路径</param>  
        /// <returns>文件的编码类型</returns>  
        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }
        /// 通过给定的文件流，判断
        /// 文件的编码类型  
        /// <param name="fs">文件流</param>  
        /// <returns>文件的编码类型</returns>  
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM  
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }
        /// 判断是否是不带 BOM 的 UTF8 格式  
        /// <param name="data"></param>  
        /// <returns></returns>  
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数  
            byte curByte; //当前分析的字节.  
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前  
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　  
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1  
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
        public System.Data.DataTable OpenCsv(string filePath)
        {
            System.Text.Encoding encoding = GetType(filePath); //Encoding.ASCII;//  
            System.Data.DataTable dt = new System.Data.DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,System.IO.FileAccess.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);
            //记录每次读取的一行记录  
            string strLine = "";
            //记录每行记录中的各字段内容  
            
            //标示列数  
            int columnCount = 0;
            //逐行读取CSV中的数据  
            for (int k = 0; k < 5; k++)
            {
                DataColumn dc = new DataColumn();
                dt.Columns.Add(dc);
            }
            int maxColumn = 5;
                while ((strLine = sr.ReadLine()) != null)
                {
                    int index = strLine.IndexOf('"');
                    string[] aryLineFirst = strLine.Substring(0, index - 1).Split(',');
                    string[] aryLine = null;
                    aryLine = strLine.Substring(index + 1, strLine.Length - index - 2).Split(';');
                    DataRow dr = dt.NewRow();
                    int i = 0;
                    columnCount = aryLine.Length;
                    if ((columnCount+5) > maxColumn)
                    {
                        int preColumn = maxColumn - 1;
                        maxColumn = columnCount + 5;                       
                        for (; preColumn < maxColumn; preColumn++)
                        {
                            DataColumn dc = new DataColumn();
                            dt.Columns.Add(dc);
                        }
                    }
                    for (i = 0; i < aryLineFirst.Length; i++)
                    {
                        dr[i] = aryLineFirst[i];
                    }
                    for (int j = i; j < i + columnCount; j++)
                    {
                        dr[j] = aryLine[j - i];
                    }
                    dt.Rows.Add(dr);
                }
            sr.Close();
            fs.Close();
            return dt;
        }

        public bool IsFeatureClassExistZ(IFeatureClass fc)
        {
            IGeoDataset geoDT = fc as IGeoDataset;
            IZAware zAware = geoDT.Extent as IZAware;
            return zAware.ZAware;
        }
        bool bisWrite = true;
        System.IO.FileStream FS;
        System.IO.StreamWriter SW;

        private void PublicTrans_Polyline_Routes(IFeatureWorkspace pFWS, string shpName, string InputFilePath)
        {
            //开始添加属性字段；
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            //添加字段“OID”；
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            //设置生成图的空间坐标参考系统；
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //投影坐标系
            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
            spatialReferenceResolution2.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
            spatialReferenceTolerance2.SetDefaultXYTolerance();
            geometryDefEdit.SpatialReference_2 = spatialReference2;

            //添加字段“Shape”;
            IField geometryField = new FieldClass();
            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(geometryField);
            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            //添加字段序号ID
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "序号ID";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);


            //添加字段“origin”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起点位置";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终点位置”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终点位置";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段Route_time
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Route_time(s)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Route_distance
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Route_distance(km)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Price_Yuan
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Price(Yuan)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Traffic_status
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Traffic_status";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段“点对数”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "点对数";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            //在工作空间中生成FeatureClass;
            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


            System.IO.FileStream FS_Routescsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            System.IO.StreamReader SR_Routescsv = new System.IO.StreamReader(FS_Routescsv, utf8WithoutBom, true);

            string SubPathFileName = InputFilePath.Substring(0, InputFilePath.Length - 10) + "subpaths.csv";

            System.IO.FileStream FS_Subpathscsv = new System.IO.FileStream(SubPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader SR_Subpathscsv = new System.IO.StreamReader(FS_Subpathscsv, utf8WithoutBom, true);

            string Routes_Line = "";
            string Subpath_Line = "";
            IFeatureBuffer pFeatureBuffer = null;
            IFeatureCursor pFeatureCursor = null;
            string SumPath = "";
            string PrePath = "";
            int num = 0;
            while ((Routes_Line = SR_Routescsv.ReadLine()) != null)
            {
                NumOfRecord++;
            }
            SR_Routescsv.BaseStream.Seek(0, SeekOrigin.Begin);

            while ((Routes_Line = SR_Routescsv.ReadLine()) != null)
            {
                pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
                pFeatureCursor = pNewFeaCls.Insert(true);
                //int index = Routes_Line.IndexOf('"');//第一个双引号在字符串中的位置
                //string aryLineFirst = Routes_Line.Substring(0, index - 1);
                string[] aryLineFirstArray = Routes_Line.Split(',');
                pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
                pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);
                pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
                pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
                pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
                pFeatureBuffer.set_Value(7, aryLineFirstArray[5]);
                pFeatureBuffer.set_Value(8, aryLineFirstArray[6]);
                pFeatureBuffer.set_Value(9, aryLineFirstArray[7]);
                pFeatureBuffer.set_Value(10, aryLineFirstArray[8]);
                pFeatureBuffer.set_Value(11, aryLineFirstArray[9]);
                pFeatureBuffer.set_Value(12, "0");//拥堵状况设为0 

                SumPath = "";

                SumPath += PrePath;
                int flag = 1;
                while ((Subpath_Line = SR_Subpathscsv.ReadLine()) != null)
                {
                    int indexTemp = Subpath_Line.IndexOf(',');
                    int indexYin1 = Subpath_Line.IndexOf('"');
                    int indexYin2 = Subpath_Line.LastIndexOf('"');
                    string path = Subpath_Line.Substring(indexYin1 + 1, indexYin2 - indexYin1 - 1);
                    if (flag != 1)
                    {
                        path = path.Substring(path.IndexOf(';') + 1, path.Length - path.IndexOf(';') - 1);
                    }
                    string id = Subpath_Line.Substring(0, indexTemp);
                    if (id != aryLineFirstArray[0])
                    {
                        PrePath = path;
                        PrePath += ";";
                        break;
                    }
                    SumPath += path;
                    SumPath += ";";
                    flag++;
                }
                SumPath = SumPath.Substring(0, SumPath.Length - 1);
                string temp = SumPath;
                int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
                pFeatureBuffer.set_Value(13, SumOfpoint.ToString());


                if (SumOfpoint > 11000)
                {
                    int time = SumOfpoint / 11000 + 1;
                    string[] subAryLine = new string[time];
                    string tempStr = SumPath;
                    int indexPre = 0;
                    int indexSub = 0;
                    int t = 0;
                    while (tempStr.Length > 0)
                    {
                        int indexT = 0;
                        for (int i = 0; i < 11000; i++)
                        {
                            if ((t + 1) == time)
                            {
                                indexSub = tempStr.Length;
                                subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                                t++;
                                tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                                break;
                            }
                            indexT = tempStr.IndexOf(';', indexT + 1);
                            if (indexT == -1)
                            {
                                break;
                            }
                            else
                            {
                                indexSub = indexT;
                            }
                        }
                        if (t == time)
                            continue;
                        indexSub++;
                        subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                        t++;
                        tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                    }
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
                }
                else
                {
                    string[] strArray = new string[1];
                    strArray[0] = SumPath;
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
                }
                ProgressFm.setPos((int)((++num) / (double)(NumOfHead * NumOfRecord * 2) * 100));//设置进度条位置
            }
            SR_Subpathscsv.Close();
            FS_Subpathscsv.Close();
            SR_Routescsv.Close();
            FS_Routescsv.Close();
        }
        private void Car_Polyline_Routes(IFeatureWorkspace pFWS, string shpName, string InputFilePath)
        {
            //开始添加属性字段；
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            //添加字段“OID”；
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            //设置生成图的空间坐标参考系统；
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //投影坐标系
            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
            spatialReferenceResolution2.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
            spatialReferenceTolerance2.SetDefaultXYTolerance();
            geometryDefEdit.SpatialReference_2 = spatialReference2;

            //添加字段“Shape”;
            IField geometryField = new FieldClass();
            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(geometryField);
            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            //添加字段序号ID
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "序号ID";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);


            //添加字段“origin”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起点位置";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终点位置”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终点位置";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起点区域”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起点区域";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终点区域";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段Route_time
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Route_time(s)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Route_distance
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Route_distance(km)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Traffic_status
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Traffic_status";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段“点对数”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "点对数";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            //在工作空间中生成FeatureClass;
            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


            System.IO.FileStream FS_Routescsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            System.IO.StreamReader SR_Routescsv = new System.IO.StreamReader(FS_Routescsv, utf8WithoutBom, true);

            string SubPathFileName = InputFilePath.Substring(0, InputFilePath.Length - 10) + "subpaths.csv";

            System.IO.FileStream FS_Subpathscsv = new System.IO.FileStream(SubPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader SR_Subpathscsv = new System.IO.StreamReader(FS_Subpathscsv, utf8WithoutBom, true);

            string Routes_Line = "";
            string Subpath_Line = "";
            IFeatureBuffer pFeatureBuffer = null;
            IFeatureCursor pFeatureCursor = null;
            string SumPath = "";
            string PrePath = "";
            int num = 0;
            while ((Routes_Line = SR_Routescsv.ReadLine()) != null)
            {
                NumOfRecord++;
            }
            SR_Routescsv.BaseStream.Seek(0, SeekOrigin.Begin);

            while ((Routes_Line = SR_Routescsv.ReadLine()) != null)
            {
                pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
                pFeatureCursor = pNewFeaCls.Insert(true);
                //int index = Routes_Line.IndexOf('"');//第一个双引号在字符串中的位置
                //string aryLineFirst = Routes_Line.Substring(0, index - 1);
                string[] aryLineFirstArray = Routes_Line.Split(',');
                pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
                pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);
                pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
                pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
                pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
                pFeatureBuffer.set_Value(7, aryLineFirstArray[5]);
                pFeatureBuffer.set_Value(8, aryLineFirstArray[6]);
                pFeatureBuffer.set_Value(9, aryLineFirstArray[7]);
                pFeatureBuffer.set_Value(10, aryLineFirstArray[8]);
                pFeatureBuffer.set_Value(11, aryLineFirstArray[9]);
                pFeatureBuffer.set_Value(12, aryLineFirstArray[10]);
                pFeatureBuffer.set_Value(13, "0");//拥堵状况设为0 

                SumPath = "";

                SumPath += PrePath;
                int flag = 1;
                while ((Subpath_Line = SR_Subpathscsv.ReadLine()) != null)
                {
                    int indexTemp = Subpath_Line.IndexOf(',');
                    int indexYin1 = Subpath_Line.IndexOf('"');
                    int indexYin2 = Subpath_Line.LastIndexOf('"');
                    string path = Subpath_Line.Substring(indexYin1+1, indexYin2 - indexYin1 - 1);
                    if (flag != 1)
                    {
                        path = path.Substring(path.IndexOf(';')+1,path.Length - path.IndexOf(';') - 1);
                    }
                    string id = Subpath_Line.Substring(0, indexTemp);
                    if (id != aryLineFirstArray[0])
                    {
                        PrePath = path;
                        PrePath += ";";
                        break;
                    }
                    SumPath += path;
                    SumPath += ";";
                    flag++;
                }
                SumPath = SumPath.Substring(0, SumPath.Length - 1);
                string temp = SumPath;
                int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
                pFeatureBuffer.set_Value(14,SumOfpoint.ToString());


                if (SumOfpoint > 11000)
                {
                    int time = SumOfpoint / 11000 + 1;
                    string[] subAryLine = new string[time];
                    string tempStr = SumPath;
                    int indexPre = 0;
                    int indexSub = 0;
                    int t = 0;
                    while (tempStr.Length > 0)
                    {
                        int indexT = 0;
                        for (int i = 0; i < 11000; i++)
                        {
                            if ((t + 1) == time)
                            {
                                indexSub = tempStr.Length;
                                subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                                t++;
                                tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                                break;
                            }
                            indexT = tempStr.IndexOf(';', indexT + 1);
                            if (indexT == -1)
                            {
                                break;
                            }
                            else
                            {
                                indexSub = indexT;
                            }
                        }
                        if (t == time)
                            continue;
                        indexSub++;
                        subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                        t++;
                        tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                    }
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
                }
                else
                {
                    string[] strArray = new string[1];
                    strArray[0] = SumPath;
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
                }
                ProgressFm.setPos((int)((++num) / (double)(NumOfHead * NumOfRecord * 2) * 100));//设置进度条位置
            }
            SR_Subpathscsv.Close();
            FS_Subpathscsv.Close();
            SR_Routescsv.Close();
            FS_Routescsv.Close();
        }
        private void Car_Polyline_subPaths(IFeatureWorkspace pFWS, string shpName, string InputFilePath)
        {
            //开始添加属性字段；
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            //添加字段“OID”；
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            //设置生成图的空间坐标参考系统；
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //投影坐标系
            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
            spatialReferenceResolution2.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
            spatialReferenceTolerance2.SetDefaultXYTolerance();
            geometryDefEdit.SpatialReference_2 = spatialReference2;

            //添加字段“Shape”;
            IField geometryField = new FieldClass();
            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(geometryField);
            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            //添加字段车站对序号
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "序号ID";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段step_num
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "step_num";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段subPath_time
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "subPath_time(s)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段subPath_distance
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "subPath_distance(km)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Area
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Area";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Traffic_status
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Traffic_status";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段“长度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "点对数";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            //在工作空间中生成FeatureClass;
            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


            System.IO.FileStream FScsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            System.IO.StreamReader SRcsv = new System.IO.StreamReader(FScsv, utf8WithoutBom, true);
            string aryLine = "";
            IFeatureBuffer pFeatureBuffer = null;
            IFeatureCursor pFeatureCursor = null;
            while ((aryLine = SRcsv.ReadLine()) != null)
            {
                NumOfRecord++;
            }
            SRcsv.BaseStream.Seek(0, SeekOrigin.Begin);
            int num = 0;
            while ((aryLine = SRcsv.ReadLine()) != null)
            {
                num++;
                for (int tt = 0; tt <= 1; tt++)
                {
                    pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
                    pFeatureCursor = pNewFeaCls.Insert(true);
                    int index = aryLine.IndexOf('"');//第一个双引号在字符串中的位置
                    string aryLineFirst = aryLine.Substring(0, index - 1);
                    string[] aryLineFirstArray = aryLineFirst.Split(',');
                    pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
                    pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);

                    pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
                    pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
                    pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
                    pFeatureBuffer.set_Value(7, aryLineFirstArray[5]);
                    pFeatureBuffer.set_Value(8, aryLineFirstArray[6]);
                    pFeatureBuffer.set_Value(9, aryLineFirstArray[7]);
                    pFeatureBuffer.set_Value(10, aryLineFirstArray[8]);

                    string aryLineLast = aryLine.Substring(index + 1, aryLine.Length - index - 2); //下面几行通过分号个数计算点对数
                    string temp = aryLineLast;
                    int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
                    int numOfpoint = 0;
                    if (tt == 0)
                    {
                        numOfpoint = int.Parse(aryLineFirstArray[10]);
                        pFeatureBuffer.set_Value(11, aryLineFirstArray[9]); //状态注意更改
                        pFeatureBuffer.set_Value(12, numOfpoint.ToString());
                    }
                    else
                    {
                        string temps = "1";
                        pFeatureBuffer.set_Value(11, temps); //状态注意更改
                        numOfpoint = SumOfpoint - int.Parse(aryLineFirstArray[10]);
                        pFeatureBuffer.set_Value(12, numOfpoint.ToString());
                    }
                    if (SumOfpoint > 11000)
                    {
                        int time = SumOfpoint / 11000 + 1;
                        string[] subAryLine = new string[time];
                        string tempStr = aryLineLast;
                        int indexPre = 0;
                        int indexSub = 0;
                        int t = 0;
                        while (tempStr.Length > 0)
                        {
                            int indexT = 0;
                            for (int i = 0; i < 11000; i++)
                            {
                                if ((t + 1) == time)
                                {
                                    indexSub = tempStr.Length;
                                    subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                                    t++;
                                    tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                                    break;
                                }
                                indexT = tempStr.IndexOf(';', indexT + 1);
                                if (indexT == -1)
                                {
                                    break;
                                }
                                else
                                {
                                    indexSub = indexT;
                                }
                            }
                            if (t == time)
                                continue;
                            indexSub++;
                            subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                            t++;
                            tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                        }
                        MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
                    }
                    else
                    {
                        string[] strArray = new string[1];
                        strArray[0] = aryLineLast;
                        MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
                    }
                }
                ProgressFm.setPos(50 / NumOfHead + (int)((num) / (double)(NumOfHead * NumOfRecord * 2) * 100));//设置进度条位置
            }
            SRcsv.Close();
            FScsv.Close();
        }
        private void PublicTrans_Polyline_Subpaths(IFeatureWorkspace pFWS, string shpName, string InputFilePath)
        {
            //开始添加属性字段；
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            //添加字段“OID”；
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            //设置生成图的空间坐标参考系统；
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //投影坐标系
            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
            spatialReferenceResolution2.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
            spatialReferenceTolerance2.SetDefaultXYTolerance();
            geometryDefEdit.SpatialReference_2 = spatialReference2;

            //添加字段“Shape”;
            IField geometryField = new FieldClass();
            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(geometryField);
            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            //添加字段车站对序号
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "序号ID";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段step_num
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "step_num";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“起始点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点经度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点经度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“终止点纬度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终止点纬度";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段subPath_time
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "subPath_time(s)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段subPath_distance
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "subPath_distance(km)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Vehicle_info
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Vehicle_info";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段Traffic_cond
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "Traffic_status";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            //添加字段“点对数q”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "点对数";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);

            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            //在工作空间中生成FeatureClass;
            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


            System.IO.FileStream FScsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            System.IO.StreamReader SRcsv = new System.IO.StreamReader(FScsv, utf8WithoutBom, true);
            string aryLine = "";
            IFeatureBuffer pFeatureBuffer = null;
            IFeatureCursor pFeatureCursor = null;
            while ((aryLine = SRcsv.ReadLine()) != null)
            {
                NumOfRecord++;
            }
            SRcsv.BaseStream.Seek(0, SeekOrigin.Begin);
            int num = 0;
            while ((aryLine = SRcsv.ReadLine()) != null)
            {
                num++;
                pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
                pFeatureCursor = pNewFeaCls.Insert(true);
                int index = aryLine.IndexOf('"');//第一个双引号在字符串中的位置
                string aryLineFirst = aryLine.Substring(0, index - 1);
                string[] aryLineFirstArray = aryLineFirst.Split(',');
                pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
                pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);
                pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
                pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
                pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
                pFeatureBuffer.set_Value(7, aryLineFirstArray[5]);
                pFeatureBuffer.set_Value(8, aryLineFirstArray[6]);
                pFeatureBuffer.set_Value(9, aryLineFirstArray[7]);
                pFeatureBuffer.set_Value(10, aryLineFirstArray[8]);
                pFeatureBuffer.set_Value(11, aryLineFirstArray[9]);

                string aryLineLast = aryLine.Substring(index + 1, aryLine.Length - index - 2); //下面几行通过分号个数计算点对数
                string temp = aryLineLast;
                int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
                pFeatureBuffer.set_Value(12, SumOfpoint.ToString());
                if (SumOfpoint > 11000)
                {
                    int time = SumOfpoint / 11000 + 1;
                    string[] subAryLine = new string[time];
                    string tempStr = aryLineLast;
                    int indexPre = 0;
                    int indexSub = 0;
                    int t = 0;
                    while (tempStr.Length > 0)
                    {
                        int indexT = 0;
                        for (int i = 0; i < 11000; i++)
                        {
                            if ((t + 1) == time)
                            {
                                indexSub = tempStr.Length;
                                subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                                t++;
                                tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                                break;
                            }
                            indexT = tempStr.IndexOf(';', indexT + 1);
                            if (indexT == -1)
                            {
                                break;
                            }
                            else
                            {
                                indexSub = indexT;
                            }
                        }
                        if (t == time)
                            continue;
                        indexSub++;
                        subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                        t++;
                        tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                    }
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
                }
                else
                {
                    string[] strArray = new string[1];
                    strArray[0] = aryLineLast;
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
                }
                ProgressFm.setPos(50 / NumOfHead + (int)((num) / (double)(NumOfHead * NumOfRecord * 2) * 100));//设置进度条位置
            }
            SRcsv.Close();
            FScsv.Close();
        }


        private void ReadExl_Click(object sender, EventArgs e)
        {
            if (DataMode == -1 || VehicleMode == -1)
            {
                MessageBox.Show("未选择交通方式或要素类型！");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Csv文件(*.csv;*.csv)|*.csv;*.csv|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //ExlFilePath.Text = ofd.FileName;
            AxmapCtrl.ClearLayers();
            string InputFilePath = ofd.FileName;
            string shpDirectoryPath = System.IO.Path.GetDirectoryName(InputFilePath);
            string shpName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
            string shpFullName = shpName + ".shp";
            string prjName = shpName + ".prj";
            string dbfName = shpName + ".dbf";
            string shxName = shpName + ".shx";
            string sbnName = shpName + ".sbn";
            string xmlName = shpName + ".shp.xml";
            string sbxName = shpName + ".sbx";
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + shpFullName))
                System.IO.File.Delete(shpDirectoryPath + "\\" + shpFullName);
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + prjName))
                System.IO.File.Delete(shpDirectoryPath + "\\" + prjName);
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + dbfName))
                System.IO.File.Delete(shpDirectoryPath + "\\" + dbfName);
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + shxName))
                System.IO.File.Delete(shpDirectoryPath + shxName);
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbnName))
                System.IO.File.Delete(shpDirectoryPath + "\\" + sbnName);
            if (System.IO.File.Exists(shpDirectoryPath + "\\" + xmlName))
                System.IO.File.Delete(shpDirectoryPath + "\\" + xmlName);
             if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbxName))
                 System.IO.File.Delete(shpDirectoryPath + "\\" + sbxName);
            //生成shp
            string shpFileName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
            //打开生成shapefile的工作空间；
            IFeatureWorkspace pFWS = null;
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            try
            {
                IWorkspace pWs = pWSF.OpenFromFile(shpDirectoryPath + "\\", 0);
                pFWS = pWs as IFeatureWorkspace;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
            //开始添加属性字段；
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            //添加字段“OID”；
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);
            
            //设置生成图的空间坐标参考系统；
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            //ISpatialReference spatialReference =  spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            //ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;
            //spatialReferenceResolution.ConstructFromHorizon();
            //ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;
            //spatialReferenceTolerance.SetDefaultXYTolerance();
            //geometryDefEdit.SpatialReference_2 = spatialReference;
            //投影坐标系
            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
            spatialReferenceResolution2.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
            spatialReferenceTolerance2.SetDefaultXYTolerance();
            geometryDefEdit.SpatialReference_2 = spatialReference2;


            //添加字段“Shape”;
            IField geometryField = new FieldClass();
            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(geometryField);
            IField nameField = new FieldClass();
            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
            //添加字段车站配对序号
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "车站对序号";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段起始站
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "起始站";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段终点站
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "终点站";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段消耗时间
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "消耗时间(秒)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“长度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "长度(千米)";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            //添加字段“长度”；
            nameField = new FieldClass();
            nameFieldEdit = (IFieldEdit)nameField;
            nameFieldEdit.Name_2 = "点对数";
            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            nameFieldEdit.Length_2 = 20;
            fieldsEdit.AddField(nameField);
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
            //在工作空间中生成FeatureClass;

            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
            IFeature feature = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //添加feature

            System.Text.Encoding encoding = GetType(InputFilePath); //Encoding.ASCII;//  
            System.IO.FileStream FScsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader SRcsv = new System.IO.StreamReader(FScsv, encoding);

            List<ESRI.ArcGIS.Geometry.IPoint> pts = new List<ESRI.ArcGIS.Geometry.IPoint>();
            IPolyline py = new PolylineClass();
            string aryLine = "";
            List<IFeatureBuffer> pyList = new List<IFeatureBuffer>();
            while ((aryLine = SRcsv.ReadLine()) != null)
            {
                if (aryLine == "")
                    continue;
                int index = aryLine.IndexOf('"');//第一个双引号在字符串中的位置
                string aryLineFirst = aryLine.Substring(0, index - 1);
                string[] aryLineFirstArray = aryLineFirst.Split(',');
                string aryLineLast = aryLine.Substring(index + 1, aryLine.Length - index - 2);
                string temp = aryLineLast;
                int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
                //feature = pNewFeaCls.CreateFeature();


                IFeatureBuffer pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
                IFeatureCursor pFeatureCursor = pNewFeaCls.Insert(true);

                pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
                pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);
                pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
                pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
                pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
                pFeatureBuffer.set_Value(7, SumOfpoint.ToString());
                IMap pmap = AxmapCtrl.Map;
                IActiveView pactive = pmap as IActiveView;
                
                if (SumOfpoint > 11000)
                {
                    int time = SumOfpoint / 11000 + 1;
                    string[] subAryLine = new string[time];
                    string tempStr = aryLineLast;
                    int indexPre = 0;
                    int indexSub = 0;
                    int t = 0;
                    while (tempStr.Length > 0)
                    {
                        int indexT = 0;
                        for (int i = 0; i < 11000; i++)
                        {
                            if ((t + 1) == time)
                            {
                                indexSub = tempStr.Length;
                                subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                                t++;
                                tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                                break;
                            }
                            indexT = tempStr.IndexOf(';', indexT + 1);
                            if (indexT == -1)
                            {
                                break;
                            }
                            else
                            {
                                indexSub = indexT;
                            }
                        }
                        if (t == time)
                            continue;
                        indexSub++;
                        subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
                        t++;
                        tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
                    }
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
                }
                else
                {
                    string[] strArray = new string[1];
                    strArray[0] = aryLineLast;
                    MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
                   
                }
            }
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            MessageBox.Show("Finished! " + (ts2.TotalMilliseconds / 1000).ToString());
        }
//         private void MakeFeature(IFeature feature,string[] strArray)
//         {
//             ISegmentCollection pPath = new PolylineClass();//多态
//             IPolyline py = new PolylineClass();
//             IPointCollection ptc = py as IPointCollection;
//             object missing = Type.Missing;
//             for (int j = 0; j < strArray.Length; j++)
//             {
//                 string str = strArray[j];
//                 string[] pointCpArray = str.Split(';');
//                 for (int i = 0; i < pointCpArray.Length; i++)
//                 {
//                     
//                     //int index0 = pointCpArray[i].IndexOf(',');
//                     //int length0 = pointCpArray[i].Length;
//                     //string xPosOri = pointCpArray[i].Substring(0, index0);
//                     //string yPosOri = pointCpArray[i].Substring(index0 + 1, length0 - index0 - 1);
//                     string[] pos = pointCpArray[i].Split(',');
//                     string xPosOri = pos[0];
//                     string yPosOri = pos[1];
//                     ESRI.ArcGIS.Geometry.IPoint pPoint = new PointClass();
//                     pPoint.X = double.Parse(xPosOri);
//                     pPoint.Y = double.Parse(yPosOri);
//     
//                     ptc.AddPoint(pPoint, missing, missing);
//                 }
//                 //for (int i = 0; i < pointCpArray.Length - 1; i++)
//                 //{
//                 //    int index0 = pointCpArray[i].IndexOf(',');
//                 //    int length0 = pointCpArray[i].Length;
//                 //    string xPosOri = pointCpArray[i].Substring(0, index0);
//                 //    string yPosOri = pointCpArray[i].Substring(index0 + 1, length0 - index0 - 1);
//                 //    int index1 = pointCpArray[i + 1].IndexOf(',');
//                 //    int length1 = pointCpArray[i + 1].Length;
//                 //    string xPosEnd = pointCpArray[i + 1].Substring(0, index1);
//                 //    string yPosEnd = pointCpArray[i + 1].Substring(index1 + 1, length1 - index1 - 1);
// 
//                 //    ESRI.ArcGIS.Geometry.ILine pLine = new LineClass();
//                 //    ESRI.ArcGIS.Geometry.IPoint pPointFrom = new PointClass();
//                 //    ESRI.ArcGIS.Geometry.IPoint pPointTo = new PointClass();
//                 //    double[,] pos = new double[2, 2];
//                 //    pos[0, 0] = double.Parse(xPosOri);
//                 //    pos[0, 1] = double.Parse(yPosOri);
//                 //    pos[1, 0] = double.Parse(xPosEnd);
//                 //    pos[1, 1] = double.Parse(yPosEnd);
//                 //    pPointFrom.X = pos[0, 0];
//                 //    pPointFrom.Y = pos[0, 1];
//                 //    pPointTo.X = pos[1, 0];
//                 //    pPointTo.Y = pos[1, 1];
//                 //    pLine.PutCoords(pPointFrom, pPointTo);
//                 //    pPath.AddSegment(pLine as ISegment);
//                 //}
//             }
//             //py = pPath as IPolyline;
//             (py as ITopologicalOperator).Simplify();
//             feature.Shape = py;
//             feature.Store();
//         }
        public void MakeFeature2(IFeatureBuffer featureBuffer,IFeatureCursor pFeatureCursor, string[] strArray)
        {   
            IPolyline py = new PolylineClass();
            IPointCollection ptc = py as IPointCollection;
            object missing = Type.Missing;
            ESRI.ArcGIS.Geometry.IPoint pPoint = new PointClass();
            for (int j = 0; j < strArray.Length; j++)
            {
                string str = strArray[j];
                string[] pointCpArray = str.Split(';');
                for (int i = 0; i < pointCpArray.Length; i++)
                {
                    string[] pos = pointCpArray[i].Split(',');
                    string xPosOri = pos[0];
                    string yPosOri = pos[1];
                    pPoint.X = double.Parse(xPosOri);
                    pPoint.Y = double.Parse(yPosOri);
                    ptc.AddPoint(pPoint, missing, missing);
                }
            }
            (py as ITopologicalOperator).Simplify();
            featureBuffer.Shape = py;
            pFeatureCursor.InsertFeature(featureBuffer);
        }
        private void BtnOriginArrayRead_Click(object sender, EventArgs e)
        {
            if (DataMode == -1 || VehicleMode == -1)
            {
                MessageBox.Show("未选择交通方式或要素类型！");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            ofd.Filter = "txt文件(*.txt;*.txt)|*.txt;*.txt|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            List<string> originArray = new List<string>();
            string path = ofd.FileName;

            StreamReader sr = new StreamReader(path, utf8WithoutBom);
            string line = null;
            string dirPath = System.IO.Path.GetDirectoryName(path);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FS = new System.IO.FileStream("origin" + DateTime.Now.ToLongDateString() + DateTime.Now.Hour.ToString() +"."+DateTime.Now.Minute+"." + DateTime.Now.Second.ToString() + ".txt", System.IO.FileMode.OpenOrCreate);
            SW = new System.IO.StreamWriter(FS, utf8WithoutBom);
            while ((line = sr.ReadLine()) != null)
            {
                NumOfHead++;
            }
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            int progress = 1;
            ProgressFm.Show(this);
            while((line = sr.ReadLine()) != null)
            {
                //string fileName = dirPath + "\\" + line + "_subpaths.csv";
                string fileName = dirPath + "\\" + line;
                ProgressFm.setPos(((++progress) / NumOfHead) * 100);//设置进度条位置
                CsvToShp(fileName);
                
            }
            sr.Close();
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            SW.Close();
            FS.Close();
            ProgressFm.Close();
            MessageBox.Show("Finished! " + (ts2.TotalMilliseconds / 1000).ToString());
        }

        private void CarModeRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (CarModeRBtn.Checked == true)
            {
                VehicleMode = 1;
            }
            else
            {
                VehicleMode = -1;
            }
        }

        private void PublicTranspRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (PublicTranspRBtn.Checked == true)
            {
                VehicleMode = 2;
            }
            else
            {
                VehicleMode = -1;
            }
        }

        private void RidingModeRbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (RidingModeRbtn.Checked == true)
            {
                VehicleMode = 3;
            }
            else
            {
                VehicleMode = -1;
            }
        }

        private void WalkingModeRbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (WalkingModeRbtn.Checked == true)
            {
                VehicleMode = 4;
            }
            else
            {
                VehicleMode = -1;
            }
        }

        private void PolylineRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (PolylineRBtn.Checked == true)
            {
                DataMode = 1;
            }
            else
            {
                DataMode = -1;
            }
        }

        private void PointRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (PointRBtn.Checked == true)
            {
                DataMode = 1;
            }
            else
            {
                DataMode = -1;
            }
        }

        private void PolygonRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (PolygonRBtn.Checked == true)
            {
                DataMode = 1;
            }
            else
            {
                DataMode = -1;
            }
        }
        private void CsvToShp(string csvFilePath)
        {
            if (DataMode == 1)//polyline
            {
                if (VehicleMode == 1) // car
                {
                    string InputFilePath = csvFilePath + "_routes.csv";
                    string shpDirectoryPath = System.IO.Path.GetDirectoryName(InputFilePath);
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    string shpFullName = shpName + ".shp";
                    string prjName = shpName + ".prj";
                    string dbfName = shpName + ".dbf";
                    string shxName = shpName + ".shx";
                    string sbnName = shpName + ".sbn";
                    string xmlName = shpName + ".shp.xml";
                    string sbxName = shpName + ".sbx";
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shpFullName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + shpFullName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + prjName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + prjName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + dbfName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + dbfName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shxName))
                        System.IO.File.Delete(shpDirectoryPath + shxName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbnName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbnName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + xmlName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + xmlName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbxName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbxName);
                    //生成shp
                    string shpFileName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    //打开生成shapefile的工作空间；
                    IFeatureWorkspace pFWS = null;
                    IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
                    try
                    {
                        IWorkspace pWs = pWSF.OpenFromFile(shpDirectoryPath + "\\", 0);
                        pFWS = pWs as IFeatureWorkspace;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    Car_Polyline_Routes(pFWS, shpName, InputFilePath);


                    InputFilePath = csvFilePath + "_subpaths.csv";
                    shpDirectoryPath = System.IO.Path.GetDirectoryName(InputFilePath);
                    shpName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    shpFullName = shpName + ".shp";
                    prjName = shpName + ".prj";
                    dbfName = shpName + ".dbf";
                    shxName = shpName + ".shx";
                    sbnName = shpName + ".sbn";
                    xmlName = shpName + ".shp.xml";
                    sbxName = shpName + ".sbx";
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shpFullName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + shpFullName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + prjName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + prjName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + dbfName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + dbfName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shxName))
                        System.IO.File.Delete(shpDirectoryPath + shxName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbnName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbnName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + xmlName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + xmlName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbxName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbxName);
                    //生成shp
                    shpFileName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    //打开生成shapefile的工作空间；
                    pFWS = null;
                    pWSF = new ShapefileWorkspaceFactoryClass();
                    try
                    {
                        IWorkspace pWs = pWSF.OpenFromFile(shpDirectoryPath + "\\", 0);
                        pFWS = pWs as IFeatureWorkspace;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    Car_Polyline_subPaths(pFWS, shpName, InputFilePath);
                }
                if (VehicleMode == 2)
                {
                    string InputFilePath = csvFilePath + "_routes.csv";
                    string shpDirectoryPath = System.IO.Path.GetDirectoryName(InputFilePath);
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    string shpFullName = shpName + ".shp";
                    string prjName = shpName + ".prj";
                    string dbfName = shpName + ".dbf";
                    string shxName = shpName + ".shx";
                    string sbnName = shpName + ".sbn";
                    string xmlName = shpName + ".shp.xml";
                    string sbxName = shpName + ".sbx";
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shpFullName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + shpFullName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + prjName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + prjName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + dbfName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + dbfName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shxName))
                        System.IO.File.Delete(shpDirectoryPath + shxName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbnName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbnName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + xmlName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + xmlName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbxName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbxName);
                    //生成shp
                    string shpFileName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    //打开生成shapefile的工作空间；
                    IFeatureWorkspace pFWS = null;
                    IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
                    try
                    {
                        IWorkspace pWs = pWSF.OpenFromFile(shpDirectoryPath + "\\", 0);
                        pFWS = pWs as IFeatureWorkspace;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    PublicTrans_Polyline_Routes(pFWS, shpName, InputFilePath);


                    InputFilePath = csvFilePath + "_subpaths.csv";
                    shpDirectoryPath = System.IO.Path.GetDirectoryName(InputFilePath);
                    shpName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    shpFullName = shpName + ".shp";
                    prjName = shpName + ".prj";
                    dbfName = shpName + ".dbf";
                    shxName = shpName + ".shx";
                    sbnName = shpName + ".sbn";
                    xmlName = shpName + ".shp.xml";
                    sbxName = shpName + ".sbx";
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shpFullName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + shpFullName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + prjName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + prjName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + dbfName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + dbfName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + shxName))
                        System.IO.File.Delete(shpDirectoryPath + shxName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbnName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbnName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + xmlName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + xmlName);
                    if (System.IO.File.Exists(shpDirectoryPath + "\\" + sbxName))
                        System.IO.File.Delete(shpDirectoryPath + "\\" + sbxName);
                    //生成shp
                    shpFileName = System.IO.Path.GetFileNameWithoutExtension(InputFilePath);
                    //打开生成shapefile的工作空间；
                    pFWS = null;
                    pWSF = new ShapefileWorkspaceFactoryClass();
                    try
                    {
                        IWorkspace pWs = pWSF.OpenFromFile(shpDirectoryPath + "\\", 0);
                        pFWS = pWs as IFeatureWorkspace;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    PublicTrans_Polyline_Subpaths(pFWS, shpName, InputFilePath);
                }

            }
        }
    }
}



       