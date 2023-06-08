using Microsoft.AspNetCore.Mvc.Rendering;

public class RoleEditViewModel
    {
        public string RoleName { get; set; }
        public string NewRoleName { get; set; }
        public IEnumerable<SelectListItem> RoleNames { get; set; }
    }
