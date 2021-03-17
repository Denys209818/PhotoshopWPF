using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniPhotoshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Весь код прокоментований!
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Встановлення дефолтного режиму для InkCanvas (режим малювання)
            this.inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            //  Встановлення першого елемента ComboBox активним
            this.comboColor.SelectedIndex = 0;
            //  Встановлення першого чекбокса активним
            this.inkRadio.IsChecked = true;
        }

        private void RadioButtonClick(object sender, RoutedEventArgs e) 
        {
            //  вибірка активного RadioButton через sender
            switch ((sender as RadioButton)?.Content) 
            {
                //  Визначення який RadioButton був нажатий через Content RadioButton
                case "Ink Mode!": 
                    {
                        this.inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                        break; 
                    }
                case "Erase Mode!": 
                    {
                        this.inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                        break; 
                    }
                case "Select Mode!": 
                    {
                        this.inkCanvas.EditingMode = InkCanvasEditingMode.Select;
                        break; 
                    }
            }
        }

        private void SelectedItemChanged(object sender, SelectionChangedEventArgs e) 
        {
            //  Визначення кольору елемента через Tag ComboBoxItem, оскільки в ComboBoxItem
            //  Вложений StackPanel і можна лише використати службову властивість Tag у яку записується колір
            string color = ((sender as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString();
            //  Встановлення дефолтного кольору який вибирається через метод службового класа ColorConverter,
            //  який визначає колір через строку і повертає object, який конвертується у тип Color
            this.inkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(color);
        }

        private void btnSave(object sender, RoutedEventArgs e) 
        {
            //  Встановлення потоку для роботи з файлом StrokeData.bin
            using (FileStream fs = new FileStream("StrokeData.bin", FileMode.OpenOrCreate)) 
            {
                //  Збереження маисву Strokes, у якому зберігаються усі начерки у файл, як зображення
                this.inkCanvas.Strokes.Save(fs);
                //  Закритя файлу
                fs.Close();
            }
        }
        private void btnLoad(object sender, RoutedEventArgs e) 
        {
            //  Встановлення потоку для роботи з файлом StrokeData.bin
            using (FileStream fs = new FileStream("StrokeData.bin", FileMode.OpenOrCreate, FileAccess.Read)) 
            {
                //  Витягування усіх начерків у колекцію типу StrokeCollection з потока
                StrokeCollection coll = new StrokeCollection(fs);
                //  Присвоєння пустій колекції елемента InkCanvas колекцію начерків, які було взято з файла
                this.inkCanvas.Strokes = coll;
                //  Закритя файла
                fs.Close();
            }
        }
        private void btnClear(object sender, RoutedEventArgs e) 
        {
            //  Очищення усіх начерків з елемента InkCanvas
            this.inkCanvas.Strokes.Clear();
        }

    }
}
