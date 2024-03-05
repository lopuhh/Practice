using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Practice.Models;
using Practice.Models.Report;
using Practice.View;
using Spire.Xls;

namespace Practice.ViewModel
{
    class DataManageVM : INotifyPropertyChanged
    {
        //Получение данных из БД в view
        #region ALL DATA
        private List<Abonent> allAbonents = DataWorker.GetAllAbonents();
        public List<Abonent> AllAbonents
        {
            get { return allAbonents; }
            set
            {
                allAbonents = value;
                NotifyPropertyChanged("AllAbonents");
            }
        }

        private List<AbonentDetail> allAbonentDetails = DataWorker.GetAllAbonentDetails();
        public List<AbonentDetail> AllAbonentDetails
        {
            get { return allAbonentDetails; }
            set
            {
                allAbonentDetails = value;
                NotifyPropertyChanged("AllAbonentDetails");
            }
        }

        private List<AbonentService> allAbonentServices = DataWorker.GetAllAbonentServices();
        public List<AbonentService> AllAbonentServices
        {
            get { return allAbonentServices; }
            set
            {
                allAbonentServices = value;
                NotifyPropertyChanged("AllAbonentServices");
            }
        }

        private List<AbonentType> allAbonentTypes = DataWorker.GetAllAbonentTypes();
        public List<AbonentType> AllAbonentTypes
        {
            get { return allAbonentTypes; }
            set
            {
                allAbonentTypes = value;
                NotifyPropertyChanged("AllAbonentTypes");
            }
        }
        #endregion


        //Обьявление столбцов из БД и обьявление выбранной строки из view 
        #region DECLARATION VARIABLE
        public static string AbonentTypeName { get; set; }
        public static byte AbonentTypeMobile { get; set; }

        public static byte AbonentCountry { get; set; }
        public static byte AbonentCity { get; set; }
        public static int AbonentPnumber { get; set; }
        public static byte AbonentFax { get; set; }
        public static string AbonentDescription { get; set; }
        public static byte  AbonentPtype { get; set; }
        public static byte AbonentSecure { get; set; }

        public TabItem SelectedTabItem { get; set; }
        public static Abonent SelectedAbonent { get; set; }
        public static AbonentType SelectedAbonentType { get; set; }
        #endregion

        //Команда создания отчета
        private RelayCommand reportExcel;
        public RelayCommand ReportExcel
        {
            get
            {
                return reportExcel ?? new RelayCommand(obj =>
                {
                    AllCreateExcelReport();
                    ShowMessageToUser("Отчет создан");
                }
                );
            }
        }
        
        //Команды создания данных в БД
        #region COMMANDS TO ADD
        private RelayCommand addNewAbonentType;
        public RelayCommand AddNewAbonentType
        {
            get
            {
                return addNewAbonentType ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    string resultStr = "";
                    if(AbonentTypeName == null || AbonentTypeName.Replace(" ","").Length == 0)
                    {
                        SetRedBlockControll(wnd, "NameBlock");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateAbonentType(AbonentTypeName, AbonentTypeMobile);
                        UpdateAllAbonentTypesView();
                        ShowMessageToUser(resultStr);

                        SetNullValuesToProperties();

                        wnd.Close();
                    }
                }
                );
            }
        }

        private RelayCommand addNewAbonent;
        public RelayCommand AddNewAbonent
        {
            get
            {
                return addNewAbonent ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    string resultStr = "";

                    resultStr = DataWorker.CreateAbonent(AbonentCountry, AbonentCity, AbonentPnumber, AbonentFax,
                            AbonentDescription, AbonentPtype, AbonentSecure);
                    UpdateAllAbonentsView();
                    ShowMessageToUser(resultStr);

                    SetNullValuesToProperties();

                    wnd.Close();
                }
                );
            }
        }
        #endregion

        //Команды удаления данных в БД
        #region COMMANDS TO DELETE
        private RelayCommand deleteItem;
        public RelayCommand DeleteItem
        {
            get
            {
                return deleteItem ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";

                    if (SelectedTabItem.Name == "AbonentsTab" && SelectedAbonent != null)
                    {
                        resultStr = DataWorker.DeleteAbonent(SelectedAbonent);
                        UpdateAllDataView();
                    }
                    if (SelectedTabItem.Name == "AbonentTypesTab" && SelectedAbonentType != null)
                    {
                        resultStr = DataWorker.DeleteAbonentType(SelectedAbonentType);
                        UpdateAllDataView();
                    }
                    SetNullValuesToProperties();
                    ShowMessageToUser(resultStr);
                }
                );
            }
        }
        #endregion

        //Команды редактирования данных в БД
        #region EDIT COMMAND
        private RelayCommand editAbonent;
        public RelayCommand EditAbonent
        {
            get
            {
                return editAbonent ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "Не выбран абонент";

                    if (SelectedAbonent != null)
                    {  
                        resultStr = DataWorker.EditAbonent(SelectedAbonent, AbonentCountry, AbonentCity, AbonentPnumber, AbonentFax, AbonentDescription, AbonentPtype, AbonentSecure);
                        UpdateAllDataView();
                        SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                        window.Close();
                    }
                    else ShowMessageToUser(resultStr);
                }
                );
            }
        }

        private RelayCommand editAbonentType;
        public RelayCommand EditAbonentType
        {
            get
            {
                return editAbonentType ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "Не выбран тип абонента";
                    if (SelectedAbonentType != null)
                    {                      
                        resultStr = DataWorker.EditAbonentType(SelectedAbonentType, AbonentTypeName, AbonentTypeMobile);
                        UpdateAllDataView();
                        SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                        window.Close();                        
                    }
                    else ShowMessageToUser(resultStr);
                }
                );
            }
        }



        #endregion

        //Методы открытия окон
        #region METHODS TO OPEN WINDOW

        private void OpenAddAbonentTypeWindowMethod()
        {
            AddNewAbonentTypeWindow newAbonentTypeWindow = new AddNewAbonentTypeWindow();
            SetCenterPositionAndOpen(newAbonentTypeWindow);
        }

        private void OpenAddAbonentWindowMethod()
        {
            AddNewAbonentWindow newAbonentWindow = new AddNewAbonentWindow();
            SetCenterPositionAndOpen(newAbonentWindow);
        }

        private void OpenEditAbonentTypeWindowMethod(AbonentType abonentType)
        {
            EditNewAbonentTypeWindow editAbonentTypeWindow = new EditNewAbonentTypeWindow(abonentType);
            SetCenterPositionAndOpen(editAbonentTypeWindow);
        }

        private void OpenEditAbonentWindowMethod(Abonent abonent)
        {
            EditNewAbonentWindow editAbonentWindow = new EditNewAbonentWindow(abonent);
            SetCenterPositionAndOpen(editAbonentWindow);
        }
        #endregion

        //Команды открытия окон
        #region COMMAND TO OPEN WINDOW
        private RelayCommand openAddNewAbonentTypeWnd;
        public RelayCommand OpenAddNewAbonentTypeWnd
        {
            get
            {
                return openAddNewAbonentTypeWnd ?? new RelayCommand(obj =>
                {
                    OpenAddAbonentTypeWindowMethod();
                }
                );
            }
        }

        private RelayCommand openAddNewAbonentWnd;
        public RelayCommand OpenAddNewAbonentWnd
        {
            get
            {
                return openAddNewAbonentWnd ?? new RelayCommand(obj =>
                {
                    OpenAddAbonentWindowMethod();
                }
                );
            }
        }

        private RelayCommand openEditItemWnd;
        public RelayCommand OpenEditItemWnd
        {
            get
            {
                return openEditItemWnd ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";

                    if (SelectedTabItem.Name == "AbonentsTab" && SelectedAbonent != null)
                    {
                        OpenEditAbonentWindowMethod(SelectedAbonent);

                    }
                    if (SelectedTabItem.Name == "AbonentTypesTab" && SelectedAbonentType != null)
                    {
                        OpenEditAbonentTypeWindowMethod(SelectedAbonentType);
                    }
                }
                );
            }
        }

        #endregion

        //Обновление окон
        #region UPDATE VIEWS

        private void UpdateAllDataView()
        {
            UpdateAllAbonentTypesView();
            UpdateAllAbonentsView();
        }

        private void UpdateAllAbonentTypesView()
        {
            AllAbonentTypes = DataWorker.GetAllAbonentTypes();
            MainWindow.AllAbonentTypesView.ItemsSource = null;
            MainWindow.AllAbonentTypesView.Items.Clear();
            MainWindow.AllAbonentTypesView.ItemsSource = AllAbonentTypes;
            MainWindow.AllAbonentTypesView.Items.Refresh();
        }

        private void UpdateAllAbonentsView()
        {
            AllAbonents = DataWorker.GetAllAbonents();
            MainWindow.AllAbonentsView.ItemsSource = null;
            MainWindow.AllAbonentsView.Items.Clear();
            MainWindow.AllAbonentsView.ItemsSource = AllAbonents;
            MainWindow.AllAbonentsView.Items.Refresh();
        }
        #endregion

        //Методы создания отчетов в Excel
        #region METHODS TO CREATE REPORT EXCEL
        private void AllCreateExcelReport()
        {
            Workbook workbook = new Workbook();
            CreateExcelRepport1(workbook);
            CreateExcelRepport2(workbook);
            CreateExcelRepport3(workbook);
            workbook.SaveToFile("Report.xlsx", ExcelVersion.Version2016);
            //System.Diagnostics.Process.Start("Report.xlsx");
        }

        private void CreateExcelRepport1(Workbook workbook)
        {
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Отчет 1";

            List<Report1> allReport1 = DataWorker.GetAllReport1();
            worksheet.Range[1, 1].Value = "Месяц";
            worksheet.Range[1, 2].Value = "Оператор";
            worksheet.Range[1, 3].Value = "Стоимость";
            int i = 1;
            decimal sumCost = 0;
            foreach (Report1 report1 in allReport1)
            {
                i++;
                worksheet.Range[i, 1].Value2 = report1.Month;
                worksheet.Range[i, 2].Value = report1.Name;
                worksheet.Range[i, 3].Value2 = report1.Cost;
                sumCost += report1.Cost;
            }
            worksheet.Range[i + 1, 3].Value = "Итого " + sumCost;
            worksheet.AllocatedRange.AutoFitColumns();
        }

        private void CreateExcelRepport2(Workbook workbook)
        {
            Worksheet worksheet = workbook.Worksheets[1];
            worksheet.Name = "Отчет 2";

            List<Report2> allReport2 = DataWorker.GetAllReport2();
            worksheet.Range[1, 1].Value = "Номер Абонента";
            worksheet.Range[1, 2].Value = "Оператор";
            worksheet.Range[1, 3].Value = "Описание";
            worksheet.Range[1, 4].Value = "Месяц";
            worksheet.Range[1, 5].Value = "Количество";
            worksheet.Range[1, 6].Value = "Стоимость";
            int i = 1;
            decimal sumCost = 0;
            foreach (Report2 report2 in allReport2)
            {
                i++;
                worksheet.Range[i, 1].Value2 = report2.Pnumber;
                worksheet.Range[i, 2].Value = report2.Name;
                worksheet.Range[i, 3].Value = report2.Service;
                worksheet.Range[i, 4].Value2 = report2.Timestamp;
                worksheet.Range[i, 5].Value = report2.Duration;
                worksheet.Range[i, 6].Value2 = report2.Cost;
                sumCost += report2.Cost;
            }
            worksheet.Range[i + 1, 6].Value = "Итого " + sumCost;

            worksheet.AllocatedRange.AutoFitColumns();    
        }

        private void CreateExcelRepport3(Workbook workbook)
        { 
            Worksheet worksheet = workbook.Worksheets[2];
            worksheet.Name = "Отчет 3";

            List<Report3> allReport3 = DataWorker.GetAllReport3();
            worksheet.Range[1, 1].Value = "Номер Абонента";
            worksheet.Range[1, 2].Value = "Оператор";
            worksheet.Range[1, 3].Value = "Описание";
            worksheet.Range[1, 4].Value = "Месяц";
            worksheet.Range[1, 5].Value = "Количество";
            worksheet.Range[1, 6].Value = "Стоимость";
            worksheet.Range[1, 7].Value = "НДС";
            worksheet.Range[1, 8].Value = "Стоимость с НДС";
            int i = 1;
            decimal sumCost = 0;
            decimal sumCostNds = 0;
            decimal sumSumCost = 0;
            foreach (Report3 report3 in allReport3)
            {
                i++;
                worksheet.Range[i, 1].Value2 = report3.Pnumber;
                worksheet.Range[i, 2].Value = report3.Name;
                worksheet.Range[i, 3].Value = report3.Service;
                worksheet.Range[i, 4].Value2 = report3.Timestamp;
                worksheet.Range[i, 5].Value = report3.Duration;
                worksheet.Range[i, 6].Value2 = report3.Cost;
                worksheet.Range[i, 7].Value2 = report3.CostNds;
                worksheet.Range[i, 8].Value2 = report3.SumCost;
                sumCost += report3.Cost;
                sumCostNds += report3.CostNds;
                sumSumCost += report3.SumCost;
            }
            worksheet.Range[i + 1, 5].Value = "Итого ";
            worksheet.Range[i + 1, 6].Value2 = sumCost;
            worksheet.Range[i + 1, 7].Value2 = sumCostNds;
            worksheet.Range[i + 1, 8].Value2 = sumSumCost;

            worksheet.AllocatedRange.AutoFitColumns();
        }

        #endregion

        //Метод закрашивает рамку textbox
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        //Метод открытия диологового окна
        private void ShowMessageToUser(string message)
        {
            MessageView messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }

        // Метод обнуления переменных
        private void SetNullValuesToProperties()
        {
            AbonentTypeName = null;
            AbonentTypeMobile = 0;

            AbonentCountry = 24;
            AbonentCity = 0;
            AbonentPnumber = 0;
            AbonentFax = 0;
            AbonentDescription = null;
            AbonentPtype = 0;
            AbonentSecure = 0;
        }

        // Метод центрирования окна
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        // Интерфейс изменения свойств обьектов
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
