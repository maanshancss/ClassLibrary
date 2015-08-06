/* -----------------------------------------------------------------------
 *    版权所有： 版权所有(C) 2006，EYE
 *    文件编号： 0001
 *    文件名称： Excel.cs
 *    系统编号： E-Eye_0001
 *    系统名称： SFWL管理系统
 *    模块编号： 0001
 *    模块名称： Excel数据导入导出
 *    设计文档： 
 *    完成日期： 2006-5-11 9:33:56
 *    作　　者： 林付国
 *    内容摘要： 完成Excel数据文件API读写操作
 *    属性描述： 该接口有10个属性
 *					01 DataSource			数据源
 *					02 Title				Excel文件表格标题
 *					03 FilePath				源文件路径
 *					04 XMLFilePath			XML架构文件路径
 *	  			    05 IsOpen				判断Excel管理对象是否已经打开
 *					06 WriteType			写入类型(普通，重写，追加)
 *					07 SheetName			Sheet表名称
 *					08 BackColor			背景颜色
 *					09 ForeColor			字体颜色
 *					10 Font					字体样式
 *    方法描述： 该接口包括10个方法:
 *					01 Open					打开Excel管理对象
 *					02 Read					读取Excel文件中有效行列数据集
 *					03 ReadCell				读取某单元格的内容
 *					04 ReadCell				读取某单元格的内容，按行，列方式
 *					05 Write				写入至Excel文件中
 *					06 ReWrite				按照指定重写行重写数据
 *					07 WriteCell			写数据至单元格
 *					08 Close				资源释放
 *					09 OpenCreate			打开Excel管理对象，若Excel文件不存在则创建之
 *					10 ActiveSheet			激活当前读写数据的Sheet表
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
	///  类 编 号： 02
	///  类 名 称： CExcel 
	///  内容摘要： 完成Excel管理对象部分操作实例
	///  完成日期： 2006-5-12 14:37:38
	///  编码作者： 林付国
	/// </summary>
	public class CExcelManager : IExcelManager
	{
		#region 成员变量
		/// <summary>
		/// Excel启动之前时间
		/// </summary>
		private DateTime beforeTime;            
		/// <summary>
		/// Excel启动之后时间
		/// </summary>
		private DateTime afterTime;                
		/// <summary>
		/// 数据源
		/// </summary>
		/// <summary>
		private System.Data.DataSet ds = null;
		/// Excel表格标题
		/// </summary>
		private string title = null;
		/// <summary>
		/// 源文件路径
		/// </summary>
		private string inputFilePath = "";
		/// <summary>
		/// XML文件路径
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
		private System.Drawing.Font font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)),false);
		#endregion

		#region Property
		/// <summary>
		/// 数据源
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
		/// Excel文件表格标题
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
		/// 源文件路径
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
		/// XML架构文件路径
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
		/// 判断Excel管理对象是否已经打开
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
		/// Excel文件写入类型
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
		/// Sheet表名称
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
					throw new Exception("Sheet表名不能大于32字符，请检查！");
				}
				else
				{
					this.m_sheetName = value;
				}
			}
		}
		/// <summary>
		/// 背景颜色
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
		/// 字体颜色
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
		/// 字体样式
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
		/// 方法名称： CExcelManager
		/// 内容描述： 实例化Excel对象并使其不可见
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-13 16:30:39
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
				throw new Exception("创建Excel应用对象出现错误，请检查你的机器！");
			}
		}
		/// <summary>
		/// 方法名称： Open
		/// 内容描述： 无
		/// 实现流程： 打开/连接一个excel数据文档
		/// 作    者： 林付国
		/// 日    期： 2006-5-13 16:23:23
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
				throw new Exception("源文件不存在，请检查！");	
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
				throw new Exception("打开或连接Excel文档错误，请检查！");
			}
			return bolRetValue ;
		}
		/// <summary>
		/// 激活Sheet表
		/// </summary>
		public void ActiveSheet(EnumType.SheetIndex sheetindex)
		{
			if(((Excel.Sheets)(((Excel.ApplicationClass)(app)).Worksheets)).Count < (int) sheetindex)
			{
				this.Dispose();
				throw new Exception("Sheet表不存在，请检查！");
			}
			try
			{
				ws = (excel.Worksheet)wb.Worksheets[sheetindex];
				ws.Activate();
			}
			catch
			{
				this.Dispose();
				throw new Exception("Sheet表激活错误，请检查！");
			}
			int irowcount = 0;
			app.ActiveCell.SpecialCells(Excel.XlCellType.xlCellTypeLastCell,Type.Missing).Select();
			irowcount = app.ActiveCell.Row;
			if((this.WriteType != EnumType.WriteType.ReWrite) && (irowcount > iMaxRow))
			{
				this.Dispose();
				throw new Exception("当前Sheet表已达存储上限，不能写入，请检查！");
			}
			int iSheetCount = wb.Sheets.Count ;
			for(int i = 1; i <= iSheetCount ; i++)
			{
				Excel.Worksheet wsT = (Excel.Worksheet) wb.Sheets[i];
				if(((this.m_sheetName != "") && ((this.m_sheetName != null)) && (this.m_sheetName.Trim().ToString() == wsT.Name.Trim().ToString()))
				 && (wsT.Name != ws.Name))
				{
					this.Dispose();
					throw new Exception("当前文件存在同名Sheet表，请检查！");
				}
			}
		}
		/// <summary>
		/// 方法名称： FileExteCheck
		/// 内容描述： Excel文件扩展名检查
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-19 9:09:35
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
				throw new Exception("文件格式不正确！");
			}
		}

		/// <summary>
		/// 方法名称： Read
		/// 内容描述： 读取Excel文件中有效行列数据集
		/// 实现流程： 
		/// 1.根据输入参数起始位置，结束位置确定读取范围
		/// 2.读取数据至DataSet
		/// 作    者： 林付国
		/// 日    期： 2006-5-11 9:20:53
		/// </summary>
		/// <returns>DataSet数据集</returns>
		public System.Data.DataSet Read()
		{
			#region 变量定义
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

			#region 加载Xml架构
			// 可选用不同于Excel文件的Xml文件
			if(this.XMLFilePath.Trim().ToString() == "")
			{
				dsxmlSch = this.LoadTemplate();
			}
			else
			{
				dsxmlSch = this.LoadTemplate(this.XMLFilePath.Trim().ToString());
			}
			// 增加列名
			this.AddColName(ref dt,dsxmlSch);
			#endregion

			#region 读取状态列
			try
			{
				// 读取状态列	rng = (Excel.Range)ws.Cells[0,0];在C#中这种写法不正确
				rng = ws.get_Range("A1","B1");
			}
			catch
			{
				throw new Exception("读取Excel状态列错误，请检查！");
			}
			saRet = this.ConvertRngArray(rng);
			if((saRet[1,1] != null) && (saRet[1,2] != null))
			{
				strStart = saRet[1,1].ToString();
				strEnd = saRet[1,2].ToString();
			}
			else
			{	
				throw new Exception("读取Excel文件错误，请检查！");
			}
			#endregion

			#region 读取数据
			try
			{
				// 读取数据
				rng = ws.get_Range(strStart,strEnd);
				AData = this.ConvertRngArray(rng);
			}
			catch
			{
				throw new Exception("读取Excel文件错误，请检查！");
			}

			//Determine the dimensions of the array. 
			int iRows; 
			int iCols; 
			iRows = AData.GetUpperBound(0); 
			iCols = AData.GetUpperBound(1); 
			// Checking
			if(!CellCheck(iRows,iCols))
			{
				throw new Exception("数据量过大，数据读取错误，请检查！");
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
				throw new Exception("读取Excel文件错误，请检查！");
			}
			dsExcel.Tables.Add(dt); 
			#endregion
			return dsExcel; 
		}
		/// <summary>
		/// 方法名称： ReadCell
		/// 内容描述： 读取某单元格的内容，注意输入单元格的合法性
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-15 15:52:00
		/// </summary>
		/// <param name="strCell"></param>
		/// <returns></returns>
		public string ReadCell(string strCell)
		{
			// 判断输入项不合法
			Excel.Range rng ;
			string strValue = "";
			// Checking
			if(!CellCheck(strCell))
			{
				throw new Exception("读取单元格标识错误，请检查！");
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
				throw new Exception("读取当前列错误，请检查！");
			}
			return strValue;
		}

		/// <summary>
		/// 方法名称： ReadCell
		/// 内容描述： 读取某单元格内容，按照行列参数读取
		/// 实现流程： .
		/// 作    者： 林付国
		/// 日    期： 2006-5-19 9:10:15
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
				throw new Exception("读取单元格超出范围，请检查！");
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
				throw new Exception("读取当前列错误，请检查！");
			}
			return strValue;
		}

		/// <summary>
		/// 方法名称： LoadTemplate
		/// 内容描述： 加载Xml架构文档
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-15 15:52:32
		/// </summary>
		/// <returns></returns>
		private DataSet LoadTemplate()
		{
			string strpath = this.FilePath;
			string strFile;
			DataSet dsxml = new DataSet();
			strFile = strpath.Remove(strpath.Length - 4,4);
			this.XMLFilePath = strFile + ".xml";
			// 判断对应XML架构文件是否存在
			if(!System.IO.File.Exists(this.XMLFilePath))
			{
				throw new Exception("Xml架构文件不存在");
			}
			try
			{
				dsxml.ReadXml(this.XMLFilePath);
			}
			catch
			{
				throw new Exception("加载Xml架构文件错误，请检查！");
			}

			return dsxml;
		}
		
		/// <summary>
		/// 方法名称： LoadTemplate
		/// 内容描述： 加载Xml架构文档，用于加载与Excel不同名的Xml架构文档
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-15 15:52:32
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
				throw new Exception("加载Xml架构文件错误，请检查！");
			}
			return dsxml;
		}

		/// <summary>
		/// 方法名称： AddColName
		/// 内容描述： 根据XML架构文件增加列名
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-12 8:51:40
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="arrxml"></param>
		private void AddColName(ref System.Data.DataTable dt,DataSet dsxmlSch)
		{
			if((dsxmlSch == null) || (dsxmlSch.Tables[0].Rows.Count == 0))
			{
				throw new Exception("Xml架构文件为空，请检查！");	
			}
			for(int i = 0 ; i<= dsxmlSch.Tables[0].Columns.Count - 1;i++)
			{
				dt.Columns.Add(new DataColumn(dsxmlSch.Tables[0].Columns[i].ColumnName.ToString(), typeof(string))); 
			}
		}

		/// <summary>
		/// 方法名称： 
		/// 内容描述： ConvertRngArray
		/// 实现流程： 将Excel数据集转换为数组
		/// 作    者： 林付国
		/// 日    期： 2006-5-11 17:45:12
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
		/// 方法名称： Write
		/// 内容描述： 写入数据，包括（列名，数据集）
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-19 10:26:31
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
			// 写入检查
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
						// 改变Sheet表名称
						if(this.SheetName != "")
						{
							ws.Name = this.SheetName;;
						}
						m_objRange = ws.get_Range(strCell, Type.Missing);
						m_objRange = m_objRange.get_Resize(dt.Rows.Count,dt.Columns.Count);
						m_objRange.Value2 = AData;

						#region // 删除默认Sheet表
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
//						// 改变Sheet表名称
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
				throw new Exception("写入Excel文件出错，请检查！");
			}
			return bolRet;
		}
		/// <summary>
		/// 方法名称： ReWrite
		/// 内容描述： 重写数据
		/// 作    者： 林付国
		/// 日    期： 2006-5-25 11:22:30
		/// </summary>
		/// <param name="iRow">重写开始行数</param>
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
				throw new Exception("写入单元格超出范围，请检查！");
			}
			// 写入检查
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
				// 插入的测试
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
				// 重写部分
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
				throw new Exception("写入Excel文件出错，请检查！");
			}
			return bolRet;
		}
		/// <summary>
		/// 写入检查
		/// </summary>
		/// <param name="dt"></param>
		private void WriteException(System.Data.DataTable dt)
		{
			if((dt == null) || (dt.Rows.Count > iMaxRow))
			{
				throw new Exception("当前数据源数据不存在或数据量过大，请检查！");
			}
			if((this.WriteType != EnumType.WriteType.None) && ((((Excel.Sheets)(((Excel.ApplicationClass)(app)).Worksheets)).Count > iMaxSheet - 1) 
				&& (dt.Rows.Count != 0)))
			{
				throw new Exception("当前文件已达到存储上限，请检查！");
			}
		}
		/// <summary>
		/// 数组转换为DataTable
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
		/// 读写Excel时Cell合法性检查，单元格方式
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
		/// 读写Excel时Cell合法性检查，行，列方式
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
		/// 方法名称： WriteCell
		/// 内容描述： 写入数据到某单元格
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-19 10:28:58
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
				throw new Exception("写入单元格超出范围，请检查！");
			}
			if(strValue.Length > 255)
			{
				bolRet = false;
				throw new Exception("单元格值长度超过255个字符，请检查！");	
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
				throw new Exception("写入单元格错误，请检查！");
			}
			return bolRet;
		}
		/// <summary>
		/// 方法名称： OpenCreate
		/// 内容描述： 写文件时，用于文件创建及打开
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-19 10:29:41
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
				//如果文件不存在，则创建文件
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
				throw new Exception("创建Excel出现错误，请检查！");			
			}
			return bolRet;
		}

		/// <summary>
		/// 另存为Excel文件
		/// </summary>
		/// <param name="savePath">保存路径</param>
		public void SaveAsExcelFile(string savePath)
		{
			//wb.Save();
			wb.SaveAs(savePath,excel.XlFileFormat.xlHtml,Type.Missing,Type.Missing,Type.Missing,Type.Missing,excel.XlSaveAsAccessMode.xlExclusive,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
		}
		/// <summary>
		/// 存储
		/// </summary>
		public void Save()
		{
			wb.Save();
		}
	
		/// <summary>
		/// 资源释放
		/// </summary>
		public void Close()
		{
			this.Dispose();
			this.KillExcelProcess();
		}

		/// <summary>
		/// 方法名称： KillExcelProcess
		/// 内容描述： 用Process方法结束Excel进程
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-11 9:25:30
		/// </summary>
		public void KillExcelProcess()
		{
			Process[] myProcesses;
			DateTime startTime;
			myProcesses = Process.GetProcessesByName("Excel");

			//得不到Excel进程ID，暂时只能判断进程启动时间
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
		/// 方法名称： Dispose
		/// 内容描述： 如果对Excel的操作没有引发异常的话，用这个方法可以正常结束Excel进程
		/// 否则要用KillExcelProcess()方法来结束Excel进程
		/// 实现流程： 
		/// 作    者： 林付国
		/// 日    期： 2006-5-11 9:24:46
		/// </summary>
		public void Dispose()
		{
			//注意：这里用到的所有Excel对象都要执行这个操作，否则结束不了Excel进程
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