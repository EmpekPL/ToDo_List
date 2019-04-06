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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<String> taskList = new List<String>();
        static int y = 0;
        string path;
        public MainWindow()
        {
            InitializeComponent();
            addBtn.Content = "\u25b6";
            saveBtn.Content = "save";

            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += "\\ToDoList.txt";

            if (!File.Exists(path)) {
                File.Create(path);
            }

            LoadTasks();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            
            String x = addTextBox.Text;
            
            addTextBox.Text = "";
            if (x.Length > 0) { 
            createNewTask(x);
            }


        }

        private void createNewTask(String taskContent)
        {
            taskList.Add(taskContent);

            //create container for task and Btn 
            StackPanel newSP = new StackPanel();
            newSP.Name = "stack" + y;
            newSP.Orientation = Orientation.Horizontal;
            newSP.MinWidth = 300;


            //create textBlock for task text
            TextBlock newTB = new TextBlock();
            newTB.Text = taskContent;
            newTB.Name = "task" + y;
            newTB.Foreground = new SolidColorBrush(Colors.RoyalBlue);
            newTB.Padding = new Thickness(10);
            newTB.FontSize = 16;
            newTB.Width = 220;
            newTB.TextWrapping = TextWrapping.Wrap;
            RegisterName(newTB.Name, newTB);

            //create done btn
            Button doneTaskBtn = new Button();
            doneTaskBtn.Content = "\u2713";
            doneTaskBtn.HorizontalAlignment = HorizontalAlignment.Right;
            doneTaskBtn.Height = 20;
            doneTaskBtn.Width = 20;
            doneTaskBtn.Click += (se, ev) => copleteTask(se, ev);

            //create delete btn
            Button deleteTaskBtn = new Button();
            deleteTaskBtn.Content = "\u2715";
            deleteTaskBtn.HorizontalAlignment = HorizontalAlignment.Right;
            deleteTaskBtn.Height = 20;
            deleteTaskBtn.Width = 20;
            deleteTaskBtn.Click += (se, ev) => deleteTask(se, ev);

            // add created elements
            taskListStack.Children.Add(newSP);
            newSP.Children.Add(newTB);
            newSP.Children.Add(doneTaskBtn);
            newSP.Children.Add(deleteTaskBtn);
           
            y++;
        }

        protected void deleteTask(object sender, EventArgs e) {
            FrameworkElement parent =  (FrameworkElement)((Button)sender).Parent;
            string taskName = parent.Name; 
            char lastS = taskName[taskName.Length - 1];   //last char = inxex number of task       
            int last = (int)Char.GetNumericValue(lastS); // char => int

            //Delete task and null list item
            taskList.RemoveAt(last);
            taskList.Insert(last, null);    // clear value in List without mixing index List
            taskListStack.Children.Remove(parent);
        }

        protected void copleteTask(object sender, EventArgs e)
        {
            FrameworkElement parent = (FrameworkElement)((Button)sender).Parent;

            string taskBoxName = parent.Name;
            char last = taskBoxName[taskBoxName.Length-1 ];
            string taskBoxNameReady = "task" + last;
            
            //switch between done and active task
            var txtblock = (TextBlock)this.FindName(taskBoxNameReady);
            if(txtblock.TextDecorations == TextDecorations.Strikethrough)
            {
                txtblock.TextDecorations = null;
            }
            else { 
            txtblock.TextDecorations = TextDecorations.Strikethrough;
            }
        }

        //Key Enter automaticlly add task without click button
        private void EnterAutoAdd(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(addBtn, e);
            }
        }
        
        //Save all not empy task to the file
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (string line in taskList)
                {
                    if (line != null)
                    {
                        file.WriteLine(line);
                    }
                    }
            }
        }

        // at the beginning of app, all task are loading from file
        private void LoadTasks()
        {
            if (new FileInfo(path).Length==0)
            {
                return;
            }
            else {
                List<string> loadedTasks = new List<string>();

                using (StreamReader sr = File.OpenText(path))
                {
                    string taskText = "";
                    while ((taskText = sr.ReadLine()) != null)
                    {
                        createNewTask(taskText);
                    }
                }
            }
        }
    }
}
