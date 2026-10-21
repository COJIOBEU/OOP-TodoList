using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TodoList.Models;

namespace TodoList.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions {WriteIndented = true, Converters = { new JsonStringEnumConverter()}};

        public JsonTaskRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(List<TodoModel> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, _options);
            File.WriteAllText(_filePath, json);
        }

        public List<TodoModel> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TodoModel>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TodoModel>>(json, _options) ?? new List<TodoModel>();
        }
    }
}