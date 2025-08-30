using System.Reflection;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SprintPokerApi.Helpers;

public static class ModelBuilderXmlComments
{
    public static void ApplyXmlDocComments(this ModelBuilder modelBuilder, Assembly assembly, string? xmlPath = null)
    {
        xmlPath ??= Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (!File.Exists(xmlPath)) return;

        var doc = XDocument.Load(xmlPath);
        var members = doc.Descendants("member").ToDictionary(
            m => (string?)m.Attribute("name") ?? "",
            m => (string?)m.Element("summary")?.Value.Trim()
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
           ApplyTableComments(modelBuilder, entityType, members);
           ApplyPropertyComments(modelBuilder, entityType, members);
        }
    }

    private static string? Summary(string key, Dictionary<string, string?> members)
    {
        return members.TryGetValue(key, out var summary) ? 
            string.IsNullOrWhiteSpace(summary) ? 
                null : 
                summary
            : null;
    }
    
    private static void ApplyTableComments(ModelBuilder modelBuilder, IMutableEntityType entityType, Dictionary<string, string?> members)
    {
        var clrType = entityType.ClrType;
            
        // Table comments from Model class comments
        var typeKey = $"T:{clrType.FullName}";
        var typeSummary = Summary(typeKey, members);
        if (!string.IsNullOrWhiteSpace(typeSummary))
        {
            modelBuilder.Entity(clrType).ToTable(tb => tb.HasComment(typeSummary));
        }
    }

    private static void ApplyPropertyComments(ModelBuilder modelBuilder, IMutableEntityType entityType,
        Dictionary<string, string?> members)
    {
        // Column comments from Model property comments
        foreach (var prop in entityType.GetProperties())
        {
            var propInfo = prop.PropertyInfo;
            if (propInfo == null) continue;

            var propKey = $"P:{propInfo.DeclaringType!.FullName}.{propInfo.Name}";
            var propSummary = Summary(propKey, members);
            if (!string.IsNullOrWhiteSpace(propSummary))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(propInfo.Name)
                    .HasComment(propSummary);
            }
        }
    }
}