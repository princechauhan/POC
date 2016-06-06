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
            TopTableList = new ObservableCollection<TopTableData>();
            ItemDetailList = new ObservableCollection<string>();
            HotItemList = new ObservableCollection<ItemData>();

            GetTopTableList();
            GetItemList();

            ItemDetailList.Add("dsdsd");
            ItemDetailList.Add("dsdsd");
           
        }


        #region Properties

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
        private ObservableCollection<string> itemDetailList;
        /// <summary>
        /// ObservableCollection for item detail list
        /// </summary>
        /// /// <remarks>
        /// after fetching data from database file, this list contain all items detail 
        /// </remarks>
        public ObservableCollection<string> ItemDetailList
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
                             
                        });
                }

                return hotItemClickCommand;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// method for fetching items list from database file 
        /// </summary>
        private async void GetItemList()
        {
            try
            {
                
                string CountriesFile = @"Assets\ItemsData.Json";
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile localFile = await InstallationFolder.GetFileAsync(CountriesFile);
                using (StreamReader file = File.OpenText(localFile.Path))
                {
                    var json = file.ReadToEnd();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(json);
                    if (result.ItemData != null)
                    {
                        HotItemList.Clear();
                        foreach (var item in result.ItemData)
                        {
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
                string CountriesFile = @"Assets\TopTableData.Json";
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile localFile = await InstallationFolder.GetFileAsync(CountriesFile);
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
