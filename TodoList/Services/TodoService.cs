using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Models;

namespace TodoList.Services
{
    public class TodoService
    {
        private readonly List<TodoModel> _tasks = new List<TodoModel>();

        public void AddTask(TodoModel task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Пустая задача.");
            }
            _tasks.Add(task);
        }

        private TodoModel GetTask(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
        public void EditTask(Guid id, string newTitle, string newDescription, DateTime? newDueTime, TaskPriority newPriority)
        {
            var task = GetTask(id);
            if (task == null)
            {
                throw new InvalidOperationException("Задача не найдена.");
            }
            
            task.Title = newTitle;
            task.Description = newDescription;
            task.DueTime = newDueTime;
            task.Priority = newPriority;
        }

        public void DeleteTask(Guid id)
        {
            var task = GetTask(id);
            if (task == null)
            {
                throw new InvalidOperationException("Задача не найдена.");
            }
            _tasks.Remove(task);
        }

        public void ChangeStatus(Guid id, TaskStatus newStatus)
        {
            var task = GetTask(id);
            if (task == null)
            {
                throw new InvalidOperationException("Задача не найдена.");
            }
            task.Status = newStatus;
        }

        public IReadOnlyList<TodoModel> GetTasks()
        {
            return _tasks.AsReadOnly();
        }
    }
}
