using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    /// <summary>
    /// MapItem ”≥…‰œÓ°£
    /// </summary>
    public class MapItem
    {
        #region Ctor
        public MapItem()
        {
        }

        public MapItem(string theSource, string theTarget)
        {
            this.source = theSource;
            this.target = theTarget;
        }
        #endregion

        #region Source
        private string source;
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        #endregion

        #region Target
        private string target;
        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        #endregion
    }
}
