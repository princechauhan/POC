using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Model
{
  public  class TopTableInfo
    {
        public List<TopTableData> TopTableData { get; set; }
    }
    /// <summary>
    /// top table item properties 
    /// </summary>
    public class TopTableData
    {
        public string Number { get; set; }
      
    }
}
