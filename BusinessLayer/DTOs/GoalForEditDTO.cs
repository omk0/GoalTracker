﻿using BusinessLayer.DTOs.GoalCreationDTO;
using DataAccessLayer.Models;

namespace BusinessLayer.DTOs
{
    public class GoalForEditDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplex { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string DateOfBeginning { get; set; } = string.Empty;
        public string DateOfEnding { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public IList<MemberForCreationDTO>? MembersIds { get; set; } = new List<MemberForCreationDTO>();
        public IList<Subgoal>? SubGoals { get; set; } = new List<Subgoal>();
        public IList<GoalTask>? Tasks { get; set; } = new List<GoalTask>();
    }
}
