using System.Reflection;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;

namespace SprintPokerApi.Helpers;

/// <summary>
/// Provides functionality to apply XML documentation comments from code to database objects during Entity Framework model building.
/// This class reads XML documentation files and applies the comments to corresponding database tables and columns.
/// </summary>
public class ModelBuilderXmlComments
{
    private readonly ModelBuilder _modelBuilder;
    private readonly Dictionary<string, string?> _members = new();
    
    /// <summary>
    /// Initializes a new instance of the ModelBuilderXmlComments class.
    /// </summary>
    /// <param name="modelBuilder">The Entity Framework model builder instance.</param>
    /// <param name="assembly">The assembly containing the entity models.</param>
    /// <param name="xmlPath">Optional path to the XML documentation file. If not provided, uses the default path based on the assembly name.</param>
    public ModelBuilderXmlComments(ModelBuilder modelBuilder, Assembly assembly, string? xmlPath = null)
    {
        _modelBuilder = modelBuilder;
        
        xmlPath ??= Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (File.Exists(xmlPath))
        {
            var doc = XDocument.Load(xmlPath);
            _members = doc.Descendants("member").ToDictionary(
                member => member.Attribute("name")?.Value ?? "",
                member => member.Element("summary")?.Value.Trim()
            );
        }
    }

    /// <summary>
    /// Applies XML documentation comments to all entities and their properties in the model.
    /// If no XML documentation is available, this method does nothing.
    /// </summary>
    public void ApplyXmlDocComments()
    {
        if (_members.IsNullOrEmpty()) return;
        
        foreach (var entityType in _modelBuilder.Model.GetEntityTypes())
        {
           ApplyTableComments(entityType);
           ApplyPropertyComments(entityType);
        }
    }

    /// <summary>
    /// Retrieves the summary comment for a given member key from the XML documentation.
    /// </summary>
    /// <param name="key">The XML documentation key for the member.</param>
    /// <returns>The summary text if found and not empty; otherwise null.</returns>
    private string? Summary(string key)
    {
        return _members.TryGetValue(key, out var summary) ? 
            string.IsNullOrWhiteSpace(summary) ? 
                null : 
                summary
            : null;
    }
    
    /// <summary>
    /// Applies XML documentation comments to database tables based on entity class documentation.
    /// </summary>
    /// <param name="entityType">The entity type to apply comments to.</param>
    private void ApplyTableComments(IMutableEntityType entityType)
    {
        var clrType = entityType.ClrType;
        var typeKey = $"T:{clrType.FullName}";
        var typeSummary = Summary(typeKey);
        
        if (!string.IsNullOrWhiteSpace(typeSummary))
        {
            _modelBuilder.Entity(clrType).ToTable(tb => tb.HasComment(typeSummary));
        }
    }

    /// <summary>
    /// Applies XML documentation comments to database columns based on entity property documentation.
    /// </summary>
    /// <param name="entityType">The entity type whose properties should receive comments.</param>
    private void ApplyPropertyComments(IMutableEntityType entityType)
    {
        foreach (var prop in entityType.GetProperties())
        {
            var propInfo = prop.PropertyInfo;
            if (propInfo == null) continue;

            var propKey = $"P:{propInfo.DeclaringType!.FullName}.{propInfo.Name}";
            var propSummary = Summary(propKey);
            
            if (!string.IsNullOrWhiteSpace(propSummary))
            {
                _modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(propInfo.Name)
                    .HasComment(propSummary);
            }
        }
    }
}