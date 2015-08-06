using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    [Serializable]
    public class RemotingServiceAddress
    {
        #region Protocal
        private string protocal = "tcp";
        public string Protocal
        {
            get { return protocal; }
            set { protocal = value; }
        } 
        #endregion

        #region IP
        private string iP = "";
        public string IP
        {
            get { return iP; }
            set { iP = value; }
        }
        #endregion

        #region Port
        private int port = 9000;
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        #endregion         

        #region ServiceName
        private string serviceName;
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        } 
        #endregion

        #region ServiceUrl get
        public string ServiceUrl
        {
            get
            {
                return string.Format("{0}://{1}:{2}/{3}",this.protocal, this.iP, this.port, this.serviceName);
            }
        } 
        #endregion

        public override string ToString()
        {
            return this.ServiceUrl;
        }
    }
}
