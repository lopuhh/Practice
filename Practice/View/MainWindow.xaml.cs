using Practice.Models;
using Practice.ViewModel;
using Spire.Xls;
using System.Windows;
using System.Windows.Controls;


namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListView AllAbonentTypesView;
        public static ListView AllAbonentsView;
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataManageVM();
            AllAbonentTypesView = ViewAllAbonentTypes;
            AllAbonentsView = ViewAllAbonents;
            

        }
    }
}