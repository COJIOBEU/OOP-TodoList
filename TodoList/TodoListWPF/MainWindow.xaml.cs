using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TodoList.Models;
using TodoList.Services;
using TodoList.Repositories;

namespace TodoList
{
    public partial class MainWindow : Window
    {
        private TodoService _service;
        private ITaskRepository _repository;
        private const string FilePath = "tasks.json";

        public MainWindow()
        {
            InitializeComponent();

            _repository = new JsonTaskRepository(FilePath);
            _service = new TodoService();

            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _repository.Load();
            foreach (var task in tasks)
            {
                _service.AddTask(task);
            }
            TasksListBox.ItemsSource = _service.GetTasks();
        }

        private void SaveTasks()
        {
            _repository.Save(new List<TodoModel>(_service.GetTasks()));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newTask = new TodoModel(TitleTextBox.Text);
                newTask.Description = DescriptionTextBox.Text;
                newTask.DueTime = DueDatePicker.SelectedDate;
                newTask.Priority = GetSelectedPriority();

                _service.AddTask(newTask);
                SaveTasks();
                RefreshTasksList();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem is TodoModel selectedTask)
            {
                try
                {
                    _service.EditTask(selectedTask.Id, TitleTextBox.Text, DescriptionTextBox.Text,
                                      DueDatePicker.SelectedDate, GetSelectedPriority());
                    _service.ChangeStatus(selectedTask.Id, GetSelectedStatus());
                    SaveTasks();
                    RefreshTasksList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите задачу из списка!");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem is TodoModel selectedTask)
            {
                _service.DeleteTask(selectedTask.Id);
                SaveTasks();
                RefreshTasksList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Сначала выберите задачу из списка!");
            }
        }

        private void TasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksListBox.SelectedItem is TodoModel selectedTask)
            {
                TitleTextBox.Text = selectedTask.Title;
                DescriptionTextBox.Text = selectedTask.Description;
                DueDatePicker.SelectedDate = selectedTask.DueTime;

                SetComboBoxValue(PriorityComboBox, selectedTask.Priority.ToString());
                SetComboBoxValue(StatusComboBox, selectedTask.Status.ToString());
            }
        }

        private TaskPriority GetSelectedPriority()
        {
            if (PriorityComboBox.SelectedItem is ComboBoxItem item)
            {
                string val = item.Content.ToString();
                if (val == "High") return TaskPriority.High;
                if (val == "Low") return TaskPriority.Low;
            }
            return TaskPriority.Medium;
        }

        private TaskStatus GetSelectedStatus()
        {
            if (StatusComboBox.SelectedItem is ComboBoxItem item)
            {
                string val = item.Content.ToString();
                if (val == "InProcess") return TaskStatus.InProcess;
                if (val == "Done") return TaskStatus.Done;
            }
            return TaskStatus.NotStarted;
        }

        private void SetComboBoxValue(ComboBox box, string value)
        {
            foreach (ComboBoxItem item in box.Items)
            {
                if (item.Content.ToString() == value)
                {
                    box.SelectedItem = item;
                    return;
                }
            }
        }

        private void RefreshTasksList()
        {
            TasksListBox.ItemsSource = null;
            TasksListBox.ItemsSource = _service.GetTasks();
        }

        private void ClearForm()
        {
            TitleTextBox.Text = "";
            DescriptionTextBox.Text = "";
            DueDatePicker.SelectedDate = null;
            PriorityComboBox.SelectedIndex = 1;
            StatusComboBox.SelectedIndex = 0;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}