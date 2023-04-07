﻿using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.DTOs.GoalCreationDTO;
using BusinessLayer.DTOs.GoalsGetting;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class GoalService : IGoalService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.GoalContext _context;
        private readonly IMapper _mapper;

        public GoalService(DataAccessLayer.Data.GoalContext context,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<GoalForCreationDTO> CreateGoal(GoalForCreationDTO goal)
        {
            var mainGoal = _mapper.Map<Goal>(goal);
            _context.GoalList.Add(mainGoal);
            await _context.SaveChangesAsync();
            if (!goal.IsComplex) return goal;

            if (goal.SubGoals != null)
                foreach (var subGoal in goal.SubGoals)
                {
                    var mappedSubGoal = _mapper.Map<Goal>(subGoal);
                    mappedSubGoal.MainGoalId = mainGoal.Id;
                    _context.GoalList.Add(mappedSubGoal);
                }
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<GoalsListForGettingDTO> GetGoals()
        {
            GoalsListForGettingDTO goalsList = new();
            foreach (var goal in _context.GoalList)
            {
                bool isSubgoal = goal.MainGoalId != null;
                var subgoalsOfCurrentGoal = await _context.GoalList.Where(item => item.MainGoalId == goal.Id)?.ToListAsync();
                bool isComplexGoal = subgoalsOfCurrentGoal.Count != 0;
                if (!isSubgoal)
                {
                    var goalToGet = _mapper.Map<GoalForGettingDTO>(goal);

                    goalToGet.Creator = new UserForGettingDTO(goal.CreatorId!);
                    var members = await _context.MembersIds.Where(member => member.GoalId == goal.Id).ToListAsync();
                    goalToGet.Members =
                        members.Select(member => new UserForGettingDTO(member.MemberId)).ToList();

                    if (!isComplexGoal)
                    {
                        var tasksForCurrentGoal =
                            await _context.GoalTasks.Where(task => task.GoalId == goal.Id).ToListAsync();
                        foreach (var task in tasksForCurrentGoal)
                        {
                            goalToGet.Tasks.Add(_mapper.Map<GoalTaskDTO>(task));
                        }

                    }
                    else
                    {
                        foreach (Goal subgoal in subgoalsOfCurrentGoal)
                        {
                            var subgoalToGet = _mapper.Map<SubgoalDTO>(subgoal);
                            subgoalToGet.Tasks = _mapper.Map<List<GoalTaskDTO>>(await _context.GoalTasks.Where(task => task.GoalId == subgoal.Id)
                                .ToListAsync());

                            goalToGet.Subgoals.Add(subgoalToGet);
                        }
                    }
                    goalsList.Goals.Add(goalToGet);
                }
            }
            return goalsList;
        }
    }
}