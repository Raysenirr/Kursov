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
    /// <summary> Внутренний список шаблонов домашних заданий </summary>
    private readonly ICollection<HomeworkTemplate> _templates = [];

    /// <summary> Коллекция шаблонов, доступная только для чтения (не сохраняется в БД) </summary>
    [NotMapped]
    public IReadOnlyCollection<HomeworkTemplate> Templates => [.. _templates];
    #endregion

    #region Конструкторы
    /// <summary>
    /// Только для EF
    /// </summary>
    private HomeworkBank() { }

    public HomeworkBank(bool createEmpty = true)
    {
    }

    protected HomeworkBank(ICollection<HomeworkTemplate> templates)
    {
        _templates = templates ?? throw new TemplatesIsNullException();
    }

    #endregion

    #region Методы
    /// <summary> Добавляет новый шаблон домашнего задания по теме урока, если такого ещё нет </summary>
    public void AddTemplate(LessonTopic topic, HomeworkTitle title)
    {
        if (title == null)
            throw new HomeworkTitleIsNullsException();

        if (_templates.Any(t => t.Topic.Equals(topic)))
            throw new DuplicateHomeworkTemplateException(topic);

        _templates.Add(new HomeworkTemplate(topic, title));
    }
    /// <summary> Ищет шаблон домашнего задания по заданной теме урока </summary>
    public HomeworkTemplate? FindTemplate(LessonTopic topic)
    {
        return _templates.FirstOrDefault(t => t.Topic.Equals(topic));
    }
    /// <summary> Удаляет шаблон домашнего задания по теме, если он существует </summary>
    public bool RemoveTemplate(LessonTopic topic)
    {
        var template = _templates.FirstOrDefault(t => t.Topic.Equals(topic));
        return template != null && _templates.Remove(template);
    }
    #endregion
}


