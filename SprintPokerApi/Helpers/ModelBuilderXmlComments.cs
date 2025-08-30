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

        string? Summary(string key) =>
            members.TryGetValue(key, out var s) ? string.IsNullOrWhiteSpace(s) ? null : s : null;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            
            // Table comments from Model class comments
            var typeKey = $"T:{clrType.FullName}";
            var typeSummary = Summary(typeKey);
            if (!string.IsNullOrWhiteSpace(typeSummary))
            {
                modelBuilder.Entity(clrType).ToTable(tb => tb.HasComment(typeSummary));
            }

            // Column comments from Model property comments
            foreach (var prop in entityType.GetProperties())
            {
                var pi = prop.PropertyInfo;
                if (pi == null) continue;

                var propKey = $"P:{pi.DeclaringType!.FullName}.{pi.Name}";
                var propSummary = Summary(propKey);
                if (!string.IsNullOrWhiteSpace(propSummary))
                {
                    modelBuilder.Entity(clrType).Property(pi.Name).HasComment(propSummary);
                }
            }
        }
    }

    // private static void Summary(string key)
    // {
    //     
    // }
    //
    // private static void ApplyCommentToEntityType(IMutableEntityType entityType)
    // {
    // }
}