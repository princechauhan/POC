using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Model
{
  
  public  class Info
    {
        public List<ItemData> ItemData { get; set; }
    }
    /// <summary>
    /// item details properties 
    /// </summary>
    public class ItemData
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Qty { get; set; }
        public string Price { get; set; }
        public string Disc { get; set; }
        public string Amount { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }

}
