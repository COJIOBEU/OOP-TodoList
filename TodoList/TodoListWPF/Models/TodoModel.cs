using System;

namespace TodoList.Models
{
    public enum TaskStatus
    {
        NotStarted,
        InProcess,
        Done
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    public class TodoModel
    {
        private Guid _id;
        private string _title;
        private string _description = string.Empty;
        private DateTime _created;
        private DateTime? _dueTime;
        private TaskPriority _priority;
        private TaskStatus _status;

        public Guid Id
        {
            get => _id;
            private set => _id = value;
        }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название задачи не может быть пустым.");
                }
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value ?? string.Empty;
            }
        }

        public DateTime Created
        {
            get => _created;
            private set => _created = value;
        }

        public DateTime? DueTime
        {
            get => _dueTime;

            set
            {
                if (value.HasValue && value.Value < _created)
                {
                    throw new ArgumentException("Срок выполнения не может быть раньше даты создания.");
                }
                _dueTime = value;
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public TaskStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public TodoModel(string title)
        {
            _id = Guid.NewGuid();
            Title = title;
            _created = DateTime.Now;
            _priority = TaskPriority.Medium;
            _status = TaskStatus.NotStarted;
        }

        public TodoModel()
        {

        }
    }
}
