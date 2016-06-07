using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using POC.Model;
using System.IO;
using Windows.ApplicationModel;
using POC.Constant;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using System.Windows.Input;
using POC.Common_Classes;


namespace POC.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
       
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            
            // create a decrement quantity Command instance
            this.DecrementQuantityCommand = new DelegateCommand(ExecuteDecrementQuantityCommand);
            // create a increment quantity Command instance
            this.IncrementQuantityCommand = new DelegateCommand(ExecuteIncrementQuantityCommand);
            //create a TopTableList instance
            TopTableList = new ObservableCollection<TopTableData>();
            //create a ItemDetailList instance
            ItemDetailList = new ObservableCollection<ItemData>();
            //create a HotItemList instance
            HotItemList = new ObservableCollection<ItemData>();
            //calling function to fetch top table data
            GetTopTableList();
            //calling function to fetch hot item data list
            GetItemList();
        }


        #region Properties

        /// <summary>
        ///  member variable for Total amount property 
        /// </summary>
        private double totalAmount;
        /// <summary>
        /// This property will be contain total amount of all items after deduction discount
        /// </summary>
        public double TotalAmount
        {
            get { return totalAmount; }
            set
            {
                this.Set(ref totalAmount, value, broadcast: true);
            }
        }

        /// <summary>
        ///  This property will be bound to button's Command property for increment quantity of an item
        /// </summary>
        public ICommand IncrementQuantityCommand { get; set; }

        /// <summary>
        ///  This property will be bound to button's Command property for decrement quantity of an item
        /// </summary>
        public ICommand DecrementQuantityCommand { get; set; }

        /// <summary>
        /// member variable for top table list
        /// </summary>
        private ObservableCollection<TopTableData> topTableList;
        /// <summary>
        /// ObservableCollection for top table list
        /// </summary>
        /// /// <remarks>
        /// after fetching data from database file, this list contain all items related to the top table 
        /// </remarks>
        public ObservableCollection<TopTableData> TopTableList
        {
            get { return topTableList; }
            set
            {
                this.Set(ref topTableList, value, broadcast: true);
            }
        }


        /// <summary>
        /// member variable for item detail list
        /// </summary>
        private ObservableCollection<ItemData> itemDetailList;
        /// <summary>
        /// ObservableCollection for item detail list
        /// </summary>
        /// /// <remarks>
        /// after fetching data from database file, this list contain all items detail 
        /// </remarks>
        public ObservableCollection<ItemData> ItemDetailList
        {
            get { return itemDetailList; }
            set
            {
                this.Set(ref itemDetailList, value, broadcast: true);
            }
        }


        /// <summary>
        /// member variable for item list
        /// </summary>
        private ObservableCollection<ItemData> hotItemList;
        /// <summary>
        /// ObservableCollection for item list
        /// </summary>
        /// /// <remarks>
        /// after fetching data from database file, this list contain all items
        /// </remarks>
        public ObservableCollection<ItemData> HotItemList
        {
            get { return hotItemList; }
            set
            {
                this.Set(ref hotItemList, value, broadcast: true);
            }
        }

        #endregion

        #region Commands


        /// <summary>
        /// button click event, occurs when the user click on quantity button, and it is use for increment quantity 
        /// </summary>
        void ExecuteIncrementQuantityCommand(object param)
        {
            var item = param as ItemData;
            if(item!=null)
            {
                item.Qty = item.Qty + item.BaseQuantity;
                //calling function to calculate amount 
                CalculateAmount(item);
            }
        }
        
        /// <summary>
        /// button click event, occurs when the user click on minus button, and it is use for decrement quantity 
        /// </summary>
        void ExecuteDecrementQuantityCommand(object param)
        {
            var item = param as ItemData;
            if (item != null)
            {
                item.Qty = item.Qty - 1;
                //calling function to calculate amount 
                CalculateAmount(item);
            }
        }

        /// <summary>
        /// member variable for hot item click command
        /// </summary>
        private RelayCommand<ItemData> hotItemClickCommand;
        /// <summary>
        /// itemclick event, occurs when the user click any desire item from a hot item list
        /// </summary>
        public RelayCommand<ItemData> HotItemClickCommand
        {
            get
            {
                if (hotItemClickCommand == null)
                {
                    hotItemClickCommand = new RelayCommand<ItemData>(
                        (item) =>
                        {
                            foreach (var hotItem in HotItemList)
                            {
                                hotItem.IsQuantityButtonVisible = Visibility.Collapsed;
                                hotItem.IsMinusButtonVisible = Visibility.Collapsed;
                            }
                            if (item != null && item.Code != "0")
                            {                              
                                if (ItemDetailList.Count!=0)
                                {
                                    foreach (var selectedItem in ItemDetailList)
                                    {
                                        if (selectedItem.Code == item.Code)
                                        {
                                            ItemDetailList.Remove(item);
                                            break;
                                        }
                                    }
                                    ItemDetailList.Add(item);
                                }
                                else
                                {
                                    ItemDetailList.Add(item);
                                }
                                
                                //calling function to calculate amount 
                                CalculateAmount(item);                                                                                       
                                item.IsQuantityButtonVisible = Visibility.Visible;
                                item.IsMinusButtonVisible = Visibility.Visible;
                            }
                            
                        });
                }

                return hotItemClickCommand;
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// method for calculating amount for selected item base on quantity + discount 
        /// </summary>
        private void CalculateAmount(ItemData item)
        {
            TotalAmount = 0;
            if (item.Disc > 0.0)
            {
                item.Amount = Convert.ToDouble(item.Qty * item.Price) - Convert.ToDouble((item.Qty * item.Price * item.Disc) / 100);
            }
            else
            {
                item.Amount = Convert.ToDouble(item.Qty * item.Price);
            }
            foreach(var item1 in ItemDetailList)
            {
                TotalAmount = item1.Amount + TotalAmount;
            }
        }

        /// <summary>
        /// method for fetching hot items list from database file 
        /// </summary>
        private async void GetItemList()
        {
            try
            {
                
                string databaseFile = ConstantValue.DataBaseFileName;
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile localFile = await InstallationFolder.GetFileAsync(databaseFile);
                using (StreamReader file = File.OpenText(localFile.Path))
                {
                    var json = file.ReadToEnd();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(json);
                    if (result.ItemData != null)
                    {
                        HotItemList.Clear();
                        foreach (var item in result.ItemData)
                        {
                            item.BaseQuantity = item.Qty;
                            HotItemList.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        /// <summary>
        /// method for fetching top table list from database file 
        /// </summary>
        private async void GetTopTableList()
        {
            try
            {
                string databaseFile = ConstantValue.DataBaseTopTableFileName;
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile localFile = await InstallationFolder.GetFileAsync(databaseFile);
                using (StreamReader file = File.OpenText(localFile.Path))
                {
                    var json = file.ReadToEnd();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TopTableInfo>(json);
                    if (result.TopTableData != null)
                    {
                        TopTableList.Clear();
                        foreach (var item in result.TopTableData)
                        {
                            TopTableList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion
    }
}
