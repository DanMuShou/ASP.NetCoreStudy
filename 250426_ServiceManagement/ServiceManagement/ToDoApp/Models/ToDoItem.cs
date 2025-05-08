using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string? Name { get; set; }

    private bool _isComplete;
    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            _isComplete = value;
            if (_isComplete)
            {
                CompleteTime = DateTime.Now;
            }
        }
    }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime CompleteTime { get; set; }
}
