using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Education.Domain.Entities;
using Education.Domain.Entities.Base;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        // Устанавливаем первичный ключ по Id
        builder.HasKey(x => x.Id);

        // Указываем, что Id будет сгенерирован автоматически при добавлении
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Настраиваем маппинг значения GroupName через ValueObject
        builder.Property(x => x.Name)
.HasConversion(                       // преобразование значения при сохранении/чтении
                name => name.Value,              // из GroupName в string (для БД)
                name => new GroupName(name))     // из string обратно в GroupName (для модели)
            .IsRequired()                        // значение обязательно
            .HasMaxLength(10);                   // ограничение длины строки (на уровне БД)

        // Создаём уникальный индекс по имени группы
        builder.HasIndex(x => x.Name).IsUnique();

        // Один ко многим: группа имеет много студентов, каждый студент принадлежит одной группе
        builder.HasMany(x => x.Students)
.WithOne(x => x.Group);           // навигационное свойство в Student
    }
}
