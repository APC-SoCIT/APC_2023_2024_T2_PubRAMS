using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

using webapp.Helpers;
using webapp.Models;
using webapp.Models.Dtos;
using webapp.Models.EntityManagers;
using webapp.Models.ViewModels;

namespace webapp.Controllers;

[AuthorizeForScopes(ScopeKeySection = "GraphApi:Scopes")]
public class GroupsController(IDownstreamApi graphApi) : Controller
{
    private readonly IDownstreamApi _graphApi = graphApi;

    public async Task<ActionResult> Index()
    {
        AuthHelper gh = new();
        IUser? user = await gh.RolesOnly([(int)Roles.Instructor]).GetUser(_graphApi);
        StaffManager staffManager = new();
        ViewData["User"] = user;
        ViewData["UserType"] = user?.GetType() == typeof(Student) ? "student" : "staff";
        ViewData["UserRoles"] = staffManager.GetRoles(user).Select(e => e.Id).ToList();
        if (user == null)
        {
            return Redirect("/");
        }

        GroupManager manager = new();
        GroupListViewModel model = manager.GenerateGroupListViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddGroupDto dto)
    {
        AuthHelper gh = new();
        IUser? user = await gh.RolesOnly([(int)Roles.Instructor]).GetUser(_graphApi);
        StaffManager staffManager = new();
        ViewData["User"] = user;
        ViewData["UserType"] = user?.GetType() == typeof(Student) ? "student" : "staff";
        ViewData["UserRoles"] = staffManager.GetRoles(user).Select(e => e.Id).ToList();
        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        GroupManager manager = new();
        Group group = manager.AddWithLeaderEmail(dto.Name, dto.LeaderEmail);
        GroupViewModel model = manager.GenerateGroupViewModel(group);

        return PartialView("/Views/Groups/_GroupPartial.cshtml", model);
    }

    [HttpPost]
    public async Task<ActionResult> AddMember(GroupMemberDto dto)
    {
        AuthHelper gh = new();
        IUser? user = await gh.RolesOnly([(int)Roles.Instructor]).GetUser(_graphApi);
        StaffManager staffManager = new();
        ViewData["User"] = user;
        ViewData["UserType"] = user?.GetType() == typeof(Student) ? "student" : "staff";
        ViewData["UserRoles"] = staffManager.GetRoles(user).Select(e => e.Id).ToList();
        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Group or user not found.");
        }

        GroupManager manager = new();
        Group group = manager.AddMemberByEmail(dto.GroupName, dto.StudentEmail);
        GroupViewModel model = manager.GenerateGroupViewModel(group);

        return PartialView("/Views/Groups/_GroupPartial.cshtml", model);
    }

    [HttpPost]
    public async Task<ActionResult> ChangeLeader(GroupMemberDto dto)
    {
        AuthHelper gh = new();
        IUser? user = await gh.StaffOnly().GetUser(_graphApi);
        StaffManager staffManager = new();
        ViewData["User"] = user;
        ViewData["UserType"] = user?.GetType() == typeof(Student) ? "student" : "staff";
        ViewData["UserRoles"] = staffManager.GetRoles(user).Select(e => e.Id).ToList();
        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Group or user not found.");
        }

        GroupManager manager = new();
        Group group = manager.ReassignLeaderByEmail(dto.GroupName, dto.StudentEmail);
        GroupViewModel model = manager.GenerateGroupViewModel(group);

        return PartialView("/Views/Groups/_GroupPartial.cshtml", model);
    }

    [HttpPost]
    public async Task<ActionResult> RemoveMember(GroupMemberDto dto)
    {
        AuthHelper gh = new();
        IUser? user = await gh.StaffOnly().GetUser(_graphApi);
        StaffManager staffManager = new();
        ViewData["User"] = user;
        ViewData["UserType"] = user?.GetType() == typeof(Student) ? "student" : "staff";
        ViewData["UserRoles"] = staffManager.GetRoles(user).Select(e => e.Id).ToList();
        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Group or user not found.");
        }

        GroupManager manager = new();
        Group group = manager.RemoveMemberByEmail(dto.GroupName, dto.StudentEmail);
        GroupViewModel model = manager.GenerateGroupViewModel(group);

        return PartialView("/Views/Groups/_GroupPartial.cshtml", model);
    }
}