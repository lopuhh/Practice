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
    /// Логика взаимодействия для EditNewAbonentTypeWindow.xaml
    /// </summary>
    public partial class EditNewAbonentTypeWindow : Window
    {
        public EditNewAbonentTypeWindow(AbonentType abonentTypeToEdit)
        {
            InitializeComponent();
            DataContext = new DataManageVM();
            DataManageVM.SelectedAbonentType = abonentTypeToEdit;
            DataManageVM.AbonentTypeName = abonentTypeToEdit.Name;
            DataManageVM.AbonentTypeMobile = abonentTypeToEdit.Mobile;
        }

        //Метод запрета ввода всего кроме цифр
        private void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
