using Education.Domain.Exceptions;
using Education.Domain.ValueObjects;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Domain.Entities;

/// <summary>
/// Хранилище шаблонов домашних заданий, привязанных к темам уроков.
/// Используется как owned entity внутри Teacher.
/// </summary>
public class HomeworkBank
{
    #region Свойства
    private readonly ICollection<HomeworkTemplate> _templates = new List<HomeworkTemplate>();

    /// <summary>
    /// Получить все шаблоны в виде коллекции (не маппится EF)
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<HomeworkTemplate> Templates => new ReadOnlyCollection<HomeworkTemplate>(_templates.ToList());
    #endregion

    #region Конструкторы
    /// <summary>
    /// Только для EF
    /// </summary>
    private HomeworkBank() { }

    public HomeworkBank(bool createEmpty = true)
    {
    }

    /// <summary>
    /// Конструктор для восстановления из БД 
    /// </summary>
    protected HomeworkBank(ICollection<HomeworkTemplate> templates)
    {
        _templates = templates ?? throw new TemplatesIsNullException();
    }

    #endregion

    #region Методы
    public void AddTemplate(LessonTopic topic, HomeworkTitle title)
    {
        if (title == null)
            throw new HomeworkTitleIsNullsException();

        if (_templates.Any(t => t.Topic.Equals(topic)))
            throw new DuplicateHomeworkTemplateException(topic);

        _templates.Add(new HomeworkTemplate(topic, title));
    }

    public HomeworkTemplate? FindTemplate(LessonTopic topic)
    {
        return _templates.FirstOrDefault(t => t.Topic.Equals(topic));
    }

    public bool RemoveTemplate(LessonTopic topic)
    {
        var template = _templates.FirstOrDefault(t => t.Topic.Equals(topic));
        return template != null && _templates.Remove(template);
    }
    #endregion
}


