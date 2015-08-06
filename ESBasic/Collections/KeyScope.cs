using System;
using System.Collections.Generic ;

namespace ESBasic.Collections
{
	/// <summary>
	/// KeyScope 用于表示一个整数范围。
	/// </summary>	
	public class KeyScope
	{
		#region Ctor
		public KeyScope(){}

		public KeyScope(int theMin ,int theMax)
		{
			this.min = theMin ;
			this.max = theMax ;
		}

		public KeyScope(string scopeStr)
		{
			this.SetScopeStr(scopeStr) ;
		}	

		private void SetScopeStr(string str)
		{
			string[] parts = str.Split(',') ;
			this.min = int.Parse(parts[0].Trim()) ;
			this.max = int.Parse(parts[1].Trim()) ;
		}
		#endregion
		
		#region ScopeString
        /// <summary>
        /// ScopeString 以英文逗号分隔，如"10,1000"
        /// </summary>
		public string ScopeString
		{
			get
			{
				return string.Format("{0},{1}" ,this.min ,this.max) ;
			}
			set
			{
				this.SetScopeStr(value) ;
			}
		}
		#endregion

		#region Max
		private int max = int.MinValue ;
		public  int Max
		{
			get
			{
				return this.max ;
			}
			set
			{
				this.max = value ;
			}
		}
		#endregion

		#region Min
		private int min = int.MaxValue ; 
		public  int Min
		{
			get
			{
				return this.min ;
			}
			set
			{
				this.min = value ;
			}
		}
		#endregion

		#region Contains
		public bool Contains(int val)
		{
			if((this.min <=val) && (val<= this.max))
			{
				return true ;
			}

			return false ;
		}
		#endregion

		#region Intersect
		public bool Intersect(KeyScope scope)
		{
			for(int i=this.min ;i<this.max ;i++)
			{
				if(scope.Contains(i))
				{
					return true ;
				}
			}

			return false ;
		}
		#endregion

        public override string ToString()
        {
            return this.ScopeString;
        }
	}

	
}
