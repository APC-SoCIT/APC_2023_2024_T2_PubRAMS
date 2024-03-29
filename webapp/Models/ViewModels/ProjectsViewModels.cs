namespace webapp.Models.ViewModels;

public class ProjectViewModel
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Abstract { get; set; }

    public required string Tags { get; set; }

    public required string Category { get; set; }

    public required StateViewModel State { get; set; }

    public required GroupViewModel Group { get; set; }

    public required bool HasPrf { get; set; }

    public required bool HasPdf { get; set; }

    public required string School { get; set; }

    public required string Subject { get; set; }

    public required string Course { get; set; }

    public string? StaffComment { get; set; }

    public string? StudentComment { get; set; }

    // Used to determine the action column (we'll make a form component for it)
    public string? Action { get; set; }

    public DateTime? Deadline { get; set; }
}

public class ProjectListViewModel
{
    public required List<ProjectViewModel> UrgentProjects { get; set; }

    public required List<ProjectViewModel> Projects { get; set; }
}