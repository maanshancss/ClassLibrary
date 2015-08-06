//============================================================================
//Gios Pdf.NET - A library for exporting Pdf Documents in C#
//Copyright (C) 2005  Paolo Gios - www.paologios.com
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================
using System;
using System.Data;
using System.Collections;
using System.Drawing;

namespace Gios.Pdf
{
	/// <summary>
	/// 
	/// </summary>
	public class PdfTablePage : PdfObject
	{
		internal Hashtable cellAreas;
		internal int startRow,endRow,columns;
		internal string stream;
		internal PdfArea area;
		/// <summary>
		/// 
		/// </summary>
		public PdfArea Area
		{
			get
			{
				return area;
			}
		}
		internal PdfTablePage(int startRow,int endRow,int columns)
		{
			this.cellAreas=new Hashtable();
			this.startRow=startRow;
			this.endRow=endRow;
			this.columns=columns;
		}
		/// <summary>
		/// returns the Area where the Cell has been rasterized.
		/// </summary>
		/// <param name="Row"></param>
		/// <param name="Column"></param>
		/// <returns></returns>
		public PdfArea CellArea(int Row,int Column)
		{
			object o=this.cellAreas[Row+","+Column];
			if (o==null) throw new Exception("Cell ["+Row+","+Column+"] does not belong to the TablePage.");
			return o as PdfArea;
		}
		internal void SetArea()
		{
			
			this.area=this.CellArea(startRow,0).Merge(this.CellArea(endRow,0));
			double great=0;
			foreach (PdfArea pa in this.cellAreas.Values)
			{
				double d=pa.BottomRightCornerX;
				if(d>great) great=d;
			}
			this.area.BottomRightCornerX=great;
			
		}
		/// <summary>
		/// the index of the first Row of the rendered Table Page.
		/// </summary>
		public int FirstRow
		{
			get
			{
				return this.startRow;
			}
		}
		/// <summary>
		/// the index of the last Row of the rendered Table Page.
		/// </summary>
		public int LastRow
		{
			get
			{
				return this.endRow;
			}
		}
		/// <summary>
		/// returns true if the Table Contains the rendering of this Row Index.
		/// </summary>
		/// <param name="RowIndex"></param>
		/// <returns></returns>
		public bool ContainsRow(int RowIndex)
		{
			if ((this.startRow<=RowIndex)&&(RowIndex<=this.endRow)) return true;
			return false;
		}
		internal override int StreamWrite(System.IO.Stream stream)
		{
			int num=this.id;

			string text=this.stream;
			Byte[] part2;
				
			if (PdfDocument.FlateCompression) part2=Utility.Deflate(text); else
				part2=System.Text.ASCIIEncoding.ASCII.GetBytes(text);

			string s1="";
			s1+=num.ToString()+" 0 obj\n";
			s1+="<< /Lenght "+part2.Length;
			if (PdfDocument.FlateCompression) s1+=" /Filter /FlateDecode";
			s1+=">>\n";
				
			s1+="stream\n";
				
			string s3="\nendstream\n";
			s3+="endobj\n";

			Byte[] part1=System.Text.ASCIIEncoding.ASCII.GetBytes(s1);
			Byte[] part3=System.Text.ASCIIEncoding.ASCII.GetBytes(s3);
			stream.Write(part1,0,part1.Length);
			stream.Write(part2,0,part2.Length);
			stream.Write(part3,0,part3.Length);
			return part1.Length+part2.Length+part3.Length;
		}

	}
}
