using System.Collections.Generic;
using SQLite;
using TaskyPortableStandardLibrary;

namespace Tasky.PortableStandardLibrary
{
    public class TodoItemRepository
    {
        TodoDatabase db = null;
        IEventHubSender _eventHubSender = null;

        public TodoItemRepository(SQLiteConnection conn)
        {
            db = new TodoDatabase(conn);
            _eventHubSender = new EventHubSender();
        }

        public TodoItem GetTask(int id)
        {
            return db.GetItem(id);
        }

        public IEnumerable<TodoItem> GetTasks()
        {
            return db.GetItems();
        }

        public int SaveTask(TodoItem item)
        {
            _eventHubSender.Send($"saved {item.Name}");
            return db.SaveItem(item);
        }

        public int DeleteTask(int id)
        {
            return db.DeleteItem(id);
        }
    }
}
