using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace POC.Model
{
  
  public  class Info
    {
        public List<ItemData> ItemData { get; set; }
    }
    /// <summary>
    /// item details properties 
    /// </summary>
    public class ItemData : ViewModelBase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int BaseQuantity { get; set; }
        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                this.Set(ref price, value, broadcast: true);
            }
        }
      
        private double disc;
        public double Disc
        {
            get { return disc; }
            set
            {
                this.Set(ref disc, value, broadcast: true);
            }
        }
       
        private double amount;
        public double Amount
        {
            get { return amount; }
            set
            {
                this.Set(ref amount, value, broadcast: true);
            }
        }
       
        private Visibility isQuantityButtonVisible=Visibility.Collapsed;      
        public Visibility IsQuantityButtonVisible
        {
            get { return isQuantityButtonVisible; }
            set
            {
                this.Set(ref isQuantityButtonVisible, value, broadcast: true);
            }
        }
        private Visibility isMinusButtonVisible=Visibility.Collapsed;
        public Visibility IsMinusButtonVisible
        {
            get { return isMinusButtonVisible; }
            set
            {
                this.Set(ref isMinusButtonVisible, value, broadcast: true);
            }
        }
        private int qty;
        public int Qty
        {
            get { return qty; }
            set
            {
                this.Set(ref qty, value, broadcast: true);
            }
        }
    }

}
