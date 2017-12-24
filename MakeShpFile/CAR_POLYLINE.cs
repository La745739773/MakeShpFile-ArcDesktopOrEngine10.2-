//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using ESRI.ArcGIS.Geodatabase;
//using Microsoft.Office.Interop.Excel;
//using ESRI.ArcGIS.DataSourcesFile;
//using ESRI.ArcGIS.Geometry;
//using System.Data.OleDb;
//using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Carto;
//using System.Diagnostics;
//using System.IO;

//namespace MakeShpFile
//{
//    public class CAR_POLYLINE
//    {
//        static public void Car_Polyline(IFeatureWorkspace pFWS, string shpName, string InputFilePath)
//        {
//            //开始添加属性字段；
//            IFields fields = new FieldsClass();
//            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
//            //添加字段“OID”；
//            IField oidField = new FieldClass();
//            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
//            oidFieldEdit.Name_2 = "OID";
//            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
//            fieldsEdit.AddField(oidField);

//            //设置生成图的空间坐标参考系统；
//            IGeometryDef geometryDef = new GeometryDefClass();
//            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
//            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

//            //投影坐标系
//            ISpatialReferenceFactory spatialReferenceFactory2 = new SpatialReferenceEnvironmentClass();
//            ISpatialReference spatialReference2 = spatialReferenceFactory2.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_CM_120E);
//            ISpatialReferenceResolution spatialReferenceResolution2 = (ISpatialReferenceResolution)spatialReference2;
//            spatialReferenceResolution2.ConstructFromHorizon();
//            ISpatialReferenceTolerance spatialReferenceTolerance2 = spatialReferenceResolution2 as ISpatialReferenceTolerance;
//            spatialReferenceTolerance2.SetDefaultXYTolerance();
//            geometryDefEdit.SpatialReference_2 = spatialReference2;

//            //添加字段“Shape”;
//            IField geometryField = new FieldClass();
//            IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
//            geometryFieldEdit.Name_2 = "Shape";
//            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
//            geometryFieldEdit.GeometryDef_2 = geometryDef;
//            fieldsEdit.AddField(geometryField);
//            IField nameField = new FieldClass();
//            IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
//            //添加字段车站对序号
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "序号ID";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段step_num
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "step_num";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段“起始点经度”；
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "起始点经度";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段“起始点纬度”；
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "起始点纬度";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段“终止点经度”；
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "终止点经度";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段“终止点纬度”；
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "终止点纬度";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段subPath_time
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "subPath_time(s)";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段subPath_distance
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "subPath_distance(km)";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段Area
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "Area";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);
//            //添加字段Traffic_status
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "Traffic_status";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);

//            //添加字段“长度”；
//            nameField = new FieldClass();
//            nameFieldEdit = (IFieldEdit)nameField;
//            nameFieldEdit.Name_2 = "点对数";
//            nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
//            nameFieldEdit.Length_2 = 20;
//            fieldsEdit.AddField(nameField);

//            IFieldChecker fieldChecker = new FieldCheckerClass();
//            IEnumFieldError enumFieldError = null;
//            IFields validatedFields = null;
//            fieldChecker.ValidateWorkspace = (IWorkspace)pFWS;
//            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
//            //在工作空间中生成FeatureClass;
//            IFeatureClass pNewFeaCls = pFWS.CreateFeatureClass(shpName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


//            System.IO.FileStream FScsv = new System.IO.FileStream(InputFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
//            System.IO.StreamReader SRcsv = new System.IO.StreamReader(FScsv, utf8WithoutBom, true);
//            string aryLine = "";
//            IFeatureBuffer pFeatureBuffer = null;
//            IFeatureCursor pFeatureCursor = null;
//            while ((aryLine = SRcsv.ReadLine()) != null)
//            {
//                NumOfRecord++;
//            }
//            SRcsv.BaseStream.Seek(0, SeekOrigin.Begin);
//            int num = 0;
//            while ((aryLine = SRcsv.ReadLine()) != null)
//            {
//                num++;
//                for (int tt = 0; tt <= 1; tt++)
//                {
//                    pFeatureBuffer = pNewFeaCls.CreateFeatureBuffer();
//                    pFeatureCursor = pNewFeaCls.Insert(true);
//                    int index = aryLine.IndexOf('"');//第一个双引号在字符串中的位置
//                    string aryLineFirst = aryLine.Substring(0, index - 1);
//                    string[] aryLineFirstArray = aryLineFirst.Split(',');
//                    pFeatureBuffer.set_Value(2, aryLineFirstArray[0]);
//                    pFeatureBuffer.set_Value(3, aryLineFirstArray[1]);

//                    pFeatureBuffer.set_Value(4, aryLineFirstArray[2]);
//                    pFeatureBuffer.set_Value(5, aryLineFirstArray[3]);
//                    pFeatureBuffer.set_Value(6, aryLineFirstArray[4]);
//                    pFeatureBuffer.set_Value(7, aryLineFirstArray[5]);
//                    pFeatureBuffer.set_Value(8, aryLineFirstArray[6]);
//                    pFeatureBuffer.set_Value(9, aryLineFirstArray[7]);
//                    pFeatureBuffer.set_Value(10, aryLineFirstArray[8]);




//                    string aryLineLast = aryLine.Substring(index + 1, aryLine.Length - index - 2); //下面几行通过分号个数计算点对数
//                    string temp = aryLineLast;
//                    int SumOfpoint = temp.Length - temp.Replace(";", "").Length;
//                    int numOfpoint = 0;
//                    if (tt == 0)
//                    {
//                        numOfpoint = int.Parse(aryLineFirstArray[10]);
//                        pFeatureBuffer.set_Value(11, aryLineFirstArray[9]); //状态注意更改
//                        pFeatureBuffer.set_Value(12, numOfpoint.ToString());
//                    }
//                    else
//                    {
//                        pFeatureBuffer.set_Value(12, "1"); //状态注意更改
//                        numOfpoint = SumOfpoint - int.Parse(aryLineFirstArray[10]);
//                        pFeatureBuffer.set_Value(12, numOfpoint.ToString());
//                    }
//                    if (SumOfpoint > 11000)
//                    {
//                        int time = SumOfpoint / 11000 + 1;
//                        string[] subAryLine = new string[time];
//                        string tempStr = aryLineLast;
//                        int indexPre = 0;
//                        int indexSub = 0;
//                        int t = 0;
//                        while (tempStr.Length > 0)
//                        {
//                            int indexT = 0;
//                            for (int i = 0; i < 11000; i++)
//                            {
//                                if ((t + 1) == time)
//                                {
//                                    indexSub = tempStr.Length;
//                                    subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
//                                    t++;
//                                    tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
//                                    break;
//                                }
//                                indexT = tempStr.IndexOf(';', indexT + 1);
//                                if (indexT == -1)
//                                {
//                                    break;
//                                }
//                                else
//                                {
//                                    indexSub = indexT;
//                                }
//                            }
//                            if (t == time)
//                                continue;
//                            indexSub++;
//                            subAryLine[t] = tempStr.Substring(indexPre, indexSub - 1);
//                            t++;
//                            tempStr = tempStr.Substring(indexSub, tempStr.Length - indexSub);
//                        }
//                        MakeFeature2(pFeatureBuffer, pFeatureCursor, subAryLine);
//                    }
//                    else
//                    {
//                        string[] strArray = new string[1];
//                        strArray[0] = aryLineLast;
//                        MakeFeature2(pFeatureBuffer, pFeatureCursor, strArray);
//                    }
//                }
//                ProgressFm.setPos((int)((num) / (double)(NumOfHead * NumOfRecord) * 100));//设置进度条位置
//            }
//        }
//    }
//}
