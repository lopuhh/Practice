using Practice.Models;
using Practice.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Practice.View
{
    /// <summary>
    /// Логика взаимодействия для EditNewAbonentWindow.xaml
    /// </summary>
    public partial class EditNewAbonentWindow : Window
    {
        public EditNewAbonentWindow(Abonent abonentToEdit)
        {
            InitializeComponent();
            InitializeComponent();
            DataContext = new DataManageVM();
            DataManageVM.SelectedAbonent = abonentToEdit;
            DataManageVM.AbonentCountry = abonentToEdit.Country;
            DataManageVM.AbonentCity = abonentToEdit.City;
            DataManageVM.AbonentPnumber = abonentToEdit.Pnumber;
            DataManageVM.AbonentDescription = abonentToEdit.Description;
            DataManageVM.AbonentPtype = abonentToEdit.Ptype;
        }

        //Метод запрета ввода всего кроме цифр
        private void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
