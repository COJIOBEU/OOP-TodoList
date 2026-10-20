using System;
using System.IO;
using TodoList.Models;
using TodoList.Services;
using TodoList.Repositories;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "tasks.json";

            ITaskRepository repository = new JsonTaskRepository(filePath);

            var loadedTasks = repository.Load();
            Console.WriteLine($"Загружено задач при старте: {loadedTasks.Count}");

            var service = new TodoService();
            service.AddTask(new TodoModel("Сделать лабу по ООП"));
            service.AddTask(new TodoModel("Купить продукты"));

            repository.Save(new List<TodoModel>(service.GetTasks()));
            Console.WriteLine("Задачи сохранены в файл tasks.json!");

            var newService = new TodoService();
            var savedTasks = repository.Load();

            Console.WriteLine($"\nПосле 'перезапуска' загружено задач: {savedTasks.Count}");
            foreach (var task in savedTasks)
            {
                Console.WriteLine($"- {task.Title} (Создана: {task.Created})");
            }

            Console.WriteLine("\nНажми любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}