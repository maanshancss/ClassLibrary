/* -----------------------------------------------------------------------
 *    ��Ȩ���У� ��Ȩ����(C) 2006��EYE
 *    �ļ���ţ� 0001
 *    �ļ����ƣ� Excel.cs
 *    ϵͳ��ţ� E-Eye_0001
 *    ϵͳ���ƣ� SFWL����ϵͳ
 *    ģ���ţ� 0001
 *    ģ�����ƣ� Excel���ݵ��뵼��
 *    ����ĵ��� 
 *    ������ڣ� 2006-5-11 9:33:56
 *    �������ߣ� �ָ���
 *    ����ժҪ�� ���Excel�����ļ�API��д����
 *    ���������� �ýӿ���10������
 *					01 DataSource			����Դ
 *					02 Title				Excel�ļ�������
 *					03 FilePath				Դ�ļ�·��
 *					04 XMLFilePath			XML�ܹ��ļ�·��
 *	  			    05 IsOpen				�ж�Excel��������Ƿ��Ѿ���
 *					06 WriteType			д������(��ͨ����д��׷��)
 *					07 SheetName			Sheet������
 *					08 BackColor			������ɫ
 *					09 ForeColor			������ɫ
 *					10 Font					������ʽ
 *    ���������� �ýӿڰ���10������:
 *					01 Open					��Excel�������
 *					02 Read					��ȡExcel�ļ�����Ч�������ݼ�
 *					03 ReadCell				��ȡĳ��Ԫ�������
 *					04 ReadCell				��ȡĳ��Ԫ������ݣ����У��з�ʽ
 *					05 Write				д����Excel�ļ���
 *					06 ReWrite				����ָ����д����д����
 *					07 WriteCell			д��������Ԫ��
 *					08 Close				��Դ�ͷ�
 *					09 OpenCreate			��Excel���������Excel�ļ��������򴴽�֮
 *					10 ActiveSheet			���ǰ��д���ݵ�Sheet��
 *    -----------------------------------------------------------------------
 * */
using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel; 
using System.Reflection;
using excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using las.foundation;
namespace MyUtility.OFFICE.MyExcel
{
	/// <summary>
	///  �� �� �ţ� 02
	///  �� �� �ƣ� CExcel 
	///  ����ժҪ�� ���Excel������󲿷ֲ���ʵ��
	///  ������ڣ� 2006-5-12 14:37:38
	///  �������ߣ� �ָ���
	/// </summary>
	public class CExcelManager : IExcelManager
	{
		#region ��Ա����
		/// <summary>
		/// Excel����֮ǰʱ��
		/// </summary>
		private DateTime beforeTime;            
		/// <summary>
		/// Excel����֮��ʱ��
		/// </summary>
		private DateTime afterTime;                
		/// <summary>
		/// ����Դ
		/// </summary>
		/// <summary>
		private System.Data.DataSet ds = null;
		/// Excel������
		/// </summary>
		private string title = null;
		/// <summary>
		/// Դ�ļ�·��
		/// </summary>
		private string inputFilePath = "";
		/// <summary>
		/// XML�ļ�·��
		/// </summary>
		private string _xmlfilepath = "";
		private EnumType.WriteType m_writeType;
		excel.Application app;
		excel.Workbook wb;
		excel.Worksheet ws;
		excel.Range rng;
		excel.TextBox tb;
		private const int iMaxRow = 60000;
		private const int iMaxCol = 255;
		private const int iMaxSheet = 255;
		private string m_sheetName = "";
		private object objOpt = System.Reflection.Missing.Value;
		private System.Drawing.Color backcolor = System.Drawing.Color.White;
		private System.Drawing.Color forecolor = System.Drawing.Color.Black;
		private System.Drawing.Font font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)),false);
		#endregion

		#region Property
		/// <summary>
		/// ����Դ
		/// </summary>
		public DataSet DataSource
		{
			set
			{
				this.ds = value;
			}
			get
			{
				return this.ds;
			}
		}
		/// <summary>
		/// Excel�ļ�������
		/// </summary>
		public string Title
		{
			set
			{
				title = value;
			}
			get
			{
				return title;
			}
		}
		/// <summary>
		/// Դ�ļ�·��
		/// </summary>
		public string FilePath
		{
			set
			{
				inputFilePath = value;
			}
			get
			{
				return inputFilePath;
			}
		}
		/// <summary>
		/// XML�ܹ��ļ�·��
		/// </summary>
		public string XMLFilePath
		{
			set
			{
				_xmlfilepath = value;
			}
			get
			{
				return this._xmlfilepath ;
			}
		}
		/// <summary>
		/// �ж�Excel��������Ƿ��Ѿ���
		/// </summary>
		public bool IsOpen
		{
			get
			{
				if((wb != null) || (ws != null))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Excel�ļ�д������
		/// </summary>
		public EnumType.WriteType WriteType
		{
			get
			{
				return this.m_writeType;
			}
			set
			{
				m_writeType = value;
			}
		}
		/// <summary>
		/// Sheet������
		/// </summary>
		public string SheetName
		{
			get
			{
				return this.m_sheetName;
			}
			set
			{
				if(value.Length > 32)
				{
					throw new Exception("Sheet�������ܴ���32�ַ������飡");
				}
				else
				{
					this.m_sheetName = value;
				}
			}
		}
		/// <summary>
		/// ������ɫ
		/// </summary>
		public System.Drawing.Color BackColor
		{
			get
			{
				if(backcolor == Color.Empty)
					return Color.White;
				return backcolor;
			}
			set
			{
				backcolor = value;
			}
		}
		/// <summary>
		/// ������ɫ
		/// </summary>
		public System.Drawing.Color ForeColor
		{
			get
			{
				if(forecolor == Color.Empty)
					return Color.Black;
				return forecolor;
			}
			set
			{
				forecolor = value;
			}
		}
		/// <summary>
		/// ������ʽ
		/// </summary>
		public System.Drawing.Font Font
		{
			get
			{
				return font;
			}
			set
			{
				font = value;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// �������ƣ� CExcelManager
		/// ���������� ʵ����Excel����ʹ�䲻�ɼ�
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-13 16:30:39
		/// </summary>
		public CExcelManager()
		{
			try
			{
				beforeTime = DateTime.Now;
				app = new excel.ApplicationClass();
				app.Visible = false;
				app.DisplayAlerts = false;
				afterTime = DateTime.Now;
			}
			catch
			{
				this.KillExcelProcess();
				throw new Exception("����ExcelӦ�ö�����ִ���������Ļ�����");
			}
		}
		/// <summary>
		/// �������ƣ� Open
		/// ���������� ��
		/// ʵ�����̣� ��/����һ��excel�����ĵ�
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-13 16:23:23
		/// </summary>
		/// <returns></returns>
		public bool Open()
		{
			bool bolRetValue = false;
			string strPath = "";
			strPath = this.FilePath.ToString();

			if(!System.IO.File.Exists(strPath))
			{
				this.KillExcelProcess();
				throw new Exception("Դ�ļ������ڣ����飡");	
			}
			this.FileExteCheck(strPath);
			try
			{
				if(this.IsOpen == false)
				{
					wb = app.Workbooks.Open(strPath,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
					ws = (excel.Worksheet)wb.Worksheets[1];//.get_Item(1);
					//ws = (excel.Worksheet)(wb.Sheets.Add(objOpt,objOpt,objOpt,objOpt)) ;
					ws.Activate();
				}
				bolRetValue = true;
			}
			catch
			{
				this.KillExcelProcess();
				throw new Exception("�򿪻�����Excel�ĵ��������飡");
			}
			return bolRetValue ;
		}
		/// <summary>
		/// ����Sheet��
		/// </summary>
		public void ActiveSheet(EnumType.SheetIndex sheetindex)
		{
			if(((Excel.Sheets)(((Excel.ApplicationClass)(app)).Worksheets)).Count < (int) sheetindex)
			{
				this.Dispose();
				throw new Exception("Sheet�����ڣ����飡");
			}
			try
			{
				ws = (excel.Worksheet)wb.Worksheets[sheetindex];
				ws.Activate();
			}
			catch
			{
				this.Dispose();
				throw new Exception("Sheet����������飡");
			}
			int irowcount = 0;
			app.ActiveCell.SpecialCells(Excel.XlCellType.xlCellTypeLastCell,Type.Missing).Select();
			irowcount = app.ActiveCell.Row;
			if((this.WriteType != EnumType.WriteType.ReWrite) && (irowcount > iMaxRow))
			{
				this.Dispose();
				throw new Exception("��ǰSheet���Ѵ�洢���ޣ�����д�룬���飡");
			}
			int iSheetCount = wb.Sheets.Count ;
			for(int i = 1; i <= iSheetCount ; i++)
			{
				Excel.Worksheet wsT = (Excel.Worksheet) wb.Sheets[i];
				if(((this.m_sheetName != "") && ((this.m_sheetName != null)) && (this.m_sheetName.Trim().ToString() == wsT.Name.Trim().ToString()))
				 && (wsT.Name != ws.Name))
				{
					this.Dispose();
					throw new Exception("��ǰ�ļ�����ͬ��Sheet�����飡");
				}
			}
		}
		/// <summary>
		/// �������ƣ� FileExteCheck
		/// ���������� Excel�ļ���չ�����
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-19 9:09:35
		/// </summary>
		/// <param name="strpath"></param>
		private void FileExteCheck(string strpath)
		{
			string strFile;
			string strExt;

			strFile = strpath.Remove(strpath.Length - 4,4);
			strExt = Path.GetExtension(strpath);
			
			if(strExt != ".xls")
			{
				this.KillExcelProcess();
				throw new Exception("�ļ���ʽ����ȷ��");
			}
		}

		/// <summary>
		/// �������ƣ� Read
		/// ���������� ��ȡExcel�ļ�����Ч�������ݼ�
		/// ʵ�����̣� 
		/// 1.�������������ʼλ�ã�����λ��ȷ����ȡ��Χ
		/// 2.��ȡ������DataSet
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-11 9:20:53
		/// </summary>
		/// <returns>DataSet���ݼ�</returns>
		public System.Data.DataSet Read()
		{
			#region ��������
			System.Data.DataSet dsExcel = new DataSet();
			System.Data.DataTable dt = new System.Data.DataTable(); 
			System.Data.DataRow dr; 
			int i = 0;
			int j = 0;
			string strStart = "A1";
			string strEnd = "A1";
			Object[,] saRet; 
			Object[,] AData; 
			Excel.Range rng ;
			DataSet dsxmlSch = new DataSet();
			#endregion

			#region ����Xml�ܹ�
			// ��ѡ�ò�ͬ��Excel�ļ���Xml�ļ�
			if(this.XMLFilePath.Trim().ToString() == "")
			{
				dsxmlSch = this.LoadTemplate();
			}
			else
			{
				dsxmlSch = this.LoadTemplate(this.XMLFilePath.Trim().ToString());
			}
			// ��������
			this.AddColName(ref dt,dsxmlSch);
			#endregion

			#region ��ȡ״̬��
			try
			{
				// ��ȡ״̬��	rng = (Excel.Range)ws.Cells[0,0];��C#������д������ȷ
				rng = ws.get_Range("A1","B1");
			}
			catch
			{
				throw new Exception("��ȡExcel״̬�д������飡");
			}
			saRet = this.ConvertRngArray(rng);
			if((saRet[1,1] != null) && (saRet[1,2] != null))
			{
				strStart = saRet[1,1].ToString();
				strEnd = saRet[1,2].ToString();
			}
			else
			{	
				throw new Exception("��ȡExcel�ļ��������飡");
			}
			#endregion

			#region ��ȡ����
			try
			{
				// ��ȡ����
				rng = ws.get_Range(strStart,strEnd);
				AData = this.ConvertRngArray(rng);
			}
			catch
			{
				throw new Exception("��ȡExcel�ļ��������飡");
			}

			//Determine the dimensions of the array. 
			int iRows; 
			int iCols; 
			iRows = AData.GetUpperBound(0); 
			iCols = AData.GetUpperBound(1); 
			// Checking
			if(!CellCheck(iRows,iCols))
			{
				throw new Exception("�������������ݶ�ȡ�������飡");
			}
			try
			{
				for (i = 1; i <= iRows; i++)    
				{ 
					dr = dt.NewRow(); 
					for(j = 0; j <= iCols - 1; j++)
					{
						if(AData[i,j + 1] != null)
						{
							dr[j] = AData[i,j + 1];
						}
						else
						{
							dr[j] = "";
						}
					}
					dt.Rows.Add(dr); 
				} 
			}
			catch
			{
				throw new Exception("��ȡExcel�ļ��������飡");
			}
			dsExcel.Tables.Add(dt); 
			#endregion
			return dsExcel; 
		}
		/// <summary>
		/// �������ƣ� ReadCell
		/// ���������� ��ȡĳ��Ԫ������ݣ�ע�����뵥Ԫ��ĺϷ���
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-15 15:52:00
		/// </summary>
		/// <param name="strCell"></param>
		/// <returns></returns>
		public string ReadCell(string strCell)
		{
			// �ж�������Ϸ�
			Excel.Range rng ;
			string strValue = "";
			// Checking
			if(!CellCheck(strCell))
			{
				throw new Exception("��ȡ��Ԫ���ʶ�������飡");
			}
			try
			{
				rng = ws.get_Range(strCell,Missing.Value);
				if(rng != null)
				{
					strValue = rng.Value2.ToString();
				}
			}
			catch
			{
				throw new Exception("��ȡ��ǰ�д������飡");
			}
			return strValue;
		}

		/// <summary>
		/// �������ƣ� ReadCell
		/// ���������� ��ȡĳ��Ԫ�����ݣ��������в�����ȡ
		/// ʵ�����̣� .
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-19 9:10:15
		/// </summary>
		/// <param name="iRow"></param>
		/// <param name="iCol"></param>
		/// <returns></returns>
		public string ReadCell(int iRow,int iCol)
		{
			Excel.Range rng ;
			string strValue = "";
			app.ActiveCell.SpecialCells(Excel.XlCellType.xlCellTypeLastCell,Type.Missing).Select();				
			int irowcount = 0;
			int icolcount = 0;
			irowcount = app.ActiveCell.Row;
			icolcount = app.ActiveCell.Column;
			//if(!CellCheck(iRow,iCol))
			if((iRow > irowcount) || (iCol > icolcount))
			{
				throw new Exception("��ȡ��Ԫ�񳬳���Χ�����飡");
			}
			try
			{
				rng = ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]);
				if(rng != null)
				{
					strValue = rng.Value2.ToString();
				}
			}
			catch
			{
				throw new Exception("��ȡ��ǰ�д������飡");
			}
			return strValue;
		}

		/// <summary>
		/// �������ƣ� LoadTemplate
		/// ���������� ����Xml�ܹ��ĵ�
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-15 15:52:32
		/// </summary>
		/// <returns></returns>
		private DataSet LoadTemplate()
		{
			string strpath = this.FilePath;
			string strFile;
			DataSet dsxml = new DataSet();
			strFile = strpath.Remove(strpath.Length - 4,4);
			this.XMLFilePath = strFile + ".xml";
			// �ж϶�ӦXML�ܹ��ļ��Ƿ����
			if(!System.IO.File.Exists(this.XMLFilePath))
			{
				throw new Exception("Xml�ܹ��ļ�������");
			}
			try
			{
				dsxml.ReadXml(this.XMLFilePath);
			}
			catch
			{
				throw new Exception("����Xml�ܹ��ļ��������飡");
			}

			return dsxml;
		}
		
		/// <summary>
		/// �������ƣ� LoadTemplate
		/// ���������� ����Xml�ܹ��ĵ������ڼ�����Excel��ͬ����Xml�ܹ��ĵ�
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-15 15:52:32
		/// </summary>
		/// <returns></returns>
		private DataSet LoadTemplate(string sXmlFile)
		{
			DataSet dsxml = new DataSet();

			try
			{
				dsxml.ReadXml(sXmlFile);
			}
			catch
			{
				throw new Exception("����Xml�ܹ��ļ��������飡");
			}
			return dsxml;
		}

		/// <summary>
		/// �������ƣ� AddColName
		/// ���������� ����XML�ܹ��ļ���������
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-12 8:51:40
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="arrxml"></param>
		private void AddColName(ref System.Data.DataTable dt,DataSet dsxmlSch)
		{
			if((dsxmlSch == null) || (dsxmlSch.Tables[0].Rows.Count == 0))
			{
				throw new Exception("Xml�ܹ��ļ�Ϊ�գ����飡");	
			}
			for(int i = 0 ; i<= dsxmlSch.Tables[0].Columns.Count - 1;i++)
			{
				dt.Columns.Add(new DataColumn(dsxmlSch.Tables[0].Columns[i].ColumnName.ToString(), typeof(string))); 
			}
		}

		/// <summary>
		/// �������ƣ� 
		/// ���������� ConvertRngArray
		/// ʵ�����̣� ��Excel���ݼ�ת��Ϊ����
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-11 17:45:12
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		private Object[,] ConvertRngArray(Excel.Range range)
		{
			//Retrieve the data from the range. 
			Object[,] saRet; 
			saRet = (System.Object[,])range.get_Value( Missing.Value ); 
			return saRet;
		}

		/// <summary>
		/// �������ƣ� Write
		/// ���������� д�����ݣ����������������ݼ���
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-19 10:26:31
		/// </summary>
		/// <returns></returns>
		public bool Write()
		{
			System.Data.DataTable dt = null;
			Excel.Range m_objRange ;
			bool bolRet = false;
			string strCell = "A1";
			dt = this.DataSource.Tables[0];			
			Object[,] AData ;
			int irowcount = 0;
			int iSheetCount = wb.Sheets.Count ;
			app.ActiveCell.SpecialCells(Excel.XlCellType.xlCellTypeLastCell,Type.Missing).Select();
			irowcount = app.ActiveCell.Row;
			// д����
			this.WriteException(dt);
			try
			{
				switch(this.WriteType)
				{
					case EnumType.WriteType.None:
						strCell = "A1";
						AData = new object[dt.Rows.Count,dt.Columns.Count];	
						for(int i = 0;i <=dt.Columns.Count - 1;i++)
						{
							AData[0,i] = dt.Columns[i].ColumnName;
						}
						ConvertArrayDataTable(AData,dt,0,dt.Rows.Count - 1,1);
						//ws = (excel.Worksheet)(wb.Sheets.Add(objOpt,objOpt,objOpt,objOpt)) ;
						// �ı�Sheet������
						if(this.SheetName != "")
						{
							ws.Name = this.SheetName;;
						}
						m_objRange = ws.get_Range(strCell, Type.Missing);
						m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count);
						m_objRange.Value2 = AData;

						#region // ɾ��Ĭ��Sheet��
						//ws.Activate();
						//ws.Move(wb.Sheets[1],objOpt);
//						while(iSheetCount > 0)
//						{
//							Excel._Worksheet tempXSheet = (Excel._Worksheet) (wb.Worksheets[iSheetCount]) ;
//							tempXSheet.Delete() ;
//							System.Runtime.InteropServices.Marshal.ReleaseComObject(tempXSheet) ;
//							tempXSheet=null ;
//							iSheetCount--;
//						}
						#endregion

						break;
					case EnumType.WriteType.Insert:
					{
//						strCell = "A1";
//						AData = new object[dt.Rows.Count,dt.Columns.Count];	
//						for(int i = 0;i <=dt.Columns.Count - 1;i++)
//						{
//							AData[0,i] = dt.Columns[i].ColumnName;
//						}
//						ConvertArrayDataTable(AData,dt,0,dt.Rows.Count - 1,1);
//						// �ı�Sheet������
//						if(this.SheetName != "")
//						{
//							ws.Name = this.SheetName;;
//						}
//						m_objRange = ws.get_Range(strCell, Type.Missing);
//						//m_objRange.EntireRow.Insert();
//						m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count).EntireRow;
//						m_objRange.Insert();
//						//m_objRange.Value2 = AData;

						break;
					}
					case EnumType.WriteType.Append:
						string strSheetName ;
						if(this.SheetName != "")
						{
							ws.Name = this.SheetName;;
						}
						strSheetName = ws.Name.ToString();
						strCell = "A" + Convert.ToString(irowcount + 1);
						if((dt.Rows.Count + irowcount) <= iMaxRow)
						{
							AData = new object[dt.Rows.Count,dt.Columns.Count];	
							ConvertArrayDataTable(AData,dt,0,dt.Rows.Count - 1,0);
							m_objRange = ws.get_Range(strCell, Type.Missing);
							m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count);
							m_objRange.Value2 = AData;
							//m_objRange.EntireRow.Insert();
						}
						else
						{
							int iFristInsertRow,iSeceondInRow;
							iFristInsertRow = iMaxRow - irowcount ;
							AData = new object[iFristInsertRow,dt.Columns.Count];	
							ConvertArrayDataTable(AData,dt,0,iFristInsertRow,0);
							m_objRange = ws.get_Range(strCell, Type.Missing);
							m_objRange = m_objRange.get_Resize(iFristInsertRow,dt.Columns.Count);
							m_objRange.Value2 = AData;
					
							Excel._Worksheet xSheet = null ;
							xSheet = (Excel._Worksheet)(wb.Sheets.Add(objOpt,objOpt,objOpt,objOpt));
							xSheet.Name = strSheetName + "(2)";
							iSeceondInRow = dt.Rows.Count - iFristInsertRow ;
							AData = new object[iSeceondInRow,dt.Columns.Count];	
							ConvertArrayDataTable(AData,dt,0,iSeceondInRow,0);
							m_objRange = xSheet.get_Range("A1", Type.Missing);
							m_objRange = m_objRange.get_Resize(iSeceondInRow,dt.Columns.Count);
							m_objRange.Value2 = AData;
							xSheet.Activate();
							xSheet.Move(objOpt,wb.Sheets[ws.Index]);
						}
						break;
					default:
						strCell = "A1";
						break;
				}
				this.Save();
				bolRet = true;
			}
			catch
			{
				throw new Exception("д��Excel�ļ��������飡");
			}
			return bolRet;
		}
		/// <summary>
		/// �������ƣ� ReWrite
		/// ���������� ��д����
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-25 11:22:30
		/// </summary>
		/// <param name="iRow">��д��ʼ����</param>
		/// <returns></returns>
		public bool ReWrite(int iRow)
		{
			System.Data.DataTable dt = null;
			Excel.Range m_objRange ;
			bool bolRet = false;
			string strCell = "A1";
			dt = this.DataSource.Tables[0];			
			Object[,] AData ;
			int iSheetCount = wb.Sheets.Count ;
			if(!CellCheck(iRow,1))
			{
				bolRet = false;
				throw new Exception("д�뵥Ԫ�񳬳���Χ�����飡");
			}
			// д����
			this.WriteException(dt);
			
			string strSheetName ;
			if(this.SheetName != "")
			{
				ws.Name = this.SheetName;;
			}
			strSheetName = ws.Name.ToString();
			strCell = "A" + Convert.ToString(iRow);
			try
			{
				// ����Ĳ���
//				if(this.WriteType == EnumType.WriteType.Insert)
//				{
//					AData = new object[dt.Rows.Count,dt.Columns.Count];	
//					ConvertArrayDataTable(AData,dt,0,dt.Rows.Count - 1,0);
//
//					m_objRange = ws.get_Range(strCell, Type.Missing);
//					//m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count);
//					//m_objRange.EntireRow.Select();
//					for(int i = 0 ; i <= dt.Rows.Count ; i++)
//					{
//						m_objRange.EntireRow.Insert();
//					}
//					//m_objRange.Value2 = AData;
//				}
//				else
//				{
				// ��д����
				if((dt.Rows.Count + iRow) <= iMaxRow)
				{
					AData = new object[dt.Rows.Count,dt.Columns.Count];	
					ConvertArrayDataTable(AData,dt,0,dt.Rows.Count - 1,0);
					m_objRange = ws.get_Range(strCell, Type.Missing);
					m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count);
					m_objRange.Value2 = AData;
				}
				else
				{
					int iFristInsertRow,iSeceondInRow;
					iFristInsertRow = iMaxRow - iRow ;
					AData = new object[iFristInsertRow,dt.Columns.Count];	
					ConvertArrayDataTable(AData,dt,0,iFristInsertRow,0);
					m_objRange = ws.get_Range(strCell, Type.Missing);
					m_objRange = m_objRange.get_Resize(iFristInsertRow,dt.Columns.Count);
					m_objRange.Value2 = AData;
				
					Excel._Worksheet xSheet = null ;
					xSheet = (Excel._Worksheet)(wb.Sheets.Add(objOpt,objOpt,objOpt,objOpt));
					xSheet.Name = strSheetName + "(2)";
					iSeceondInRow = dt.Rows.Count - iFristInsertRow ;
					AData = new object[iSeceondInRow,dt.Columns.Count];	
					ConvertArrayDataTable(AData,dt,0,iSeceondInRow,0);
					m_objRange = xSheet.get_Range("A1", Type.Missing);
					m_objRange = m_objRange.get_Resize(iSeceondInRow,dt.Columns.Count);
					m_objRange.Value2 = AData;
					xSheet.Activate();
					xSheet.Move(objOpt,wb.Sheets[ws.Index]);
				}
				//}
				this.Save();
				bolRet = true;
			}
			catch
			{
				throw new Exception("д��Excel�ļ��������飡");
			}
			return bolRet;
		}
		/// <summary>
		/// д����
		/// </summary>
		/// <param name="dt"></param>
		private void WriteException(System.Data.DataTable dt)
		{
			if((dt == null) || (dt.Rows.Count > iMaxRow))
			{
				throw new Exception("��ǰ����Դ���ݲ����ڻ��������������飡");
			}
			if((this.WriteType != EnumType.WriteType.None) && ((((Excel.Sheets)(((Excel.ApplicationClass)(app)).Worksheets)).Count > iMaxSheet - 1) 
				&& (dt.Rows.Count != 0)))
			{
				throw new Exception("��ǰ�ļ��Ѵﵽ�洢���ޣ����飡");
			}
		}
		/// <summary>
		/// ����ת��ΪDataTable
		/// </summary>
		private void ConvertArrayDataTable(Object[,] AData,System.Data.DataTable dt,int iStartRow,int iEndRow,int iArrayStartRow)
		{
			for(int i = iStartRow;i < iEndRow;i++)
			{
				for(int j = 0;j <= dt.Columns.Count -1 ;j++)
				{
					string strValue ;
					strValue = dt.Rows[i][j].ToString().Trim();
					if(dt.Rows[i][j].GetType() == System.Type.GetType("System.DateTime"))
					{
						AData[iArrayStartRow + i,j] = (Convert.ToDateTime(strValue)).ToString("yyyy-MM-dd HH:MM");
					}
					else if(dt.Rows[i][j].GetType() == System.Type.GetType("System.String"))
					{
						AData[iArrayStartRow + i,j] = strValue;	
					}
					else
					{
						AData[iArrayStartRow + i,j] = strValue;
					}
				}
			}
		}
		/// <summary>
		/// ��дExcelʱCell�Ϸ��Լ�飬��Ԫ��ʽ
		/// </summary>
		/// <param name="strCell"></param>
		/// <returns></returns>
		private bool CellCheck(string strCell)
		{
			string str = strCell; //"AABB12";
			Regex r = new Regex(@"\A[A-Z]+[0-9]+\z" );
			if(r.IsMatch(str))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// ��дExcelʱCell�Ϸ��Լ�飬�У��з�ʽ
		/// </summary>
		/// <param name="iRow"></param>
		/// <param name="iCol"></param>
		/// <returns></returns>
		private bool CellCheck(int iRow,int iCol)
		{
            if((iRow > 0) && (iRow <= iMaxRow) && (iCol >0) && (iCol <= 255))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// �������ƣ� WriteCell
		/// ���������� д�����ݵ�ĳ��Ԫ��
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-19 10:28:58
		/// </summary>
		/// <param name="iRow"></param>
		/// <param name="iCol"></param>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public bool WriteCell(int iRow,int iCol,string strValue)
		{
			bool bolRet = false;
			if(!CellCheck(iRow,iCol))
			{
				bolRet = false;
				throw new Exception("д�뵥Ԫ�񳬳���Χ�����飡");
			}
			if(strValue.Length > 255)
			{
				bolRet = false;
				throw new Exception("��Ԫ��ֵ���ȳ���255���ַ������飡");	
			}
			try
			{
				if((strValue != null) && (strValue != ""))
				{
					ws.Cells[iRow,iCol] = strValue;
				}
				ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Interior.Color = System.Drawing.ColorTranslator.ToOle(this.BackColor);
				ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(this.ForeColor);

				if(this.Font != null)
				{
					ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Font.Bold = this.Font.Bold;
					ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Font.Italic = this.Font.Italic ;
					if(this.Font.Name != "")
					{
						ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Font.Name = this.Font.Name;
					}
					ws.get_Range(ws.Cells[iRow,iCol],ws.Cells[iRow,iCol]).Font.Size = this.Font.Size ;
				}

				wb.Save();
				bolRet = true;
				
			}
			catch
			{
				bolRet = false;
				throw new Exception("д�뵥Ԫ��������飡");
			}
			return bolRet;
		}
		/// <summary>
		/// �������ƣ� OpenCreate
		/// ���������� д�ļ�ʱ�������ļ���������
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-19 10:29:41
		/// </summary>
		/// <returns></returns>
		public bool OpenCreate()
		{
			string strPath = "";
			bool bolRet = false;
			strPath = this.FilePath;
			try
			{
				this.FileExteCheck(strPath);
				//����ļ������ڣ��򴴽��ļ�
				if(!File.Exists(strPath))
				{
                    las.foundation.SmartExcel.SmartExcel excelObj = new las.foundation.SmartExcel.SmartExcel();
					excelObj.CreateFile(strPath);
					excelObj.PrintGridLines = false;
					excelObj.CloseFile();
				}
				bolRet = this.Open();
			}
			catch
			{
				bolRet = false;
				throw new Exception("����Excel���ִ������飡");			
			}
			return bolRet;
		}

		/// <summary>
		/// ���ΪExcel�ļ�
		/// </summary>
		/// <param name="savePath">����·��</param>
		public void SaveAsExcelFile(string savePath)
		{
			//wb.Save();
			wb.SaveAs(savePath,excel.XlFileFormat.xlHtml,Type.Missing,Type.Missing,Type.Missing,Type.Missing,excel.XlSaveAsAccessMode.xlExclusive,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
		}
		/// <summary>
		/// �洢
		/// </summary>
		public void Save()
		{
			wb.Save();
		}
	
		/// <summary>
		/// ��Դ�ͷ�
		/// </summary>
		public void Close()
		{
			this.Dispose();
			this.KillExcelProcess();
		}

		/// <summary>
		/// �������ƣ� KillExcelProcess
		/// ���������� ��Process��������Excel����
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-11 9:25:30
		/// </summary>
		public void KillExcelProcess()
		{
			Process[] myProcesses;
			DateTime startTime;
			myProcesses = Process.GetProcessesByName("Excel");

			//�ò���Excel����ID����ʱֻ���жϽ�������ʱ��
			foreach(Process myProcess in myProcesses)
			{
				startTime = myProcess.StartTime;

				if(startTime > beforeTime && startTime < afterTime)
				{
					myProcess.Kill();
				}
			}
		}

		/// <summary>
		/// �������ƣ� Dispose
		/// ���������� �����Excel�Ĳ���û�������쳣�Ļ������������������������Excel����
		/// ����Ҫ��KillExcelProcess()����������Excel����
		/// ʵ�����̣� 
		/// ��    �ߣ� �ָ���
		/// ��    �ڣ� 2006-5-11 9:24:46
		/// </summary>
		public void Dispose()
		{
			//ע�⣺�����õ�������Excel����Ҫִ����������������������Excel����
			if(wb != null)
			{
				wb.Close(null,null,null);
				app.Workbooks.Close();
				app.Quit();
			}
			if(rng != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(rng);
				rng = null;
			}
			if(tb != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(tb);
				tb = null;
			}
			if(ws != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
				ws = null;
			}
			if(wb != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
				wb = null;
			}
			if(app != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
				app = null;
			}

			GC.Collect();
		}
		#endregion
	}
}